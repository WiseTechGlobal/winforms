namespace System.Windows.Forms.Tests
{
    public class TabControlTests
    {
        [StaFact]
        public void TabControl_ClearTabsWhileSelected_DoesNotThrowNullReferenceException()
        {
            Exception? capturedException = null;
            string? failureMessage = null;
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

                Assert.Empty(control.TabPages);
                Assert.Null(control.SelectedTab);
            }
            finally
            {
                Application.ThreadException -= handler;
                if (capturedException is not null)
                {
                    failureMessage = $"Unhandled exception: {capturedException.GetType().Name}\n{capturedException.Message}\n{capturedException.StackTrace}";
                }
            }

            if (failureMessage is not null)
            {
                throw new Xunit.Sdk.XunitException(failureMessage);
            }
        }
    }
}
