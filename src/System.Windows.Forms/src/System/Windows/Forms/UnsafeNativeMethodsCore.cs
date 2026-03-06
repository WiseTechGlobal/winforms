// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace WTG.System.Windows.Forms.System.Windows.Forms
{
    internal class UnsafeNativeMethodsCore
    {

        /// <summary>
        ///  This class provides static methods to create, activate and deactivate the theming scope.
        /// </summary>
        internal class ThemingScope
        {
            private static ACTCTX enableThemingActivationContext;
            private static IntPtr hActCtx;
            private static bool contextCreationSucceeded;

            /// <summary>
            ///  We now use explicitactivate everywhere and use this method to determine if we
            ///  really need to activate the activationcontext.  This should be pretty fast.
            /// </summary>
            private static bool IsContextActive()
            {
                IntPtr current = IntPtr.Zero;

                if (contextCreationSucceeded && GetCurrentActCtx(out current))
                {
                    return current == hActCtx;
                }

                return false;
            }

            /// <summary>
            ///  Activate() does nothing if a theming context is already active on the current thread, which is good
            ///  for perf reasons. However, in some cases, like in the Timer callback, we need to put another context
            ///  on the stack even if one is already present. In such cases, this method helps - you get to manage
            ///  the cookie yourself though.
            /// </summary>
            public static IntPtr Activate()
            {
                IntPtr userCookie = IntPtr.Zero;

                if (Application.UseVisualStyles && contextCreationSucceeded && OSFeature.Feature.IsPresent(OSFeature.Themes))
                {
                    if (!IsContextActive())
                    {
                        if (!ActivateActCtx(hActCtx, out userCookie))
                        {
                            // Be sure cookie always zero if activation failed
                            userCookie = IntPtr.Zero;
                        }
                    }
                }

                return userCookie;
            }

            /// <summary>
            ///  Use this to deactivate a context activated by calling ExplicitActivate.
            /// </summary>
            public static IntPtr Deactivate(IntPtr userCookie)
            {
                if (userCookie != IntPtr.Zero && OSFeature.Feature.IsPresent(OSFeature.Themes))
                {
                    if (DeactivateActCtx(0, userCookie))
                    {
                        // deactivation succeeded...
                        userCookie = IntPtr.Zero;
                    }
                }

                return userCookie;
            }

            public static bool CreateActivationContext(string dllPath, int nativeResourceManifestID)
            {
                lock (typeof(ThemingScope))
                {
                    if (!contextCreationSucceeded && OSFeature.Feature.IsPresent(OSFeature.Themes))
                    {

                        enableThemingActivationContext = new ACTCTX
                        {
                            cbSize = Marshal.SizeOf<ACTCTX>(),
                            lpSource = dllPath,
                            lpResourceName = (IntPtr)nativeResourceManifestID,
                            dwFlags = ACTCTX_FLAG_RESOURCE_NAME_VALID
                        };

                        hActCtx = CreateActCtx(ref enableThemingActivationContext);
                        contextCreationSucceeded = (hActCtx != new IntPtr(-1));
                    }

                    return contextCreationSucceeded;
                }
            }

            // All the pinvoke goo...
            [DllImport(ExternDll.Kernel32)]

            private extern static IntPtr CreateActCtx(ref ACTCTX actctx);
            [DllImport(ExternDll.Kernel32)]

            private extern static bool ActivateActCtx(IntPtr hActCtx, out IntPtr lpCookie);
            [DllImport(ExternDll.Kernel32)]

            private extern static bool DeactivateActCtx(int dwFlags, IntPtr lpCookie);
            [DllImport(ExternDll.Kernel32)]

            private extern static bool GetCurrentActCtx(out IntPtr handle);

            private const int ACTCTX_FLAG_RESOURCE_NAME_VALID = 0x008;

            private struct ACTCTX
            {
                public int cbSize;
                public uint dwFlags;
                public string lpSource;
                public ushort wProcessorArchitecture;
                public ushort wLangId;
                public string lpAssemblyDirectory;
                public IntPtr lpResourceName;
                public string lpApplicationName;
            }
        }
    }
}
