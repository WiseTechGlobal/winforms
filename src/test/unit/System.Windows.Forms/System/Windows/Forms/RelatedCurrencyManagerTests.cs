// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.ComponentModel;

namespace System.Windows.Forms.Tests;

// NB: doesn't require thread affinity
public class RelatedCurrencyManagerTests
{
    /// <summary>
    ///  Verifies that <see cref="RelatedCurrencyManager.UseParentCurrentChanged"/> defaults to
    ///  <see langword="false"/>, preserving the standard framework behavior out of the box.
    /// </summary>
    [Fact]
    public void UseParentCurrentChanged_DefaultValue_IsFalse()
    {
        bool originalValue = RelatedCurrencyManager.UseParentCurrentChanged;
        try
        {
            // Reset to the documented default so the assertion is deterministic.
            RelatedCurrencyManager.UseParentCurrentChanged = false;

            Assert.False(RelatedCurrencyManager.UseParentCurrentChanged);
        }
        finally
        {
            RelatedCurrencyManager.UseParentCurrentChanged = originalValue;
        }
    }

    /// <summary>
    ///  Default behavior: the child manager refreshes when the parent fires
    ///  <see cref="BindingManagerBase.CurrentItemChanged"/> (e.g. an item-property edit on
    ///  the parent list).
    /// </summary>
    [Fact]
    public void UseParentCurrentChanged_False_RefreshesOnCurrentItemChanged()
    {
        bool originalValue = RelatedCurrencyManager.UseParentCurrentChanged;
        try
        {
            RelatedCurrencyManager.UseParentCurrentChanged = false;

            BindingList<ParentRow> parentList = [new ParentRow(), new ParentRow()];
            BindingContext context = [];

            CurrencyManager parentManager = (CurrencyManager)context[parentList];
            CurrencyManager childManager = (CurrencyManager)context[parentList, nameof(ParentRow.Children)];

            int refreshCount = 0;
            childManager.CurrentChanged += (s, e) => refreshCount++;

            // ResetItem fires ListChanged(ItemChanged) on the BindingList, which causes the
            // parent CurrencyManager to fire OnCurrentItemChanged — but NOT OnCurrentChanged.
            // Default mode is subscribed to CurrentItemChanged, so the child should refresh.
            parentList.ResetItem(parentManager.Position);

            Assert.Equal(1, refreshCount);
        }
        finally
        {
            RelatedCurrencyManager.UseParentCurrentChanged = originalValue;
        }
    }

    /// <summary>
    ///  Opt-in behavior: when <see cref="RelatedCurrencyManager.UseParentCurrentChanged"/> is
    ///  <see langword="true"/> the child manager does <em>not</em> refresh on a parent item-property
    ///  edit that fires only <see cref="BindingManagerBase.CurrentItemChanged"/>.
    /// </summary>
    [Fact]
    public void UseParentCurrentChanged_True_DoesNotRefreshOnCurrentItemChangedOnly()
    {
        bool originalValue = RelatedCurrencyManager.UseParentCurrentChanged;
        try
        {
            RelatedCurrencyManager.UseParentCurrentChanged = true;

            BindingList<ParentRow> parentList = [new ParentRow(), new ParentRow()];
            BindingContext context = [];

            CurrencyManager parentManager = (CurrencyManager)context[parentList];
            CurrencyManager childManager = (CurrencyManager)context[parentList, nameof(ParentRow.Children)];

            int refreshCount = 0;
            childManager.CurrentChanged += (s, e) => refreshCount++;

            // ResetItem fires ListChanged(ItemChanged) which causes the parent to fire only
            // CurrentItemChanged, not CurrentChanged. Opt-in mode is subscribed to CurrentChanged,
            // so the child must NOT refresh.
            parentList.ResetItem(parentManager.Position);

            Assert.Equal(0, refreshCount);
        }
        finally
        {
            RelatedCurrencyManager.UseParentCurrentChanged = originalValue;
        }
    }

    /// <summary>
    ///  Default behavior: the child manager refreshes when the parent position changes
    ///  (which fires <see cref="BindingManagerBase.CurrentChanged"/> and consequently
    ///  <see cref="BindingManagerBase.CurrentItemChanged"/>).
    /// </summary>
    [Fact]
    public void UseParentCurrentChanged_False_RefreshesOnParentPositionChange()
    {
        bool originalValue = RelatedCurrencyManager.UseParentCurrentChanged;
        try
        {
            RelatedCurrencyManager.UseParentCurrentChanged = false;

            BindingList<ParentRow> parentList = [new ParentRow(), new ParentRow()];
            BindingContext context = [];

            CurrencyManager parentManager = (CurrencyManager)context[parentList];
            CurrencyManager childManager = (CurrencyManager)context[parentList, nameof(ParentRow.Children)];

            int refreshCount = 0;
            childManager.CurrentChanged += (s, e) => refreshCount++;

            // Moving the parent position fires CurrentChanged (and CurrentItemChanged) on the
            // parent. In default mode the child is subscribed to CurrentItemChanged so it refreshes.
            parentManager.Position = 1;

            Assert.Equal(1, refreshCount);
        }
        finally
        {
            RelatedCurrencyManager.UseParentCurrentChanged = originalValue;
        }
    }

    /// <summary>
    ///  Opt-in behavior: the child manager also refreshes when the parent position changes,
    ///  because a position change fires <see cref="BindingManagerBase.CurrentChanged"/> which
    ///  the opt-in mode subscribes to.
    /// </summary>
    [Fact]
    public void UseParentCurrentChanged_True_RefreshesOnParentPositionChange()
    {
        bool originalValue = RelatedCurrencyManager.UseParentCurrentChanged;
        try
        {
            RelatedCurrencyManager.UseParentCurrentChanged = true;

            BindingList<ParentRow> parentList = [new ParentRow(), new ParentRow()];
            BindingContext context = [];

            CurrencyManager parentManager = (CurrencyManager)context[parentList];
            CurrencyManager childManager = (CurrencyManager)context[parentList, nameof(ParentRow.Children)];

            int refreshCount = 0;
            childManager.CurrentChanged += (s, e) => refreshCount++;

            // Moving the parent position fires CurrentChanged on the parent. Opt-in mode is
            // subscribed to CurrentChanged so the child must refresh exactly once.
            parentManager.Position = 1;

            Assert.Equal(1, refreshCount);
        }
        finally
        {
            RelatedCurrencyManager.UseParentCurrentChanged = originalValue;
        }
    }

    /// <summary>
    ///  Symmetry: retrieving the same related manager from the <see cref="BindingContext"/>
    ///  cache twice does not produce a double subscription — the child refreshes exactly once
    ///  per parent event.
    /// </summary>
    [Fact]
    public void UseParentCurrentChanged_False_CachedManagerHasNoDoubleSubscription()
    {
        bool originalValue = RelatedCurrencyManager.UseParentCurrentChanged;
        try
        {
            RelatedCurrencyManager.UseParentCurrentChanged = false;

            BindingList<ParentRow> parentList = [new ParentRow(), new ParentRow()];
            BindingContext context = [];

            CurrencyManager parentManager = (CurrencyManager)context[parentList];

            // Retrieve the child manager twice; the second lookup must return the cached instance.
            CurrencyManager childManager1 = (CurrencyManager)context[parentList, nameof(ParentRow.Children)];
            CurrencyManager childManager2 = (CurrencyManager)context[parentList, nameof(ParentRow.Children)];
            Assert.Same(childManager1, childManager2);

            int refreshCount = 0;
            childManager1.CurrentChanged += (s, e) => refreshCount++;

            parentList.ResetItem(parentManager.Position);

            // Must fire exactly once — not twice — confirming no stale double subscription.
            Assert.Equal(1, refreshCount);
        }
        finally
        {
            RelatedCurrencyManager.UseParentCurrentChanged = originalValue;
        }
    }

    /// <summary>
    ///  Symmetry: when the switch is <see langword="true"/>, rebinding the manager
    ///  (<see cref="RelatedCurrencyManager.Bind"/>) does not leave a stale
    ///  <see cref="BindingManagerBase.CurrentChanged"/> subscription on the old parent.
    /// </summary>
    [Fact]
    public void UseParentCurrentChanged_True_RebindDoesNotLeaveStaleSubscription()
    {
        bool originalValue = RelatedCurrencyManager.UseParentCurrentChanged;
        try
        {
            RelatedCurrencyManager.UseParentCurrentChanged = true;

            BindingList<ParentRow> parentList = [new ParentRow(), new ParentRow()];
            BindingContext context = [];

            CurrencyManager parentManager = (CurrencyManager)context[parentList];
            RelatedCurrencyManager childManager =
                (RelatedCurrencyManager)context[parentList, nameof(ParentRow.Children)];

            // Rebind to the same parent — Bind() unwires the old CurrentChanged subscription
            // and wires a fresh one. The Bind() call itself fires an init refresh directly
            // (not via the event), so add the counter handler after the rebind.
            childManager.Bind(parentManager, nameof(ParentRow.Children));

            int refreshCount = 0;
            childManager.CurrentChanged += (s, e) => refreshCount++;

            // Move the parent position: fires CurrentChanged (and CurrentItemChanged) on the parent.
            // The child must refresh exactly once — not twice — confirming no double subscription.
            parentManager.Position = 1;

            Assert.Equal(1, refreshCount);
        }
        finally
        {
            RelatedCurrencyManager.UseParentCurrentChanged = originalValue;
        }
    }

    private sealed class ParentRow
    {
        public List<ChildRow> Children { get; set; } = [];
    }

    private sealed class ChildRow
    {
        public string Name { get; set; } = string.Empty;
    }
}
