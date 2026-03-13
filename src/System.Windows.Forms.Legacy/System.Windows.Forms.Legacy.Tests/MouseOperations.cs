namespace System.Windows.Forms.Tests
{
    using System.Runtime.InteropServices;

    public static class MouseOperations
    {
        [Flags]
        public enum MouseEventFlags
        {
            LeftDown = 0x00000002,
            LeftUp = 0x00000004,
            RightDown = 0x00000008,
            RightUp = 0x00000010,
        }

        [DllImport("user32.dll")]
        private static extern void mouse_event(int dwFlags, uint dx, uint dy, int dwData, int dwExtraInfo);

        public static void MouseEvent(MouseEventFlags value, uint dx, uint dy)
        {
            mouse_event((int)value, dx, dy, 0, 0);
        }
    }
}
