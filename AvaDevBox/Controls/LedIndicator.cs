using System;
using System.Collections.Generic;
using System.Text;
using AvaDevBox.Utils;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Shapes;
using Avalonia.Data;
using Avalonia.Media;

namespace AvaDevBox.Controls
{
    public class LedIndicator : TemplatedControl
    {
        public static readonly DirectProperty<LedIndicator, bool?> IsCheckedProperty =
            ToggleButton.IsCheckedProperty.AddOwner<LedIndicator>(o => o.IsChecked, (o,v) => o.IsChecked = v);

        public static readonly StyledProperty<Color> OnColorProperty =
            AvaloniaProperty.Register<LedIndicator, Color>(nameof(OnColor), Colors.Red, false, BindingMode.Default);

        private bool? _isChecked;

        public LedIndicator()
        {
            UpdatePseudoClasses(IsChecked);
        }

        public bool? IsChecked
        {
            get { return _isChecked; }
            set 
            { 
                SetAndRaise(IsCheckedProperty, ref _isChecked, value); UpdatePseudoClasses(value);
            }
        }

        public Color OnColor
        {
            get => GetValue(OnColorProperty);
            set => SetValue(OnColorProperty, value);
        }

        private void UpdatePseudoClasses(bool? isChecked)
        {
            PseudoClasses.Set(":checked", isChecked == true);
            PseudoClasses.Set(":unchecked", isChecked == false);
            PseudoClasses.Set(":indeterminate", isChecked == null);
        }
    }
}
