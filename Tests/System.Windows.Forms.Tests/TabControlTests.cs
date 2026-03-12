namespace System.Windows.Forms.Tests
{
    [TestFixture]
    [SingleThreaded]
    public class TabControlTests
    {
        [Test]
        public void TabControl_ClearTabsWhileSelected_DoesNotThrowNullReferenceException()
        {
            Exception? capturedException = null;
            ThreadExceptionEventHandler handler = (_, e) => capturedException = e.Exception;

            Application.ThreadException += handler;
            try
            {
                using Form form = new();
                using TabControl control = new();

                control.TabPages.Add(new TabPage("Tab 1"));
                control.TabPages.Add(new TabPage("Tab 2"));
                control.TabPages.Add(new TabPage("Tab 3"));

                form.Controls.Add(control);
                form.Show();
                _ = control.AccessibilityObject;

                control.SelectedIndex = 1;
                Application.DoEvents();

                control.TabPages.Clear();

                Application.DoEvents();
                System.Threading.Thread.Sleep(10);

                Assert.That(control.TabPages.Count, Is.EqualTo(0));
                Assert.That(control.SelectedTab, Is.Null);
            }
            finally
            {
                Application.ThreadException -= handler;
                if (capturedException is not null)
                {
                    Assert.Fail($"Unhandled exception: {capturedException.GetType().Name}\n{capturedException.Message}\n{capturedException.StackTrace}");
                }
            }
        }
    }
}
