using System;
using System.Collections.Generic;
using System.Text;
using Avalonia;
using Avalonia.Controls.Primitives;

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


        public Thumb Thumb2
        {
            get => GetValue(Thumb2Property);
            set => SetValue(Thumb2Property, value);
        }
    }
}
