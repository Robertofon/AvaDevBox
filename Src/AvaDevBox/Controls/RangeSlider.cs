using System;
using System.Collections.Generic;
using System.Text;
using Avalonia;
using Avalonia.Collections;
using Avalonia.Controls;
using Avalonia.Controls.Mixins;
using Avalonia.Controls.Primitives;
using Avalonia.Data;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Layout;
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
    //[PseudoClasses(":vertical", ":horizontal", ":pressed")]
    public class RangeSlider : RangeBase
    {
        /// <summary>
        /// Defines the <see cref="Orientation"/> property.
        /// </summary>
        public static readonly StyledProperty<Orientation> OrientationProperty =
            ScrollBar.OrientationProperty.AddOwner<RangeSlider>();

        /// <summary>
        /// Defines the <see cref="IsSnapToTickEnabled"/> property.
        /// </summary>
        public static readonly StyledProperty<bool> IsSnapToTickEnabledProperty =
            Slider.IsSnapToTickEnabledProperty.AddOwner<RangeSlider>();

        /// <summary>
        /// Defines the <see cref="TickFrequency"/> property.
        /// </summary>
        public static readonly StyledProperty<double> TickFrequencyProperty =
            Slider.TickFrequencyProperty.AddOwner<RangeSlider>();

        /// <summary>
        /// Defines the <see cref="TickPlacement"/> property.
        /// </summary>
        public static readonly StyledProperty<TickPlacement> TickPlacementProperty =
            Slider.TickPlacementProperty.AddOwner<RangeSlider>();

        /// <summary>
        /// Defines the <see cref="TicksProperty"/> property.
        /// </summary>
        public static readonly StyledProperty<AvaloniaList<double>> TicksProperty =
            Slider.TicksProperty.AddOwner<RangeSlider>();


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

        public RangeSlider()
        {
            PressedMixin.Attach<RangeSlider>();
           // OrientationProperty.OverrideDefaultValue<RangeSlider>(Orientation.Horizontal);
            Thumb.DragStartedEvent.AddClassHandler<RangeSlider>((x, e) => x.OnThumbDragStarted(e), RoutingStrategies.Bubble);
            Thumb.DragCompletedEvent.AddClassHandler<RangeSlider>((x, e) => x.OnThumbDragCompleted(e),
                RoutingStrategies.Bubble);

        }

        private void OnThumbDragCompleted(VectorEventArgs e)
        {
        }

        private void OnThumbDragStarted(VectorEventArgs vectorEventArgs)
        {
            
        }


        ///// <summary>
        ///// Gets or sets a value indicating whether the slider thumb is currently dragged.
        ///// </summary>
        ///// <value>
        /////     <c>True</c> if the slider is dragged; otherwise, <c>false</c>.
        ///// </value>
        //public bool IsDragActive { get; set; }

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

        /// <summary>
        /// Defines the ticks to be drawn on the tick bar.
        /// </summary>
        public AvaloniaList<double> Ticks
        {
            get => GetValue(TicksProperty);
            set => SetValue(TicksProperty, value);
        }

        /// <summary>
        /// Gets or sets the orientation of a <see cref="RangeSlider"/>.
        /// </summary>
        public Orientation Orientation
        {
            get { return GetValue(OrientationProperty); }
            set { SetValue(OrientationProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value that indicates whether the <see cref="RangeSlider"/> automatically moves the <see cref="Thumb"/> to the closest tick mark.
        /// </summary>
        public bool IsSnapToTickEnabled
        {
            get { return GetValue(IsSnapToTickEnabledProperty); }
            set { SetValue(IsSnapToTickEnabledProperty, value); }
        }

        /// <summary>
        /// Gets or sets the interval between tick marks.
        /// </summary>
        public double TickFrequency
        {
            get { return GetValue(TickFrequencyProperty); }
            set { SetValue(TickFrequencyProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value that indicates where to draw 
        /// tick marks in relation to the track.
        /// </summary>
        public TickPlacement TickPlacement
        {
            get { return GetValue(TickPlacementProperty); }
            set { SetValue(TickPlacementProperty, value); }
        }

        private void UpdatePseudoClasses(Orientation o)
        {
            PseudoClasses.Set(":vertical", o == Orientation.Vertical);
            PseudoClasses.Set(":horizontal", o == Orientation.Horizontal);
        }

        /// <summary>
        /// Snap the input 'value' to the closest tick.
        /// </summary>
        /// <param name="value">Value that want to snap to closest Tick.</param>
        private double SnapToTick(double value)
        {
            if (IsSnapToTickEnabled)
            {
                double previous = Minimum;
                double next = Maximum;

                // This property is rarely set so let's try to avoid the GetValue
                var ticks = Ticks;

                // If ticks collection is available, use it.
                // Note that ticks may be unsorted.
                if ((ticks != null) && (ticks.Count > 0))
                {
                    for (int i = 0; i < ticks.Count; i++)
                    {
                        double tick = ticks[i];
                        if (MathUtilities.AreClose(tick, value))
                        {
                            return value;
                        }

                        if (MathUtilities.LessThan(tick, value) && MathUtilities.GreaterThan(tick, previous))
                        {
                            previous = tick;
                        }
                        else if (MathUtilities.GreaterThan(tick, value) && MathUtilities.LessThan(tick, next))
                        {
                            next = tick;
                        }
                    }
                }
                else if (MathUtilities.GreaterThan(TickFrequency, 0.0))
                {
                    previous = Minimum + (Math.Round(((value - Minimum) / TickFrequency)) * TickFrequency);
                    next = Math.Min(Maximum, previous + TickFrequency);
                }

                // Choose the closest value between previous and next. If tie, snap to 'next'.
                value = MathUtilities.GreaterThanOrClose(value, (previous + next) * 0.5) ? next : previous;
            }

            return value;
        }

    }
}
