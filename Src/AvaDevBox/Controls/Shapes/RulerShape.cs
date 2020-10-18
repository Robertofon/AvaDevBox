using System;
using AvaDevBox.Utils;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Shapes;
using Avalonia.Layout;
using Avalonia.Media;
using Avalonia.Platform;
using Avalonia.Utilities;
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
    /// It allows three levels of Ticks. So it is easily possible to
    /// configure it as centimeter+millimeter ruler or as inch with fraction ruler.
    /// </summary>
    public class RulerShape : Shape
    {
        /// <summary>
        /// Defines the <see cref="Orientation"/> property.
        /// </summary>
        public static readonly StyledProperty<Orientation> OrientationProperty =
            ScrollBar.OrientationProperty.AddOwner<RulerShape>();

        /// <summary>
        /// Defines the <see cref="PhaseShift"/> property.
        /// </summary>
        public static readonly StyledProperty<double> PhaseShiftProperty =
            AvaloniaProperty.Register<RulerShape, double>(nameof(PhaseShift), 0, coerce: CoercePhaseShift);

        /// <summary>
        /// Defines the <see cref="TickFreq1"/> property.
        /// </summary>
        public static readonly StyledProperty<int> TickFreq1Property =
            AvaloniaProperty.Register<RulerShape, int>(nameof(TickFreq1), 2, coerce: CoerceTickFreq1);

        /// <summary>
        /// Defines the <see cref="TickFreq2"/> property.
        /// </summary>
        public static readonly StyledProperty<int> TickFreq2Property =
            AvaloniaProperty.Register<RulerShape, int>(nameof(TickFreq2), 5, coerce: CoerceTickFreq2);

        /// <summary>
        /// Defines the <see cref="SmallTickDist"/> property.
        /// </summary>
        public static readonly StyledProperty<double> SmallTickDistProperty =
            AvaloniaProperty.Register<RulerShape, double>(nameof(SmallTickDist), 0.5, coerce: CoerceSmallTickDist);

        /// <summary>
        /// Defines the <see cref="ConnectionLine"/> property.
        /// </summary>
        public static readonly StyledProperty<ConnectionLinePlacement> ConnectionLineProperty =
            AvaloniaProperty.Register<RulerShape, ConnectionLinePlacement>(nameof(ConnectionLine), ConnectionLinePlacement.Center);

        private static double CoercePhaseShift(IAvaloniaObject avaloniaObject, double val)
        {
            return double.IsInfinity(val) || double.IsNaN(val) ? 0d : val;
        }

        private static double CoerceSmallTickDist(IAvaloniaObject avaloniaObject, double val)
        {
            return val.LimitToAtLeast(0.5);
        }

        private static int CoerceTickFreq1(IAvaloniaObject avaloniaObject, int val)
        {
            return MathUtilities.Clamp(val, 1, 50);
        }

        private static int CoerceTickFreq2(IAvaloniaObject avaloniaObject, int val)
        {
            return MathUtilities.Clamp(val, 1, 50);
        }

        static RulerShape()
        {
            StrokeThicknessProperty.OverrideDefaultValue<RulerShape>(1);
            AffectsGeometry<RulerShape>(BoundsProperty, StrokeThicknessProperty, SmallTickDistProperty, 
                TickFreq1Property, TickFreq2Property, ConnectionLineProperty, OrientationProperty, PhaseShiftProperty);
        }

        /// <summary>
        /// Defines the phase shift, the ticks shall be shifted by at the origin.
        /// </summary>
        public double PhaseShift
        {
            get { return GetValue(PhaseShiftProperty); }
            set { SetValue(PhaseShiftProperty, value); }
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
            int tickFreq2 = TickFreq2;
            int tickFreq1 = TickFreq1;
            double smallTickDist = SmallTickDist;
            decimal ph = (decimal)this.PhaseShift;
            ph = ph % (decimal) smallTickDist;
            if (ph < 0)
            {
                ph += (decimal) smallTickDist;
            }
            var r = new Rect(Bounds.Size).Deflate(StrokeThickness);
            var geometry = new StreamGeometry();
            var g = geometry.Open();

            var cbegin = new Point(0, 0);
            var cend = new Point(0, 0);
            double rady1 = 0, rady2 = 0, radx1 = 0, radx2 =0;
            var orientation = Orientation;
            ConnectionLinePlacement connectionLine = ConnectionLine;
            if (orientation == Orientation.Horizontal)
            {
                radx1 = radx2 = 0;
                switch (connectionLine)
                {
                    case ConnectionLinePlacement.None:
                    case ConnectionLinePlacement.Center:
                        cbegin = new Point(r.Left, r.Center.Y);
                        cend = new Point(r.Right, r.Center.Y);
                        rady1 = -.5;
                        rady2 = +.5;
                        break;
                    case ConnectionLinePlacement.RightOrBottom:
                        cbegin = new Point(r.Left, r.Bottom);
                        cend = new Point(r.Right, r.Bottom);
                        rady1 = -1;
                        rady2 = 0;
                        break;
                    case ConnectionLinePlacement.TopOrLeft:
                        cbegin = new Point(r.Left, r.Top);
                        cend = new Point(r.Right, r.Top);
                        rady1 = 0;
                        rady2 = 1;
                        break;
                    default:
                        break;
                }
            }
            else
            {
                rady1 = rady2 = 0;
                switch (connectionLine)
                {
                    case ConnectionLinePlacement.None:
                    case ConnectionLinePlacement.Center:
                        cbegin = new Point(r.Center.X, r.Top);
                        cend = new Point(r.Center.X, r.Bottom);
                        radx1 = -.5;
                        radx2 = +.5;
                        break;
                    case ConnectionLinePlacement.RightOrBottom:
                        cbegin = new Point(r.Right, r.Top);
                        cend = new Point(r.Right, r.Bottom);
                        radx1 = -1;
                        radx2 = 0;
                        break;
                    case ConnectionLinePlacement.TopOrLeft:
                        cbegin = new Point(r.Left, r.Top);
                        cend = new Point(r.Left, r.Bottom);
                        radx1 = 0;
                        radx2 = +1;
                        break;
                    default:
                        break;

                }
            }

            if (connectionLine != ConnectionLinePlacement.None)
            {
                g.BeginFigure(cbegin, false);
                g.LineTo(cend);
                g.EndFigure(false);
            }

            int idx = 0;
            double virtx1 = orientation == Orientation.Horizontal ? r.Left : r.Top;
            double virtx2 = orientation == Orientation.Horizontal ? r.Right : r.Bottom;
            double virY = orientation == Orientation.Horizontal ? cbegin.Y : cbegin.X;
            for (double x = virtx1 + (double)ph; x < virtx2; x += smallTickDist)
            {
                double d = 0.6;
                d = idx % tickFreq1 == 0 ? 0.75 : d;
                d = idx % tickFreq2 == 0 ? 1.0 : d;
                if (orientation == Orientation.Horizontal)
                {
                    g.BeginFigure(new Point(x, virY + r.Height * rady1 * d), false);
                    g.LineTo(new Point(x, virY + r.Height * rady2 * d));
                }
                else
                {
                    g.BeginFigure(new Point(virY + r.Width * radx1 * d, x), false);
                    g.LineTo(new Point(virY + r.Width * radx2 * d, x));
                }

                g.EndFigure(false);
                idx++;
            }

            return geometry;
        }

        /// <inheritdoc />
        protected override Size MeasureOverride(Size availableSize)
        {
            return new Size(StrokeThickness, StrokeThickness);
        }
    }
}
