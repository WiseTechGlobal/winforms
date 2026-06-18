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
    private readonly EventHandler? _interceptedParentHandler;
    private static List<BindingManagerBase> IgnoreItemChangedTable { get; } = [];

    internal RelatedCurrencyManager(BindingManagerBase parentManager, string dataField)
        : this(parentManager, dataField, interceptedParentHandlerFactory: null)
    {
    }

    internal RelatedCurrencyManager(
        BindingManagerBase parentManager,
        string dataField,
        Func<RelatedCurrencyManager, EventHandler?>? interceptedParentHandlerFactory)
        : base(dataSource: null)
    {
        // The factory receives the fully-constructed child so a handler can close over it, mirroring
        // new ParentCurrentChangedHandler(cm) on .NET Framework. It is built before Bind() primes.
        _interceptedParentHandler = interceptedParentHandlerFactory?.Invoke(this);
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

        if (UsesInterceptedHandler(parentManager))
        {
            // Opt-in path: prime through the supplied handler and avoid constructor-time AddNew().
            _interceptedParentHandler!(parentManager, EventArgs.Empty);
        }
        else
        {
            // Default path: identical to current behaviour.
            ParentManager_CurrentItemChanged(parentManager, EventArgs.Empty);
        }
    }

    // The intercepted handler drives this manager off the parent's CurrentChanged and assumes a
    // CurrencyManager parent. A PropertyManager (single-object) parent always uses the stock path.
    // Wiring, unwiring and the constructor-time prime must all agree on this, or the prime can invoke
    // the handler against a parent it was not wired to.
    private bool UsesInterceptedHandler(BindingManagerBase? parent)
        => _interceptedParentHandler is not null && parent is CurrencyManager;

    private void UnwireParentManager(BindingManagerBase? bmb)
    {
        if (bmb is not null)
        {
            if (UsesInterceptedHandler(bmb))
            {
                ((CurrencyManager)bmb).CurrentChanged -= _interceptedParentHandler;
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
            if (UsesInterceptedHandler(bmb))
            {
                ((CurrencyManager)bmb).CurrentChanged += _interceptedParentHandler;
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

        // Pull the data from the controls into the backend before changing the entire list.
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
                SetDataSource(_fieldInfo.GetValue(currencyManager.Current));
                listposition = Count > 0 ? 0 : -1;
            }
            else
            {
                // Stock Everett compatibility: create a temporary row so the child manager can
                // discover metadata for an empty parent, then remove it without recursing.
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
            SetDataSource(_fieldInfo.GetValue(_parentManager.Current));
            listposition = Count > 0 ? 0 : -1;
        }

        if (oldlistposition != listposition)
        {
            OnPositionChanged(EventArgs.Empty);
        }

        OnCurrentChanged(EventArgs.Empty);
        OnCurrentItemChanged(EventArgs.Empty);
    }
}
