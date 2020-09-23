using System;
using System.Collections.Generic;
using System.Text;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Data;
using Avalonia.Media;
using Avalonia.Utilities;

namespace AvaDevBox.Controls
{
    /// <summary>
    /// Extended Slider with two hands and range indicator.
    /// Features include:
    /// Just one Range but two slider hands - to define a range.
    /// Range brush indicating the current value as a range on the same scale, but independent of slider positions.
    /// Show Values with String format beneath the ticks
    /// Show Tool tips with values
    /// Setting the number of ticks.
    /// Mouse wheel support.
    /// </summary>
    public class RangeSlider : RangeBase
    {
        public RangeSlider()
        {
            
        }

        /// <summary>
        /// Defines the <see cref="Value2"/> property.
        /// </summary>
        public static readonly DirectProperty<RangeSlider, double> Value2Property =
            AvaloniaProperty.RegisterDirect<RangeSlider, double>(
                nameof(Value2),
                o => o.Value2,
                (o, v) => o.Value2 = v,
                defaultBindingMode: BindingMode.TwoWay);

        private double _value2;

        /// <summary>
        /// Gets or sets a value indicating whether the slider thumb is currently dragged.
        /// </summary>
        /// <value>
        ///     <c>True</c> if the slider is dragged; otherwise, <c>false</c>.
        /// </value>
        public bool IsDragActive { get; set; }

        /// <summary>
        /// Gets or sets the second value of the two hands.
        /// </summary>
        public double Value2
        {
            get { return _value2; }

            set
            {
                if (!(!double.IsInfinity(value) || !double.IsNaN(value)))
                {
                    return;
                }

                if (IsInitialized)
                {
                    value = MathUtilities.Clamp(value, Minimum, Maximum);
                    SetAndRaise(Value2Property, ref _value2, value);
                }
                else
                {
                    SetAndRaise(Value2Property, ref _value2, value);
                }
            }
        }


        public IBrush SelectionBrush { get; set; }


        /// <summary>
        /// Gets or sets a value indicating whether the tick values are shown (when TickPlacement != None).
        /// </summary>
        /// <value><c>True</c> if tick values are shown; otherwise, <c>false</c>.</value>
        //public bool ShowTickValues
        //{
        //    get { return (bool)this.GetValue(ShowTickValuesProperty); }
        //    set { this.SetValue(ShowTickValuesProperty, value); }
        //}

        /// <summary>
        /// Gets or sets the string format for the tick values.
        /// </summary>
        /// <value>The tick value format.</value>
        //public string TickValueFormat
        //{
        //    get { return (string)this.GetValue(TickValueFormatProperty); }
        //    set { this.SetValue(TickValueFormatProperty, value); }
        //}
    }
}
