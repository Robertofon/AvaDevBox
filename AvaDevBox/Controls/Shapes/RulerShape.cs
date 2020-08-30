using System;
using AvaDevBox.Utils;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Shapes;
using Avalonia.Layout;
using Avalonia.Media;
using Avalonia.Platform;
using Point = Avalonia.Point;

namespace AvaDevBox.Controls.Shapes
{
    /// <summary>
    /// Enum which describes the position of the connecting line of the <see cref="RulerShape"/>
    /// </summary>
    public enum ConnectionLinePlacement
    {
        /// <summary>
        /// No line is drawn.
        /// </summary>
        None,

        /// <summary>
        /// Placed top or left of the ticks
        /// </summary>
        TopOrLeft,

        /// <summary>
        /// Placed right or at the bottom of the ticks.
        /// </summary>
        RightOrBottom,

        /// <summary>
        /// Placed in the center of the ticks.
        /// </summary>
        Center
    }

    /// <summary>
    /// A control that displays ticks in it's client area based on parameters.
    /// </summary>
    public class RulerShape : Shape
    {
        /// <summary>
        /// Defines the <see cref="Orientation"/> property.
        /// </summary>
        public static readonly StyledProperty<Orientation> OrientationProperty =
            ScrollBar.OrientationProperty.AddOwner<RulerShape>();

        public static readonly StyledProperty<int> TickFreq1Property =
            AvaloniaProperty.Register<RulerShape, int>(nameof(TickFreq1), 2, coerce: CoerceTickFreq1);

        public static readonly StyledProperty<int> TickFreq2Property =
            AvaloniaProperty.Register<RulerShape, int>(nameof(TickFreq2), 5, coerce: CoerceTickFreq2);

        public static readonly StyledProperty<double> SmallTickDistProperty =
            AvaloniaProperty.Register<RulerShape, double>(nameof(SmallTickDist), 0.4, coerce: CoerceSmallTickDist);

        public static readonly StyledProperty<ConnectionLinePlacement> ConnectionLineProperty =
            AvaloniaProperty.Register<RulerShape, ConnectionLinePlacement>(nameof(ConnectionLine), ConnectionLinePlacement.Center);

        private static double CoerceSmallTickDist(IAvaloniaObject avaloniaObject, double val)
        {
            return val.LimitToAtLeast(0.5);
        }

        private static int CoerceTickFreq1(IAvaloniaObject avaloniaObject, int arg2)
        {
            return arg2.LimitTo(1, 50);
        }

        private static int CoerceTickFreq2(IAvaloniaObject avaloniaObject, int arg2)
        {
            return arg2.LimitTo(1, 50);
        }

        static RulerShape()
        {
            AffectsGeometry<RulerShape>(BoundsProperty, StrokeThicknessProperty, SmallTickDistProperty, 
                TickFreq1Property, TickFreq2Property, ConnectionLineProperty, OrientationProperty);
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

        /// <summary>
        /// Gets or sets  whether a connecting line is drawn which connects all ticks.
        /// </summary>
        public ConnectionLinePlacement ConnectionLine
        {
            get { return GetValue(ConnectionLineProperty); }
            set { SetValue(ConnectionLineProperty, value); }
        }

        /// <summary>
        /// Ruler's orientation.
        /// </summary>
        public Orientation Orientation
        {
            get { return GetValue(OrientationProperty); }
            set { SetValue(OrientationProperty, value); }
        }

        protected override Geometry CreateDefiningGeometry()
        {
            var r = new Rect(Bounds.Size).Deflate(StrokeThickness);
            var geometry = new StreamGeometry();
            var g = geometry.Open();

            var cbegin = new Point(0, 0);
            var cend = new Point(0, 0);
            if (Orientation == Orientation.Horizontal)
            {
                switch (ConnectionLine)
                {
                    case ConnectionLinePlacement.Center:
                        cbegin = new Point(r.Left, r.Center.Y);
                        cend = new Point(r.Right, r.Center.Y);
                        break;
                    case ConnectionLinePlacement.RightOrBottom:
                        cbegin = new Point(r.Left, r.Bottom);
                        cend = new Point(r.Right, r.Bottom);
                        break;
                    case ConnectionLinePlacement.TopOrLeft:
                        cbegin = new Point(r.Left, r.Top);
                        cend = new Point(r.Right, r.Top);
                        break;
                    default:
                        break;
                }
            }
            else
            {
                switch (ConnectionLine)
                {
                    case ConnectionLinePlacement.Center:
                        cbegin = new Point(r.Center.X, r.Top);
                        cend = new Point(r.Center.X, r.Bottom);
                        break;
                    case ConnectionLinePlacement.RightOrBottom:
                        cbegin = new Point(r.Right, r.Top);
                        cend = new Point(r.Right, r.Bottom);
                        break;
                    case ConnectionLinePlacement.TopOrLeft:
                        cbegin = new Point(r.Left, r.Top);
                        cend = new Point(r.Left, r.Bottom);
                        break;
                    default:
                        break;

                }
            }

            if (ConnectionLine != ConnectionLinePlacement.None)
            {
                g.BeginFigure(r.TopLeft, false);
                g.LineTo(r.TopRight);
                g.EndFigure(false);
            }

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
