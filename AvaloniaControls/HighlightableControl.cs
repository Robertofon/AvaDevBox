using System;
using System.Collections.Generic;
using System.Text;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Media;

namespace AvaloniaControls
{
    /// <summary>
    /// Control that is selectable and thus changes color.
    /// </summary>
    public class HighlightableControl : Button, ISelectable
    {
        /// <summary>
        /// Defines the <see cref="IsSelected"/> property.
        /// </summary>
        public static readonly StyledProperty<bool> IsSelectedProperty =
            ListBoxItem.IsSelectedProperty.AddOwner<HighlightableControl>();
        
        /// <summary>
        /// Defines the  property.
        /// </summary>
        public static readonly StyledProperty<IBrush> HighlightBrushProperty =
            AvaloniaProperty.Register<HighlightableControl, IBrush>(nameof(HighlightBrush));


        static HighlightableControl()
        {
            AffectsRender<HighlightableControl>(IsSelectedProperty, HighlightBrushProperty);
        }

        /// <summary>
        /// Gets or sets a value indicating whether the <see cref="HighlightableControl"/> is currently selected.
        /// </summary>
        public bool IsSelected
        {
            get { return GetValue(IsSelectedProperty); }
            set { SetValue(IsSelectedProperty, value); }
        }

        /// <summary>
        /// Gets or sets the brush used to draw the control's highlight accent.
        /// </summary>
        public IBrush HighlightBrush
        {
            get { return GetValue(HighlightBrushProperty); }
            set { SetValue(HighlightBrushProperty, value); }
        }

        protected override void OnClick()
        {
            Toggle();
            base.OnClick();
        }

        protected virtual void Toggle()
        {
            IsSelected = !IsSelected;
        }

    }
}
