using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Presenters;
using Avalonia.Controls.Primitives;
using Avalonia.Styling;

namespace AvaloniaControls
{
    /// <summary>
    /// This is a control that displays a defined number of stars and the user
    /// can click a rating from 0 to n stars resulting in <see cref="Value"/> being
    /// set to a value between 0 and 1.
    /// </summary>
    public class RatingControl : TemplatedControl, ITemplatedControl
    {
        public static readonly StyledProperty<int> NumberOfStarsProperty =
            AvaloniaProperty.Register<RatingControl, int>(nameof(NumberOfStars), 6, validate: ValidateNumberOfStars, notifying: NotifyNumberOfStars);

        public static readonly StyledProperty<double> ValueProperty =
            AvaloniaProperty.Register<RatingControl, double>(nameof(NumberOfStars), 6, validate: ValidateValue);

        /// <summary>
        /// Defines the <see cref="StarItems"/> property.
        /// </summary>
        public static readonly DirectProperty<RatingControl, IEnumerable> StarItemsProperty =
            AvaloniaProperty.RegisterDirect<RatingControl, IEnumerable>(nameof(StarItems), o => o.StarItems);//, (o, v) => o.StarItems = v);

        private  IEnumerable _starItems;

        static RatingControl()
        {
            ContentPresenter.ContentTemplateProperty.AddOwner<RatingControl>();
            //NumberOfStarsProperty.Changed.AddClassHandler(x => x.OnNumberOfStarsChanged);
            //TemplateProperty.OverrideDefaultValue(typeof());
        }

        private static int ValidateNumberOfStars(RatingControl arg1, int val)
        {
            return val.LimitTo(1, 100);
        }

        private static void NotifyNumberOfStars(IAvaloniaObject arg1, bool arg2)
        {
            ((RatingControl)arg1).UpdateStars();
        }

        private static double ValidateValue(RatingControl arg1, double val)
        {
            return val.LimitTo(0.0, 1.0);
        }

        /// <summary>
        /// Sets or gets the number of stars to use for this rating.
        /// </summary>
        public int NumberOfStars
        {
            get { return GetValue(NumberOfStarsProperty); }
            set { SetValue(NumberOfStarsProperty, value); }
        }

        /// <summary>
        /// Sets or gets the rating value of this control.
        /// </summary>
        /// <value>Between 0.0 and 1.0 (best).</value>
        public double Value
        {
            get { return GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        public IEnumerable StarItems
        {
            get { return _starItems; }
            private set { SetAndRaise(StarItemsProperty, ref _starItems, value); }
        }

        private void UpdateStars()
        {
            StarItems = Enumerable.Repeat("S", NumberOfStars);
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();
            this.UpdateStars();
            //RaisePropertyChanged(StarItemsProperty,  );
        }
    }
}
