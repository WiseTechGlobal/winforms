namespace System.Windows.Forms.Tests
{
    public class MenuItemTests
    {
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
    [StaFact]
        public void MenuItem_OnDrawItem_Invoke_Success()
        {
            var menuItem = new SubMenuItem();

            // No handler.
            menuItem.OnDrawItem(null);

            // Handler.
            int callCount = 0;
            DrawItemEventHandler handler = (sender, e) =>
            {
                Assert.Same(menuItem, sender);
                callCount++;
            };

            menuItem.DrawItem += handler;
            menuItem.OnDrawItem(null);
            Assert.Equal(1, callCount);

            // Should not call if the handler is removed.
            menuItem.DrawItem -= handler;
            menuItem.OnDrawItem(null);
            Assert.Equal(1, callCount);
        }

        [StaFact]
        public void MenuItem_OnDrawItem_Disposed_ThrowsObjectDisposedException()
        {
            var menuItem = new SubMenuItem();
            menuItem.Dispose();
            Assert.Throws<ObjectDisposedException>(() => menuItem.OnDrawItem(null));
        }

        [StaFact]
        public void MenuItem_DrawItem_Disposed_ThrowsObjectDisposedException()
        {
            var menuItem = new SubMenuItem();
            menuItem.Dispose();
            DrawItemEventHandler handler = (sender, e) => { };
            Assert.Throws<ObjectDisposedException>(() => menuItem.DrawItem += handler);
            Assert.Throws<ObjectDisposedException>(() => menuItem.DrawItem -= handler);
        }

        [StaFact]
        public void MenuItem_OnMeasureItem_Invoke_Success()
        {
            var menuItem = new SubMenuItem();

            // No handler.
            menuItem.OnMeasureItem(null);

            // Handler.
            int callCount = 0;
            MeasureItemEventHandler handler = (sender, e) =>
            {
                Assert.Same(menuItem, sender);
                callCount++;
            };

            menuItem.MeasureItem += handler;
            menuItem.OnMeasureItem(null);
            Assert.Equal(1, callCount);

            // Should not call if the handler is removed.
            menuItem.MeasureItem -= handler;
            menuItem.OnMeasureItem(null);
            Assert.Equal(1, callCount);
        }

        [StaFact]
        public void MenuItem_OnMeasureItem_Disposed_ThrowsObjectDisposedException()
        {
            var menuItem = new SubMenuItem();
            menuItem.Dispose();
            Assert.Throws<ObjectDisposedException>(() => menuItem.OnMeasureItem(null));
        }

        [StaFact]
        public void MenuItem_MeasureItem_Disposed_ThrowsObjectDisposedException()
        {
            var menuItem = new SubMenuItem();
            menuItem.Dispose();
            MeasureItemEventHandler handler = (sender, e) => { };
            Assert.Throws<ObjectDisposedException>(() => menuItem.MeasureItem += handler);
            Assert.Throws<ObjectDisposedException>(() => menuItem.MeasureItem -= handler);
        }
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.

        public class SubMenuItem : MenuItem
        {
            public SubMenuItem()
            {
            }

            public SubMenuItem(string text) : base(text)
            {
            }

            public SubMenuItem(string text, EventHandler onClick) : base(text, onClick)
            {
            }

            public SubMenuItem(string text, MenuItem[] items) : base(text, items)
            {
            }

            public SubMenuItem(string text, EventHandler onClick, Shortcut shortcut) : base(text, onClick, shortcut)
            {
            }

            public SubMenuItem(MenuMerge mergeType, int mergeOrder, Shortcut shortcut, string text, EventHandler onClick, EventHandler onPopup, EventHandler onSelect, MenuItem[] items) : base(mergeType, mergeOrder, shortcut, text, onClick, onPopup, onSelect, items)
            {
            }

            public new int MenuID => base.MenuID;

            public new void OnClick(EventArgs e) => base.OnClick(e);

            public new void OnDrawItem(DrawItemEventArgs e) => base.OnDrawItem(e);

            public new void OnInitMenuPopup(EventArgs e) => base.OnInitMenuPopup(e);

            public new void OnMeasureItem(MeasureItemEventArgs e) => base.OnMeasureItem(e);

            public new void OnPopup(EventArgs e) => base.OnPopup(e);

            public new void OnSelect(EventArgs e) => base.OnSelect(e);

            public new void CloneMenu(Menu menuSrc) => base.CloneMenu(menuSrc);
        }
    }
}
