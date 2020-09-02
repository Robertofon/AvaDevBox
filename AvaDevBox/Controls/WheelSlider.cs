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

        public WheelSlider()
        {
            UpdatePseudoClasses(Orientation);
        }

        /// <summary>
        /// Gets or sets the orientation of a <see cref="Slider"/>.
        /// </summary>
        public Orientation Orientation
        {
            get { return GetValue(OrientationProperty); }
            set { SetValue(OrientationProperty, value); }
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
