using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Presenters;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Templates;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Metadata;
using System;

namespace AvaDevBox.Controls
{
    /// <summary>
    /// Implementation of a Menu button. This is basically an ordinary button with all
    /// its capabilities plus a side button (like a drop down) and
    /// content/content template which displays in a popup if the side button is pressed.
    ///
    /// TOD if redesign
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
            _popup.AddHandler(ClickEvent, (sender, ea) => ea.Handled = true, RoutingStrategies.Bubble);
            _popup.AddHandler(PointerPressedEvent, (sender, e1) => e1.Handled = true, RoutingStrategies.Bubble);
            _popup.AddHandler(PointerReleasedEvent, (sender, e1) => e1.Handled = true, RoutingStrategies.Bubble);

            // House keeping for Pseudoclasses
            _popup.Opened += PopupOnOpened;
            _popup.Closed += PopupOnClosed;

            // Grab key events beforehand
            _popupBtn.AddHandler(KeyDownEvent, PopupBtnKeyDown, RoutingStrategies.Tunnel);
            _popupBtn.AddHandler(KeyUpEvent, PopupBtnKeyUp, RoutingStrategies.Tunnel);
        }

        private void PopupOnOpened(object sender, EventArgs e)
        {
            UpdatePseudoClasses(true);
        }

        private void PopupOnClosed(object sender, EventArgs e)
        {
            UpdatePseudoClasses(false);
        }

        private void PopupBtnKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                _popup.IsOpen = !_popup.IsOpen;
            }

            e.Handled = true;
        }

        private void PopupBtnKeyDown(object sender, KeyEventArgs e)
        {
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

        private void UpdatePseudoClasses(bool isPressed)
        {
            PseudoClasses.Set(":popupbtnpressed", isPressed);
        }

    }
}
