// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections;
using System.ComponentModel;

namespace System.Windows.Forms;

/// <summary>
///  Represents the child version of the System.Windows.Forms.ListManager
///  that is used when a parent/child relationship exists in a System.Windows.Forms.DataSet.
/// </summary>
internal class RelatedCurrencyManager : CurrencyManager
{
    private BindingManagerBase _parentManager;
    private PropertyDescriptor _fieldInfo;
    private static List<BindingManagerBase> IgnoreItemChangedTable { get; } = [];

    /// <summary>
    ///  Opt-in switch that controls which parent event drives the refresh of related
    ///  currency managers.
    /// </summary>
    /// <remarks>
    ///  <para>
    ///   When <see langword="false"/> (the default) a <see cref="RelatedCurrencyManager"/>
    ///   refreshes from the parent manager's <see cref="BindingManagerBase.CurrentItemChanged"/>
    ///   event, which is the standard framework behavior. <see cref="BindingManagerBase.CurrentItemChanged"/>
    ///   fires on both parent position changes and parent item-property edits.
    ///  </para>
    ///  <para>
    ///   When set to <see langword="true"/> the related manager instead refreshes from the
    ///   parent's <see cref="BindingManagerBase.CurrentChanged"/> event, which fires only on
    ///   parent position changes — not on item-property edits. This is the supported .NET 10
    ///   replacement for a reflection-based event swap that consumers such as CargoWise's
    ///   <c>ZBindingContext</c> previously performed, which is no longer viable because the
    ///   <see cref="BindingContext"/> list-manager storage can no longer be intercepted.
    ///  </para>
    ///  <para>
    ///   This switch is process-wide and must be set before related currency managers are
    ///   created (for example, in the binding context constructor) so that the desired event is
    ///   wired up from the start. Changing it after managers have been created only affects
    ///   managers that are created or rebound afterwards.
    ///  </para>
    ///  <para>
    ///   Example — setting the flag once in a custom binding context constructor
    ///   (the .NET 10 replacement for the <c>ZBindingContext</c> reflection hack):
    ///  </para>
    ///  <code language="csharp">
    ///   public class ZBindingContext : BindingContext
    ///   {
    ///       public ZBindingContext()
    ///       {
    ///           // .NET 10 replacement for the old reflection hack: drive related currency
    ///           // managers off the parent's CurrentChanged event instead of CurrentItemChanged.
    ///           RelatedCurrencyManager.UseParentCurrentChanged = true;
    ///       }
    ///   }
    ///  </code>
    /// </remarks>
    internal static bool UseParentCurrentChanged { get; set; }

    internal RelatedCurrencyManager(BindingManagerBase parentManager, string dataField)
        : base(dataSource: null)
    {
        Bind(parentManager, dataField);
    }

    [MemberNotNull(nameof(_parentManager))]
    [MemberNotNull(nameof(_fieldInfo))]
    internal void Bind(BindingManagerBase parentManager, string dataField)
    {
        Debug.Assert(parentManager is not null, "How could this be a null parentManager.");

        // Unwire previous BindingManagerBase
        UnwireParentManager(_parentManager);

        _parentManager = parentManager;
        _fieldInfo = parentManager.GetItemProperties().Find(dataField, ignoreCase: true)!;
        if (_fieldInfo is null || !typeof(IList).IsAssignableFrom(_fieldInfo.PropertyType))
        {
            throw new ArgumentException(string.Format(SR.RelatedListManagerChild, dataField));
        }

        finalType = _fieldInfo.PropertyType;

        // Wire new BindingManagerBase
        WireParentManager(_parentManager);

        ParentManager_CurrentItemChanged(parentManager, EventArgs.Empty);
    }

    private void UnwireParentManager(BindingManagerBase? bmb)
    {
        if (bmb is not null)
        {
            // Detach from whichever parent event is being used to drive the refresh. The
            // selection mirrors WireParentManager so that wire/unwire stay symmetrical even if
            // the switch is toggled between calls.
            if (UseParentCurrentChanged)
            {
                bmb.CurrentChanged -= ParentManager_CurrentItemChanged;
            }
            else
            {
                bmb.CurrentItemChanged -= ParentManager_CurrentItemChanged;
            }

            if (bmb is CurrencyManager currencyManager)
            {
                currencyManager.MetaDataChanged -= ParentManager_MetaDataChanged;
            }
        }
    }

    private void WireParentManager(BindingManagerBase bmb)
    {
        if (bmb is not null)
        {
            // By default we refresh from CurrentItemChanged (standard framework behavior).
            // When UseParentCurrentChanged is enabled we refresh from CurrentChanged instead,
            // which only fires on parent position changes (not on item-value edits). This is the
            // supported replacement for the reflection-based event swap previously performed by
            // CargoWise's ZBindingContext.
            if (UseParentCurrentChanged)
            {
                bmb.CurrentChanged += ParentManager_CurrentItemChanged;
            }
            else
            {
                bmb.CurrentItemChanged += ParentManager_CurrentItemChanged;
            }

            if (bmb is CurrencyManager currencyManager)
            {
                currencyManager.MetaDataChanged += ParentManager_MetaDataChanged;
            }
        }
    }

    internal override PropertyDescriptorCollection GetItemProperties(PropertyDescriptor[]? listAccessors)
    {
        PropertyDescriptor[] accessors;

        if (listAccessors is not null && listAccessors.Length > 0)
        {
            accessors = new PropertyDescriptor[listAccessors.Length + 1];
            listAccessors.CopyTo(accessors, 1);
        }
        else
        {
            accessors = new PropertyDescriptor[1];
        }

        // Set this accessor (add to the beginning)
        accessors[0] = _fieldInfo;

        // Get props
        return _parentManager.GetItemProperties(accessors);
    }

    /// <summary>
    ///  Gets the properties of the item.
    /// </summary>
    public override PropertyDescriptorCollection GetItemProperties()
    {
        return GetItemProperties(listAccessors: null);
    }

    /// <summary>
    ///  Gets the name of the list.
    /// </summary>
    internal override string GetListName()
    {
        string name = GetListName([]);
        if (name.Length > 0)
        {
            return name;
        }

        return base.GetListName();
    }

    /// <summary>
    ///  Gets the name of the specified list.
    /// </summary>
    protected internal override string GetListName(ArrayList? listAccessors)
    {
        if (listAccessors is null)
        {
            return string.Empty;
        }

        listAccessors.Insert(0, _fieldInfo);
        return _parentManager.GetListName(listAccessors);
    }

    private void ParentManager_MetaDataChanged(object? sender, EventArgs e)
    {
        // Propagate MetaDataChanged events from the parent manager
        OnMetaDataChanged(e);
    }

    private void ParentManager_CurrentItemChanged(object? sender, EventArgs e)
    {
        if (IgnoreItemChangedTable.Contains(_parentManager))
        {
            return;
        }

        int oldlistposition = listposition;

        // we only pull the data from the controls into the backEnd. we do not care about keeping the lastGoodKnownRow
        // when we are about to change the entire list in this currencymanager.
        try
        {
            PullData();
        }
        catch (Exception ex)
        {
            OnDataError(ex);
        }

        if (_parentManager is CurrencyManager currencyManager)
        {
            if (currencyManager.Count > 0)
            {
                // Parent list has a current row, so get the related list from the relevant property on that row.
                SetDataSource(_fieldInfo.GetValue(currencyManager.Current));
                listposition = (Count > 0 ? 0 : -1);
            }
            else
            {
                // APPCOMPAT: bring back the Everett behavior where the currency manager adds an item and
                // then it cancels the addition.
                //
                // really, really hocky.
                // will throw if the list in the curManager is not IBindingList
                // and this will fail if the IBindingList does not have list change notification. read on....
                // when a new item will get added to an empty parent table,
                // the table will fire OnCurrentChanged and this method will get executed again
                // allowing us to set the data source to an object with the right properties (so we can show
                // metadata at design time).
                // we then call CancelCurrentEdit to remove the dummy row, but making sure to ignore any
                // OnCurrentItemChanged that results from this action (to avoid infinite recursion)
                currencyManager.AddNew();
                try
                {
                    IgnoreItemChangedTable.Add(currencyManager);
                    currencyManager.CancelCurrentEdit();
                }
                finally
                {
                    if (IgnoreItemChangedTable.Contains(currencyManager))
                    {
                        IgnoreItemChangedTable.Remove(currencyManager);
                    }
                }
            }
        }
        else
        {
            // Case where the parent is not a list, but a single object
            SetDataSource(_fieldInfo.GetValue(_parentManager.Current));
            listposition = (Count > 0 ? 0 : -1);
        }

        if (oldlistposition != listposition)
        {
            OnPositionChanged(EventArgs.Empty);
        }

        OnCurrentChanged(EventArgs.Empty);
        OnCurrentItemChanged(EventArgs.Empty);
    }
}
