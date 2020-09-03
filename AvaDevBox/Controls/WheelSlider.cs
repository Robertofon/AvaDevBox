using System;
using System.Collections.Generic;
using System.Text;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Layout;

namespace AvaDevBox.Controls
{
    /// <summary>
    /// A control almost like a slider. Continously scroll a wheel horinzontal or vertical.
    /// The visual shall be a wheel reaching into the screen.
    /// </summary>
    public class WheelSlider : RangeBase
    {
        /// <summary>
        /// Defines the <see cref="Orientation"/> property.
        /// </summary>
        public static readonly StyledProperty<Orientation> OrientationProperty =
            ScrollBar.OrientationProperty.AddOwner<WheelSlider>();

        /// <summary>
        /// Defines the <see cref="IsWraparoundEnabled"/> property.
        /// </summary>
        public static readonly StyledProperty<bool> IsWraparoundEnabledProperty =
            AvaloniaProperty.Register<WheelSlider, bool>(nameof(IsWraparoundEnabled), false, false);

        /// <summary>
        /// Defines the <see cref="SpeedUpFactor"/> property.
        /// </summary>
        public static readonly StyledProperty<double> SpeedUpFactorProperty =
            AvaloniaProperty.Register<WheelSlider, double>(nameof(SpeedUpFactor), 1d, false);

        private Control _wheelArea;
        private IDisposable _wheelPressedDispose;
        private IDisposable _wheelReleasedDispose;
        private IDisposable _wheelMovedDispose;
        private Point? _lastPoint;


        static WheelSlider()
        {
            AffectsRender<WheelSlider>(IsWraparoundEnabledProperty);
        }

        public WheelSlider()
        {
            UpdatePseudoClasses(Orientation);
        }

        /// <summary>
        /// Gets or sets the orientation of a <see cref="WheelSlider"/>.
        /// </summary>
        public Orientation Orientation
        {
            get { return GetValue(OrientationProperty); }
            set { SetValue(OrientationProperty, value); }
        }

        /// <summary>
        /// Gets or sets the speed up for a move. Just a factor for mouse movement.
        /// </summary>
        public double SpeedUpFactor
        {
            get { return GetValue(SpeedUpFactorProperty); }
            set { SetValue(SpeedUpFactorProperty, value); }
        }

        /// <summary>
        /// Gets or sets the value if the <see cref="WheelSlider"/> stops (<c>false</c>)
        /// at the end of the value range or whether it wraps around (<c>true</c>).
        /// </summary>
        public bool IsWraparoundEnabled
        {
            get { return GetValue(IsWraparoundEnabledProperty); }
            set { SetValue(IsWraparoundEnabledProperty, value); }
        }

        protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
        {
            base.OnApplyTemplate(e);
            _wheelPressedDispose?.Dispose();
            _wheelReleasedDispose?.Dispose();
            _wheelMovedDispose?.Dispose();
            //_increaseButtonSubscription?.Dispose();
            //_increaseButtonReleaseDispose?.Dispose();
            //_pointerMovedDispose?.Dispose();

            _wheelArea = e.NameScope.Find<Control>("PART_WheelArea");

            if (_wheelArea != null)
            {
                _wheelPressedDispose = _wheelArea.AddDisposableHandler(PointerPressedEvent, WheelPressed, RoutingStrategies.Tunnel);
                _wheelReleasedDispose = _wheelArea.AddDisposableHandler(PointerReleasedEvent, WheelReleased, RoutingStrategies.Tunnel);
                _wheelMovedDispose = _wheelArea.AddDisposableHandler(PointerMovedEvent, WheelMoved, RoutingStrategies.Tunnel);
            }
        }
        private void WheelReleased(object sender, PointerReleasedEventArgs e)
        {
            if (_lastPoint.HasValue)
            {
                var currPoint = e.GetPosition(this);
                Vector dist = currPoint - _lastPoint.Value;
                double move = this.Orientation == Orientation.Horizontal ? dist.X : dist.Y;
                double newVal = Value + move * 0.01 * SpeedUpFactor;
                if (IsWraparoundEnabled)
                {
                    newVal = (newVal - Minimum) % (Maximum - Minimum) + Minimum;
                }

                Value = newVal;
            }

            PseudoClasses.Remove(":pressed");
            PseudoClasses.Remove(":dragging");
            _lastPoint = null;
        }

        private void WheelMoved(object sender, PointerEventArgs e)
        {
            if (_lastPoint.HasValue)
            {
                PseudoClasses.Add(":dragging");
                var currPoint = e.GetPosition(this);
                Vector dist = currPoint - _lastPoint.Value;
                double move = this.Orientation == Orientation.Horizontal ? dist.X : dist.Y;
                double newVal = Value + move * 0.01 * SpeedUpFactor;
                if (IsWraparoundEnabled)
                {
                    newVal = (newVal - Minimum) % (Maximum - Minimum) + Minimum;
                }

                Value = newVal;
            }
        }


        private void WheelPressed(object sender, PointerPressedEventArgs e)
        {
            e.Handled = true;
            _lastPoint = e.GetPosition(this);

            PseudoClasses.Add(":pressed");
        }

        protected override void OnPropertyChanged<T>(AvaloniaPropertyChangedEventArgs<T> change)
        {
            base.OnPropertyChanged(change);

            if (change.Property == OrientationProperty)
            {
                UpdatePseudoClasses(change.NewValue.GetValueOrDefault<Orientation>());
            }
        }

        private void UpdatePseudoClasses(Orientation o)
        {
            PseudoClasses.Set(":vertical", o == Orientation.Vertical);
            PseudoClasses.Set(":horizontal", o == Orientation.Horizontal);
        }
    }
}
