// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.ComponentModel;
using System.Windows.Forms;

namespace System.Windows.Forms.Legacy.Tests;

public class BindingContextTests
{
    [Fact]
    public void BindingContext_UpdateBinding_RelatedManagerCurrentItemChangedDuringPullData_DoesNotReenter()
    {
        BindingContext context = [];
        BindingList<ReentrantParentDataSource> parentList = null!;
        ReentrantChildDataSource child = new(() => parentList.ResetItem(0));
        ReentrantParentDataSource parent = new(child);
        parentList = [parent];

        ReentrantBindableComponent component = new() { BindingContext = context };
        Binding binding = component.DataBindings.Add(
            nameof(ReentrantBindableComponent.Value),
            parentList,
            $"{nameof(ReentrantParentDataSource.Children)}.{nameof(ReentrantChildDataSource.Value)}");

        CurrencyManager relatedManager = Assert.IsAssignableFrom<CurrencyManager>(context[parentList, nameof(ReentrantParentDataSource.Children)]);
        Assert.Same(relatedManager, binding.BindingManagerBase);

        Exception? dataError = null;
        relatedManager.DataError += (sender, e) => dataError = e.Exception;

        component.Value = "updated";
        parentList.ResetItem(0);

        Assert.Null(dataError);
        Assert.Equal(1, child.ValueSetCount);
        Assert.Equal("updated", child.Value);
    }

    private class ReentrantParentDataSource
    {
        public ReentrantParentDataSource(ReentrantChildDataSource child)
        {
            Children = [child];
        }

        public BindingList<ReentrantChildDataSource> Children { get; }
    }

    private class ReentrantChildDataSource
    {
        private readonly Action _valueSetCallback;
        private string _value = "initial";

        public ReentrantChildDataSource(Action valueSetCallback)
        {
            _valueSetCallback = valueSetCallback;
        }

        public int ValueSetCount { get; private set; }

        public string Value
        {
            get => _value;
            set
            {
                ValueSetCount++;
                if (ValueSetCount > 1)
                {
                    throw new InvalidOperationException("Related manager pulled data re-entrantly.");
                }

                _value = value;
                _valueSetCallback();
            }
        }
    }

    private class ReentrantBindableComponent : BindableComponent
    {
        private string? _value;

        public string? Value
        {
            get => _value;
            set
            {
                if (_value == value)
                {
                    return;
                }

                _value = value;
                ValueChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public event EventHandler? ValueChanged;
    }
}