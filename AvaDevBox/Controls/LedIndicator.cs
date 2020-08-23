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

        public static readonly DirectProperty<LedIndicator, IBrush> OnColorProperty =
            AvaloniaProperty.RegisterDirect<LedIndicator, IBrush>(nameof(OnColor), o => o.OnColor, (o, v) => o.OnColor = v, Brushes.Transparent,
                BindingMode.Default);

        private bool? _isChecked;
        private IBrush _onColor;

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
            get => _onColor;
            set => SetAndRaise(OnColorProperty, ref _onColor, value);
        }
    }
}
