using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Presenters;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Templates;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml.Templates;
using Avalonia.Metadata;
using Avalonia.VisualTree;

namespace AvaDevBox.Controls
{
    /// <summary>
    /// Implementation of a Menu button. This is basically an ordinary button with all
    /// its capabilities plus a small button aside that expands the to be ........
    /// which will then be displayed and its items
    /// can then issue alternate commands.
    /// </summary>
    public class MenuButton : Button
    {
        /// <summary>
        /// Defines the <see cref="PopupContentTemplate"/> property.
        /// </summary>
        public static readonly StyledProperty<IDataTemplate> PopupContentTemplateProperty =
            AvaloniaProperty.Register<MenuButton, IDataTemplate>(nameof(PopupContentTemplate));

        /// <summary>
        /// Defines the <see cref="PopupContent"/> property. Depends on <see cref="PopupContentTemplate"/> property via internal
        /// <see cref="ContentPresenter"/>.
        /// </summary>
        public static readonly StyledProperty<object> PopupContentProperty =
            AvaloniaProperty.Register<MenuButton, object>(nameof(PopupContent));

        private Control _popupBtn;
        private Popup _popup;
        private Control _mainButton;


        /// <summary>
        /// Gets or sets the popup's content template. The main feature.
        /// </summary>
        public IDataTemplate PopupContentTemplate
        {
            get { return GetValue(PopupContentTemplateProperty); }
            set { SetValue(PopupContentTemplateProperty, value); }
        }

        /// <summary>
        /// Gets or sets the popup's content. The main feature.
        /// </summary>
        [DependsOn(nameof(PopupContentTemplate))]
        public object PopupContent
        {
            get { return GetValue(PopupContentProperty); }
            set { SetValue(PopupContentProperty, value); }
        }


        protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
        {
            base.OnApplyTemplate(e);
            _mainButton = e.NameScope.Get<Control>("PART_ContentPresenter");
            _popupBtn = e.NameScope.Get<Control>("PART_PopupBtn");
            _popup = e.NameScope.Get<Popup>("PART_Popup");

            // catch events before the rest of the control (_mainButton)
            _popupBtn.AddHandler(PointerPressedEvent, PopupBtnPointerPressed, RoutingStrategies.Tunnel);
            _popupBtn.AddHandler(PointerReleasedEvent, PopupBtnPointerReleased, RoutingStrategies.Tunnel);

            // Quit processing after popup content has done the job. Prevent _mainButton from doing so.
            _popup.AddHandler(PointerPressedEvent, PopupPointerPressed, RoutingStrategies.Bubble);
            _popup.AddHandler(PointerReleasedEvent, PopupPointerReleased, RoutingStrategies.Bubble);

            _popupBtn.AddHandler(KeyDownEvent, PopupBtnKeyDown, RoutingStrategies.Tunnel);
            _popupBtn.AddHandler(KeyUpEvent, PopupBtnKeyUp, RoutingStrategies.Tunnel);
        }

        private void PopupBtnKeyUp(object sender, KeyEventArgs e)
        {
            e.Handled = true;
        }

        private void PopupBtnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                this._popup.IsOpen = !_popup.IsOpen;
            }

            e.Handled = true;
        }

        private void PopupBtnPointerPressed(object sender, PointerPressedEventArgs e)
        {
            if (this.ClickMode == ClickMode.Press)
            {
                _popup.Open();
            }
            e.Handled = true;
        }

        private void PopupBtnPointerReleased(object sender, PointerReleasedEventArgs e)
        {
            if (this.ClickMode == ClickMode.Release)
            {
                _popup.Open();
            }
            e.Handled = true;
        }

        private void PopupPointerPressed(object sender, PointerPressedEventArgs e)
        {
            e.Handled = true;
        }

        private void PopupPointerReleased(object sender, PointerReleasedEventArgs e)
        {
            e.Handled = true;
        }

        /// <summary>
        /// Invokes the <see cref="PopUpButtonClick"/> event.
        /// </summary>
        protected virtual void OnPopupBtnClick()
        {
            _popup.IsOpen = true;
            //var e = new RoutedEventArgs(ClickEvent);
            //RaiseEvent(e);

            //if (!e.Handled && Command?.CanExecute(CommandParameter) == true)
            //{
            //    Command.Execute(CommandParameter);
            //    e.Handled = true;
            //}
        }

        /// <inheritdoc/>
        protected override void OnKeyUp(KeyEventArgs e)
        {
            //if(_popupBtn.IsFocused)
            //{
            //    // popupmenu
            //    OnPopupBtnClick();
            //    e.Handled = true;
            //    return;
            //}
            //else if (_mainButton.IsFocused)
            //{

            //}

            base.OnKeyUp(e);
        }

        /// <inheritdoc/>
        protected override void OnPointerPressed(PointerPressedEventArgs e)
        {
            //IEnumerable<IVisual> visualsAt = this.GetVisualsAt(e.GetPosition(this));
            //bool hitPopupBtn = visualsAt.Any(v => _popupBtn == v || _popupBtn.IsVisualAncestorOf(v));
            //if (hitPopupBtn)
            //{
            //    if (this.ClickMode == ClickMode.Press)
            //    {
            //        _popup.Open();
            //    }
            //    e.Handled = true;
            //    return;
            //}

            base.OnPointerPressed(e);
        }

        /// <inheritdoc/>
        protected override void OnPointerReleased(PointerReleasedEventArgs e)
        {
            //IEnumerable<IVisual> visualsAt = this.GetVisualsAt(e.GetPosition(this));
            //bool hitPopupBtn = visualsAt.Any(v => _popupBtn == v || _popupBtn.IsVisualAncestorOf(v));
            //if (hitPopupBtn)
            //{
            //    if (ClickMode == ClickMode.Release)
            //    {
            //        _popup.Open();
            //    }
            //    e.Handled = true;
            //    return;
            //}

            base.OnPointerReleased(e);
        }
    }
}
