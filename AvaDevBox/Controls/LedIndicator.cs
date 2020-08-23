using System;
using System.Collections.Generic;
using System.Text;
using AvaDevBox.Utils;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Shapes;

namespace AvaDevBox.Controls
{
    public class LedIndicator : TemplatedControl
    {
        public static readonly DirectProperty<LedIndicator, bool?> IsCheckedProperty =
            ToggleButton.IsCheckedProperty.AddOwner<LedIndicator>(o=>o.IsChecked, (o,v) => o.IsChecked = v);

        private bool? _isChecked;

        public bool? IsChecked
        {
            get { return _isChecked; }
            set { SetAndRaise(IsCheckedProperty, ref _isChecked, value); }
        }

    }
}
