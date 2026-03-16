// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#pragma warning disable IDE1006, CS8625, CS8618, CS8601, RS0036, RS0016

using System.ComponentModel;
using UnsafeNativeMethods = System.Windows.Forms.LegacyMenuUnsafeNativeMethods;

namespace System.Windows.Forms
{
    /// <summary>
    ///  Represents a menu structure for a form.
    /// </summary>
    [ToolboxItemFilter("System.Windows.Forms.MainMenu")]
    public class MainMenu : Menu
    {
        internal Form form;
        internal Form ownerForm;  // this is the form that created this menu, and is the only form allowed to dispose it.
        private RightToLeft rightToLeft = RightToLeft.Inherit;
        private EventHandler onCollapse;

        /// <summary>
        ///  Creates a new MainMenu control.
        /// </summary>
        public MainMenu()
            : base(null)
        {
        }

        /// <summary>
        ///  Initializes a new instance of the <see cref='MainMenu'/> class with the specified container.
        /// </summary>
        public MainMenu(IContainer container) : this()
        {
            ArgumentNullException.ThrowIfNull(container);

            container.Add(this);
        }

        /// <summary>
        ///  Creates a new MainMenu control with the given items to start
        ///  with.
        /// </summary>
        public MainMenu(MenuItem[] items)
            : base(items)
        {
        }

        [SRDescription(nameof(SR.MainMenuCollapseDescr))]
        public event EventHandler Collapse
        {
            add => onCollapse += value;
            remove => onCollapse -= value;
        }

        /// <summary>
        ///  This is used for international applications where the language
        ///  is written from RightToLeft. When this property is true,
        ///  text alignment and reading order will be from right to left.
        /// </summary>
        // Add an AmbientValue attribute so that the Reset context menu becomes available in the Property Grid.
        [
        Localizable(true),
        SRDescription(nameof(SR.MenuRightToLeftDescr)),
        AmbientValue(RightToLeft.Inherit)
        ]
        public virtual RightToLeft RightToLeft
        {
            get
            {
                if (rightToLeft == RightToLeft.Inherit)
                {
                    if (form is not null)
                    {
                        return form.RightToLeft;
                    }
                    else
                    {
                        return RightToLeft.Inherit;
                    }
                }
                else
                {
                    return rightToLeft;
                }
            }
            set
            {
                // valid values are 0x0 to 0x2
                if (value is < RightToLeft.No or > RightToLeft.Inherit)
                {
                    throw new InvalidEnumArgumentException(nameof(RightToLeft), (int)value, typeof(RightToLeft));
                }

                if (rightToLeft != value)
                {
                    rightToLeft = value;
                    UpdateRtl((value == RightToLeft.Yes));
                }
            }
        }

        internal override bool RenderIsRightToLeft
        {
            get
            {
                return (RightToLeft == RightToLeft.Yes && (form is null || !form.IsMirrored));
            }
        }

        /// <summary>
        ///  Creates a new MainMenu object which is a dupliate of this one.
        /// </summary>
        public virtual MainMenu CloneMenu()
        {
            MainMenu newMenu = new MainMenu();
            newMenu.CloneMenu(this);
            return newMenu;
        }

        protected override IntPtr CreateMenuHandle()
        {
            return UnsafeNativeMethods.CreateMenu();
        }

        /// <summary>
        ///  Clears out this MainMenu object and discards all of it's resources.
        ///  If the menu is parented in a form, it is disconnected from that as
        ///  well.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (form is not null && (ownerForm is null || form == ownerForm))
                {
                    form.Menu = null;
                }
            }

            base.Dispose(disposing);
        }

        /// <summary>
        ///  Indicates which form in which we are currently residing [if any]
        /// </summary>
        public Form GetForm()
        {
            return form;
        }

        internal Form GetFormUnsafe()
        {
            return form;
        }

        internal override void ItemsChanged(int change)
        {
            base.ItemsChanged(change);
            form?.MenuChanged(change, this);
        }

        internal virtual void ItemsChanged(int change, Menu menu)
        {
            form?.MenuChanged(change, menu);
        }

        /// <summary>
        ///  Fires the collapse event
        /// </summary>
        protected internal virtual void OnCollapse(EventArgs e)
        {
            onCollapse?.Invoke(this, e);
        }

        /// <summary>
        ///  Returns true if the RightToLeft should be persisted in code gen.
        /// </summary>
        internal virtual bool ShouldSerializeRightToLeft()
        {
            if (RightToLeft == RightToLeft.Inherit)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        ///  Returns a string representation for this control.
        /// </summary>
        public override string ToString()
        {
            // Removing GetForm information
            return base.ToString();
        }
    }
}
