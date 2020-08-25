using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.VisualTree;

namespace AvaDevBox.Controls
{
    /// <summary>
    /// Implementation of a Menu button. This is basically an ordinary button with all
    /// its capabilities plus a small button aside that expands the to be defined
    /// <see cref="DropDownMenu"/> which will then be displayed and its items
    /// can then issue alternate commands.
    /// </summary>
    public class MenuButton : Button
    {
        /// <summary>
        /// Defines the <see cref="DropDownMenu"/> property.
        /// </summary>
        public static readonly StyledProperty<ContextMenu> DropDownMenuProperty =
            AvaloniaProperty.Register<Control, ContextMenu>(nameof(DropDownMenu));

        private Control _popupBtn;


        /// <summary>
        /// Gets or sets a drop down menu to the menu button. The main feature.
        /// </summary>
        public ContextMenu DropDownMenu
        {
            get { return GetValue(DropDownMenuProperty); }
            set { SetValue(DropDownMenuProperty, value); }
        }


        protected override void OnTemplateApplied(TemplateAppliedEventArgs e)
        {
            base.OnTemplateApplied(e);
            _popupBtn = e.NameScope.Get<Control>("PART_PopupBtn");
        }

        /// <inheritdoc/>
        protected override void OnKeyUp(KeyEventArgs e)
        {
            if(_popupBtn.IsFocused)
            {
                // popupmenu
                e.Handled = true;
                return;
            }

            base.OnKeyUp(e);
        }

        /// <inheritdoc/>
        protected override void OnPointerPressed(PointerPressedEventArgs e)
        {
            IEnumerable<IVisual> visualsAt = this.GetVisualsAt(e.GetPosition(this));
            bool hitPopupBtn = visualsAt.Any(v => _popupBtn == v || _popupBtn.IsVisualAncestorOf(v));
            if (hitPopupBtn)
            {
                e.Handled = true;
                return;
            }

            base.OnPointerPressed(e);
        }

        /// <inheritdoc/>
        protected override void OnPointerReleased(PointerReleasedEventArgs e)
        {
            IEnumerable<IVisual> visualsAt = this.GetVisualsAt(e.GetPosition(this));
            bool hitPopupBtn = visualsAt.Any(v => _popupBtn == v || _popupBtn.IsVisualAncestorOf(v));
            if (hitPopupBtn)
            {
                e.Handled = true;
                return;
            }

            base.OnPointerReleased(e);
        }
    }
}
