using System;
using AvaDevBox.Utils;
using Avalonia;
using Avalonia.Controls.Shapes;
using Avalonia.Media;
using Avalonia.Platform;
using Point = Avalonia.Point;

namespace AvaDevBox.Controls.Shapes
{
    /// <summary>
    /// A control that displays ticks in it's client area based on parameters.
    /// </summary>
    public class RulerShape : Shape
    {
        public static readonly StyledProperty<int> TickFreq1Property =
            AvaloniaProperty.Register<Shape, int>(nameof(TickFreq1), 2, validate: ValidateTickFreq1);

        public static readonly StyledProperty<int> TickFreq2Property =
            AvaloniaProperty.Register<Shape, int>(nameof(TickFreq2), 5, validate: ValidateTickFreq2);

        public static readonly StyledProperty<double> SmallTickDistProperty =
            AvaloniaProperty.Register<Shape, double>(nameof(SmallTickDist), 0.4, validate: ValidateSmallTickDist);

        private static double ValidateSmallTickDist(Shape arg1, double val)
        {
            return val.LimitToAtLeast(0.5);
        }

        private static int ValidateTickFreq1(Shape arg1, int arg2)
        {
            return arg2.LimitTo(1, 50);
        }

        private static int ValidateTickFreq2(Shape arg1, int arg2)
        {
            return arg2.LimitTo(1, 50);
        }

        static RulerShape()
        {
            AffectsGeometry<RulerShape>(BoundsProperty, StrokeThicknessProperty, SmallTickDistProperty, TickFreq1Property, TickFreq2Property);
        }

        /// <summary>
        /// Defines the small ticks (1) frequency.
        /// </summary>
        public int TickFreq1
        {
            get { return GetValue(TickFreq1Property); }
            set { SetValue(TickFreq1Property, value); }
        }

        /// <summary>
        /// Defines the small ticks (2) frequency.
        /// </summary>
        public int TickFreq2
        {
            get { return GetValue(TickFreq2Property); }
            set { SetValue(TickFreq2Property, value); }
        }

        /// <summary>
        /// Defines the small ticks distance.
        /// </summary>
        public double SmallTickDist
        {
            get { return GetValue(SmallTickDistProperty); }
            set { SetValue(SmallTickDistProperty, value); }
        }

        protected override Geometry CreateDefiningGeometry()
        {
            var r = new Rect(Bounds.Size).Deflate(StrokeThickness);
            var geometry = new StreamGeometry();
            var g = geometry.Open();
            g.BeginFigure(r.TopLeft, false);
            g.LineTo(r.TopRight);
            g.EndFigure(false);

            int idx = 0;
            for (double x = r.X; x < r.Right; x += this.SmallTickDist)
            {
                double rady = 0.6;
                rady = idx % TickFreq1 == 0 ? 0.75 : rady;
                rady = idx % TickFreq2 == 0 ? 1.0 : rady;
                g.BeginFigure(new Point(x, r.Y), false);
                g.LineTo(new Point(x, r.Bottom * rady));
                g.EndFigure(false);
                idx++;
            }

            return geometry;
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            return new Size(StrokeThickness, StrokeThickness);
        }

    }
}
