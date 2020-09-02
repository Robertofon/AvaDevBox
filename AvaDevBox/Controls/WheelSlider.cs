using System;
using System.Collections.Generic;
using System.Text;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
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
        /// Gets or sets the value if the <see cref="WheelSlider"/> stops (<c>false</c>)
        /// at the end of the value range or whether it wraps around (<c>true</c>).
        /// </summary>
        public bool IsWraparoundEnabled
        {
            get { return GetValue(IsWraparoundEnabledProperty); }
            set { SetValue(IsWraparoundEnabledProperty, value); }
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
