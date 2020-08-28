using System;
using System.Collections.Generic;
using System.Text;
using Avalonia.Controls.Primitives;
using Avalonia.Media;

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
        /// <summary>
        /// Gets or sets a value indicating whether the slider thumb is currently dragged.
        /// </summary>
        /// <value>
        ///     <c>True</c> if the slider is dragged; otherwise, <c>false</c>.
        /// </value>
        public bool IsDragActive { get; set; }

        public double SecondHandValue { get; set; }

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
