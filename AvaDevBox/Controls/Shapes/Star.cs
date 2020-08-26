using System;
using System.Collections.Generic;
using System.Text;
using Avalonia;
using Avalonia.Controls.Shapes;
using Avalonia.Media;

namespace AvaDevBox.Controls.Shapes
{
    /// <summary>
    /// Provides a configurable star as a geometry shape.
    /// </summary>
    public class Star : Shape
    {
        public static readonly StyledProperty<int> NumberOfSpikesProperty =
            AvaloniaProperty.Register<Star, int>(nameof(NumberOfSpikes), 6, validate: ValidateNumberOfSpikes);

        public static readonly StyledProperty<double> InnerRadiusRatioProperty =
            AvaloniaProperty.Register<Star, double>(nameof(InnerRadiusRatio), 0.4, validate: ValidateInnerRadius);

        private static bool ValidateNumberOfSpikes(int i)
        {
            return i <= 2047 && i >= 1;
        }

        private static bool ValidateInnerRadius(double d)
        {
            return 0<=d && d<=1.0;
        }

        static Star()
        {
            AffectsGeometry<Star>(BoundsProperty, StrokeThicknessProperty, NumberOfSpikesProperty, InnerRadiusRatioProperty);
            StrokeThicknessProperty.OverrideDefaultValue<Star>(.5);
        }


        /// <summary>
        /// Sets or gets the number of spikes this regular star will have.
        /// </summary>
        public int NumberOfSpikes
        {
            get { return GetValue(NumberOfSpikesProperty); }
            set { SetValue(NumberOfSpikesProperty, value); }
        }

        /// <summary>
        /// Defines the percentage where the spike inset shall be in relation to the spike itself.
        /// </summary>
        public double InnerRadiusRatio
        {
            get { return GetValue(InnerRadiusRatioProperty); }
            set { SetValue(InnerRadiusRatioProperty, value); }
        }

        protected override Geometry CreateDefiningGeometry()
        {
            Point[] cpoints = new Point[NumberOfSpikes*2];
            double rx = Bounds.Width / 2, ry = Bounds.Height / 2;
            //double rxi = rx * InnerRadiusRatio, ryi = ry * InnerRadiusRatio;
            for (int i = 0; i < cpoints.Length; i++)
            {
                double rat = i % 2 == 0 ? 1.0 : InnerRadiusRatio;
                double φ = 2 * Math.PI / cpoints.Length * i - Math.PI / 2;
                cpoints[i] = new Point( rx + Math.Cos(φ) * rx * rat, ry + Math.Sin(φ) * ry * rat);
            }

            return new PolylineGeometry(cpoints, true);
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            return new Size(StrokeThickness, StrokeThickness);
        }

    }
}
