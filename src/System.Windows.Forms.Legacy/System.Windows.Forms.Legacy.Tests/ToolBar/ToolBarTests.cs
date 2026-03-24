// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.


// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Drawing;

namespace System.Windows.Forms.Legacy.Tests;

public class ToolBarTests
{
	[StaFact]
	public void ToolBar_DefaultConfiguration_MatchesLegacyBehavior()
	{
		using ToolBar toolBar = new();

		Assert.Equal(ToolBarAppearance.Normal, toolBar.Appearance);
		Assert.Equal(BorderStyle.None, toolBar.BorderStyle);
		Assert.Equal(DockStyle.Top, toolBar.Dock);
		Assert.Equal(ToolBarTextAlign.Underneath, toolBar.TextAlign);
		Assert.True(toolBar.AutoSize);
		Assert.True(toolBar.Divider);
		Assert.True(toolBar.DropDownArrows);
		Assert.True(toolBar.ShowToolTips);
		Assert.True(toolBar.Wrappable);
		Assert.False(toolBar.TabStop);
		Assert.Empty(toolBar.Buttons);
		Assert.Equal(new Size(39, 36), toolBar.ButtonSize);
	}

	[StaFact]
	public void ToolBar_ButtonSize_DefaultTracksTextAlignment()
	{
		using ToolBar toolBar = new();

		toolBar.TextAlign = ToolBarTextAlign.Right;

		Assert.Equal(new Size(23, 22), toolBar.ButtonSize);
	}

	[StaFact]
	public void ToolBar_Buttons_AddRange_PreservesOrderAndSetsParent()
	{
		using ToolBar toolBar = new();
		ToolBarButton openButton = new() { Name = "open", Text = "Open" };
		ToolBarButton saveButton = new() { Name = "save", Text = "Save" };
		ToolBarButton printButton = new() { Name = "print", Text = "Print" };

		toolBar.Buttons.AddRange([openButton, saveButton, printButton]);

		Assert.Equal(3, toolBar.Buttons.Count);
		Assert.Same(openButton, toolBar.Buttons[0]);
		Assert.Same(saveButton, toolBar.Buttons[1]);
		Assert.Same(printButton, toolBar.Buttons[2]);
		Assert.Same(toolBar, openButton.Parent);
		Assert.Same(toolBar, saveButton.Parent);
		Assert.Same(toolBar, printButton.Parent);
	}

	[StaFact]
	public void ToolBar_Buttons_Insert_UpdatesOrderAndKeyLookup()
	{
		using ToolBar toolBar = new();
		ToolBarButton firstButton = new() { Name = "first", Text = "First" };
		ToolBarButton lastButton = new() { Name = "last", Text = "Last" };
		ToolBarButton insertedButton = new() { Name = "inserted", Text = "Inserted" };

		toolBar.Buttons.AddRange([firstButton, lastButton]);

		toolBar.Buttons.Insert(1, insertedButton);

		Assert.Same(firstButton, toolBar.Buttons[0]);
		Assert.Same(insertedButton, toolBar.Buttons[1]);
		Assert.Same(lastButton, toolBar.Buttons[2]);
		Assert.Equal(1, toolBar.Buttons.IndexOfKey("inserted"));
		Assert.Same(insertedButton, toolBar.Buttons["INSERTED"]);
	}

	[StaFact]
	public void ToolBar_Buttons_SetIndexer_ReplacesButtonAndUpdatesParents()
	{
		using ToolBar toolBar = new();
		ToolBarButton originalButton = new() { Name = "original", Text = "Original" };
		ToolBarButton replacementButton = new() { Name = "replacement", Text = "Replacement" };

		toolBar.Buttons.Add(originalButton);

		toolBar.Buttons[0] = replacementButton;

		Assert.Null(originalButton.Parent);
		Assert.Same(replacementButton, toolBar.Buttons[0]);
		Assert.Same(toolBar, replacementButton.Parent);
	}

	[StaFact]
	public void ToolBar_Buttons_RemoveAt_DetachesRemovedButton()
	{
		using ToolBar toolBar = new();
		ToolBarButton keepButton = new() { Name = "keep", Text = "Keep" };
		ToolBarButton removeButton = new() { Name = "remove", Text = "Remove" };

		toolBar.Buttons.AddRange([keepButton, removeButton]);

		toolBar.Buttons.RemoveAt(1);

		Assert.Single(toolBar.Buttons);
		Assert.Same(keepButton, toolBar.Buttons[0]);
		Assert.Null(removeButton.Parent);
	}

	[StaFact]
	public void ToolBar_Buttons_Clear_DetachesAllButtons()
	{
		using ToolBar toolBar = new();
		ToolBarButton firstButton = new() { Text = "First" };
		ToolBarButton secondButton = new() { Text = "Second" };

		toolBar.Buttons.AddRange([firstButton, secondButton]);

		toolBar.Buttons.Clear();

		Assert.Empty(toolBar.Buttons);
		Assert.Null(firstButton.Parent);
		Assert.Null(secondButton.Parent);
	}

	[StaFact]
	public void ToolBar_Buttons_KeyLookup_IsCaseInsensitive_AndEmptyKeysAreIgnored()
	{
		using ToolBar toolBar = new();
		ToolBarButton saveButton = new() { Name = "Save", Text = "Save" };

		toolBar.Buttons.Add(saveButton);

		Assert.True(toolBar.Buttons.ContainsKey("save"));
		Assert.Equal(0, toolBar.Buttons.IndexOfKey("SAVE"));
		Assert.Same(saveButton, toolBar.Buttons["sAvE"]);
		Assert.False(toolBar.Buttons.ContainsKey(string.Empty));
		Assert.Equal(-1, toolBar.Buttons.IndexOfKey(string.Empty));
		Assert.Null(toolBar.Buttons[string.Empty]);
		Assert.Null(toolBar.Buttons[(string)null!]);
	}

	[StaFact]
	public void ToolBar_Buttons_RemoveByKey_MissingKey_IsNoOp()
	{
		using ToolBar toolBar = new();
		ToolBarButton button = new() { Name = "open", Text = "Open" };

		toolBar.Buttons.Add(button);

		toolBar.Buttons.RemoveByKey("missing");

		Assert.Single(toolBar.Buttons);
		Assert.Same(button, toolBar.Buttons[0]);
		Assert.Same(toolBar, button.Parent);
	}

	[StaFact]
	public void ToolBar_ButtonSize_NegativeDimension_ThrowsArgumentOutOfRangeException()
	{
		using ToolBar toolBar = new();

		Assert.Throws<ArgumentOutOfRangeException>(() => toolBar.ButtonSize = new Size(-1, 10));
		Assert.Throws<ArgumentOutOfRangeException>(() => toolBar.ButtonSize = new Size(10, -1));
	}
}

public class ToolBarButtonTests
{
	[StaFact]
	public void ToolBarButton_DefaultState_MatchesToolbarButtonBehavior()
	{
		using ToolBarButton button = new();

		Assert.Equal(string.Empty, button.Text);
		Assert.Equal(string.Empty, button.ToolTipText);
		Assert.Equal(ToolBarButtonStyle.PushButton, button.Style);
		Assert.True(button.Enabled);
		Assert.True(button.Visible);
		Assert.False(button.Pushed);
		Assert.False(button.PartialPush);
		Assert.Equal(Rectangle.Empty, button.Rectangle);
		Assert.Null(button.Parent);
		Assert.Null(button.DropDownMenu);
		Assert.Null(button.Tag);
	}

	[StaFact]
	public void ToolBarButton_Text_NormalizesNullAndEmptyToEmptyString()
	{
		using ToolBarButton button = new("Initial");

		button.Text = null;
		Assert.Equal(string.Empty, button.Text);

		button.Text = string.Empty;
		Assert.Equal(string.Empty, button.Text);
	}

	[StaFact]
	public void ToolBarButton_DropDownMenu_RequiresContextMenu()
	{
		using ToolBarButton button = new();
		using ContextMenu contextMenu = new();
		using MainMenu mainMenu = new();

		button.DropDownMenu = contextMenu;

		Assert.Same(contextMenu, button.DropDownMenu);
		Assert.Throws<ArgumentException>(() => button.DropDownMenu = mainMenu);
	}

	[StaFact]
	public void ToolBarButton_ImageIndex_LessThanMinusOne_ThrowsArgumentOutOfRangeException()
	{
		using ToolBarButton button = new();

		Assert.Throws<ArgumentOutOfRangeException>(() => button.ImageIndex = -2);
	}

	[StaFact]
	public void ToolBarButton_StateProperties_RoundTripWithoutParentHandle()
	{
		using ToolBarButton button = new();
		object tag = new();

		button.Enabled = false;
		button.Visible = false;
		button.Pushed = true;
		button.PartialPush = true;
		button.Style = ToolBarButtonStyle.ToggleButton;
		button.ToolTipText = "Click to toggle";
		button.Tag = tag;

		Assert.False(button.Enabled);
		Assert.False(button.Visible);
		Assert.True(button.Pushed);
		Assert.True(button.PartialPush);
		Assert.Equal(ToolBarButtonStyle.ToggleButton, button.Style);
		Assert.Equal("Click to toggle", button.ToolTipText);
		Assert.Same(tag, button.Tag);
	}
}

public class ToolBarButtonClickEventArgsTests
{
	[StaFact]
	public void ToolBarButtonClickEventArgs_Constructor_StoresButtonReference()
	{
		using ToolBarButton button = new() { Text = "Open" };

		ToolBarButtonClickEventArgs eventArgs = new(button);

		Assert.Same(button, eventArgs.Button);
	}

	[StaFact]
	public void ToolBarButtonClickEventArgs_Button_CanBeReassigned()
	{
		using ToolBarButton originalButton = new() { Text = "Original" };
		using ToolBarButton replacementButton = new() { Text = "Replacement" };
		ToolBarButtonClickEventArgs eventArgs = new(originalButton);

		eventArgs.Button = replacementButton;

		Assert.Same(replacementButton, eventArgs.Button);
	}
}
