using System;
using System.Collections.Generic;
using System.Text;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Data;
using Avalonia.Media;

namespace AvaDevBox.Controls
{
    public class LedButton : ToggleButton
    {
        public static readonly StyledProperty<Color> OnColorProperty =
            LedIndicator.OnColorProperty.AddOwner<LedButton>();

        static LedButton()
        {
            FocusableProperty.OverrideDefaultValue(typeof(LedButton), true);
        }

        public Color OnColor
        {
            get => GetValue(OnColorProperty);
            set => SetValue(OnColorProperty, value);
        }
    }
}
