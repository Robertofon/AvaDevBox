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
using Color = System.Drawing.Color;

namespace AvaDevBox.Controls
{
    public class LedIndicator : TemplatedControl
    {
        public static readonly DirectProperty<LedIndicator, bool?> IsCheckedProperty =
            ToggleButton.IsCheckedProperty.AddOwner<LedIndicator>(o => o.IsChecked, (o,v) => o.IsChecked = v);

        public static readonly StyledProperty<IBrush> OnColorProperty =
            AvaloniaProperty.Register<LedIndicator, IBrush>(nameof(OnColor), Brushes.Transparent, false, BindingMode.Default);

        private bool? _isChecked;

        static LedIndicator()
        {
            PseudoClass<LedIndicator, bool?>(IsCheckedProperty, c => c == true, ":checked");
            PseudoClass<LedIndicator, bool?>(IsCheckedProperty, c => c == false, ":unchecked");
            PseudoClass<LedIndicator, bool?>(IsCheckedProperty, c => c == null, ":indeterminate");
        }


        public bool? IsChecked
        {
            get { return _isChecked; }
            set { SetAndRaise(IsCheckedProperty, ref _isChecked, value); }
        }

        public IBrush OnColor
        {
            get => GetValue(OnColorProperty);
            set => SetValue(OnColorProperty, value);
        }
    }
}
