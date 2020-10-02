using System;
using System.Collections.Generic;
using System.Text;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Shapes;
using Avalonia.Media;

namespace AvaDevBox.Controls
{
    /// <summary>
    /// This is a very simple Control. You place it anywhere
    /// when the property <see cref="IsActive"/> is set to true the busy
    /// indicator spins or wobbles (depending on the theme).
    /// There are no interactions.
    /// </summary>
    public class BusyIndicator : TemplatedControl
    {
        /// <summary>
        /// Defines the <see cref="IsActive"/> property.
        /// </summary>
        public static readonly StyledProperty<bool> IsActiveProperty =
            AvaloniaProperty.Register<BusyIndicator, bool>(nameof(IsActive), true);

        /// <summary>
        /// Indicates whether the Busy Indicator is spinning or anyways active.
        /// </summary>
        public bool IsActive
        {
            get => GetValue(IsActiveProperty);
            set => SetValue(IsActiveProperty, value);
        }

        public BusyIndicator()
        {
            
            
        }
    }
}
