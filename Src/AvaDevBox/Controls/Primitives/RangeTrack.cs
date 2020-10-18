using System;
using System.Collections.Generic;
using System.Text;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Layout;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Utilities;

namespace AvaDevBox.Controls.Primitives
{
    /// <summary>
    /// Implements an extended version of <see cref="Track"/> which
    /// allows us to add a second thumb in the track. So a
    /// range display becomes possible.
    /// </summary>
    public class RangeTrack : Track
    {
        /// <summary>
        /// Defines the Track2 property.
        /// </summary>
        public static readonly StyledProperty<Thumb> Thumb2Property =
            AvaloniaProperty.Register<RangeTrack, Thumb>(nameof(Thumb2));

        public static readonly StyledProperty<Button> SelectRangeButtonProperty =
            AvaloniaProperty.Register<RangeTrack, Button>(nameof(SelectRangeButton));

        public static readonly DirectProperty<RangeTrack, double> Value2Property =
            RangeBase.ValueProperty.AddOwner<RangeTrack>(o => o.Value2, (o, v) => o.Value2 = v);

        private double _value2;

        private double Density { get; set; }

        private double ThumbCenterOffset { get; set; }
        private double Thumb2CenterOffset { get; set; }

        static RangeTrack()
        {
            Thumb2Property.Changed.AddClassHandler<RangeTrack>((x, e) => x.Thumb2Changed(e));
            AffectsArrange<RangeTrack>(MinimumProperty, MaximumProperty, ValueProperty, OrientationProperty, Value2Property);
        }

        public RangeTrack()
        {
            //base.UpdatePseudoClasses(Orientation);
        }

        public Thumb Thumb2
        {
            get => GetValue(Thumb2Property);
            set => SetValue(Thumb2Property, value);
        }

        public double Value2
        {
            get { return _value2; }
            set { SetAndRaise(ValueProperty, ref _value2, value); }
        }

        public Button SelectRangeButton
        {
            get { return GetValue(SelectRangeButtonProperty); }
            set { SetValue(SelectRangeButtonProperty, value); }
        }

        private void Thumb2Changed(AvaloniaPropertyChangedEventArgs e)
        {
            var oldThumb = (Thumb)e.OldValue;
            var newThumb = (Thumb)e.NewValue;

            if (oldThumb != null)
            {
                oldThumb.DragDelta -= Thumb2Dragged;

                LogicalChildren.Remove(oldThumb);
                VisualChildren.Remove(oldThumb);
            }

            if (newThumb != null)
            {
                newThumb.DragDelta += Thumb2Dragged;
                LogicalChildren.Add(newThumb);
                VisualChildren.Add(newThumb);
            }
        }

        private void Thumb2Dragged(object sender, VectorEventArgs e)
        {
            if (IsThumbDragHandled)
                return;

            Value2 = MathUtilities.Clamp(
                Value2 + ValueFromDistance(e.Vector.X, e.Vector.Y),
                Minimum,
                Maximum);
        }

        private static void CoerceLength(ref double componentLength, double trackLength)
        {
            if (componentLength < 0)
            {
                componentLength = 0.0;
            }
            else if (componentLength > trackLength || double.IsNaN(componentLength))
            {
                componentLength = trackLength;
            }
        }

        private void ComputeSliderLengths(Size arrangeSize, bool isVertical, out double decreaseButtonLength, out double thumb1Length, 
            out double selRangeLength, out double thumb2Length, out double increaseButtonLength)
        {
            double min = Minimum;
            double range = Math.Max(0.0, Maximum - min);
            double offset1 = Math.Min(range, Value - min);
            double offset2 = Math.Min(range, Value2 - min);

            double trackLength;

            // Compute thumb size
            if (isVertical)
            {
                trackLength = arrangeSize.Height;
                thumb1Length = Thumb?.DesiredSize.Height ?? 0;
                thumb2Length = Thumb2?.DesiredSize.Height ?? 0;
            }
            else
            {
                trackLength = arrangeSize.Width;
                thumb1Length = Thumb?.DesiredSize.Width ?? 0;
                thumb2Length = Thumb2?.DesiredSize.Width ?? 0;
            }

            CoerceLength(ref thumb1Length, trackLength);
            CoerceLength(ref thumb2Length, trackLength);

            double remainingTrackLength = trackLength - thumb1Length - thumb2Length;

            decreaseButtonLength = remainingTrackLength * offset1 / range;
            CoerceLength(ref decreaseButtonLength, remainingTrackLength);

            selRangeLength = (remainingTrackLength * offset2 / range) - decreaseButtonLength;
            CoerceLength(ref selRangeLength, remainingTrackLength);

            increaseButtonLength = remainingTrackLength - decreaseButtonLength - selRangeLength;
            CoerceLength(ref increaseButtonLength, remainingTrackLength);

            Density = range / remainingTrackLength;
        }

        protected override Size ArrangeOverride(Size arrangeSize)
        {
            double decreaseButtonLength, thumb1Length, increaseButtonLength;
            double thumb2Length, selRangeLength;
            var isVertical = Orientation == Orientation.Vertical;
            var viewportSize = Math.Max(0.0, ViewportSize);

            // If viewport is NaN, compute thumb's size based on its desired size,
            // otherwise compute the thumb base on the viewport and extent properties
            if (double.IsNaN(ViewportSize))
            {
                ComputeSliderLengths(arrangeSize, isVertical, out decreaseButtonLength, out thumb1Length, out selRangeLength, out thumb2Length, out increaseButtonLength);
            }
            else
            {
                // Don't arrange if there's not enough content or the track is too small
                if (!ComputeScrollBarLengths(arrangeSize, viewportSize, isVertical, out decreaseButtonLength, out thumb1Length, out increaseButtonLength))
                {
                    return arrangeSize;
                }
            }

            // Layout the pieces of track
            var offset = new Point();
            var pieceSize = arrangeSize;
            var isDirectionReversed = IsDirectionReversed;

            if (isVertical)
            {
                CoerceLength(ref decreaseButtonLength, arrangeSize.Height);
                CoerceLength(ref increaseButtonLength, arrangeSize.Height);
                CoerceLength(ref thumb1Length, arrangeSize.Height);
                CoerceLength(ref thumb2Length, arrangeSize.Height);
                CoerceLength(ref selRangeLength, arrangeSize.Height);

                offset = offset.WithY(isDirectionReversed ? decreaseButtonLength + thumb1Length + selRangeLength + thumb2Length : 0.0);
                pieceSize = pieceSize.WithHeight(increaseButtonLength);

                if (IncreaseButton != null)
                {
                    IncreaseButton.Arrange(new Rect(offset, pieceSize));
                }

                offset = offset.WithY(isDirectionReversed ? 0.0 : increaseButtonLength + thumb1Length + selRangeLength + thumb2Length);
                pieceSize = pieceSize.WithHeight(decreaseButtonLength);

                if (DecreaseButton != null)
                {
                    DecreaseButton.Arrange(new Rect(offset, pieceSize));
                }

                offset = offset.WithY(isDirectionReversed ? decreaseButtonLength + thumb2Length + selRangeLength : increaseButtonLength);
                pieceSize = pieceSize.WithHeight(thumb1Length);

                if (Thumb != null)
                {
                    Thumb.Arrange(new Rect(offset, pieceSize));
                }

                ThumbCenterOffset = offset.Y + (thumb1Length * 0.5);

                offset = offset.WithY(isDirectionReversed ? decreaseButtonLength + thumb2Length : increaseButtonLength + thumb1Length);
                pieceSize = pieceSize.WithHeight(selRangeLength);
                
                if (SelectRangeButton != null)
                {
                    SelectRangeButton.Arrange(new Rect(offset, pieceSize));
                }

                offset = offset.WithY(isDirectionReversed ? decreaseButtonLength : increaseButtonLength + thumb1Length + selRangeLength);
                pieceSize = pieceSize.WithHeight(thumb2Length);

                if (Thumb2 != null)
                {
                    Thumb2.Arrange(new Rect(offset, pieceSize));
                }

                Thumb2CenterOffset = offset.Y + (thumb2Length * 0.5);
            }
            else
            {
                CoerceLength(ref decreaseButtonLength, arrangeSize.Width);
                CoerceLength(ref increaseButtonLength, arrangeSize.Width);
                CoerceLength(ref thumb1Length, arrangeSize.Width);
                CoerceLength(ref thumb2Length, arrangeSize.Width);
                CoerceLength(ref selRangeLength, arrangeSize.Width);

                offset = offset.WithX(isDirectionReversed ? increaseButtonLength + thumb1Length + selRangeLength + thumb2Length : 0.0);
                pieceSize = pieceSize.WithWidth(decreaseButtonLength);

                if (DecreaseButton != null)
                {
                    DecreaseButton.Arrange(new Rect(offset, pieceSize));
                }

                offset = offset.WithX(isDirectionReversed ? 0.0 : decreaseButtonLength + thumb1Length + selRangeLength + thumb2Length);
                pieceSize = pieceSize.WithWidth(increaseButtonLength);

                if (IncreaseButton != null)
                {
                    IncreaseButton.Arrange(new Rect(offset, pieceSize));
                }

                offset = offset.WithX(isDirectionReversed ? decreaseButtonLength + thumb2Length + selRangeLength : increaseButtonLength);
                pieceSize = pieceSize.WithWidth(thumb1Length);

                if (Thumb != null)
                {
                    Thumb.Arrange(new Rect(offset, pieceSize));
                }

                ThumbCenterOffset = offset.X + (thumb1Length * 0.5);

                offset = offset.WithY(isDirectionReversed ? decreaseButtonLength + thumb2Length : increaseButtonLength + thumb1Length);
                pieceSize = pieceSize.WithWidth(selRangeLength);

                if (SelectRangeButton != null)
                {
                    SelectRangeButton.Arrange(new Rect(offset, pieceSize));
                }

                offset = offset.WithX(isDirectionReversed ? decreaseButtonLength : increaseButtonLength + thumb1Length + selRangeLength);
                pieceSize = pieceSize.WithWidth(thumb2Length);

                if (Thumb2 != null)
                {
                    Thumb2.Arrange(new Rect(offset, pieceSize));
                }

                Thumb2CenterOffset = offset.X + (thumb2Length * 0.5);
            }

            return arrangeSize;
        }


    }
}
