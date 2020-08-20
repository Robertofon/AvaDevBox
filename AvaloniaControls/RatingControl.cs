using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Disposables;
using System.Runtime.CompilerServices;
using System.Text;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Presenters;
using Avalonia.Controls.Primitives;
using Avalonia.Data;
using Avalonia.Styling;
using JetBrains.Annotations;

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
            AvaloniaProperty.Register<RatingControl, int>(nameof(NumberOfStars), 6, validate: ValidateNumberOfStars);

        public static readonly StyledProperty<double> ValueProperty =
            AvaloniaProperty.Register<RatingControl, double>(nameof(Value), 0, validate: ValidateValue);

        /// <summary>
        /// Defines the <see cref="StarItems"/> property.
        /// </summary>
        public static readonly DirectProperty<RatingControl, IEnumerable<StarItem>> StarItemsProperty =
            AvaloniaProperty.RegisterDirect<RatingControl, IEnumerable<StarItem>>(nameof(StarItems), o => o.StarItems);//, (o, v) => o.StarItems = v);

        private IEnumerable<StarItem> _starItems;
        private ItemsPresenter _starsPresenter;

        static RatingControl()
        {
            ContentPresenter.ContentTemplateProperty.AddOwner<RatingControl>();
            NumberOfStarsProperty.Changed.Subscribe(OnNumberOfStarsChanged);
            ValueProperty.Changed.Subscribe(OnValueChanged);
            AffectsRender<RatingControl>(NumberOfStarsProperty, ValueProperty);
            AffectsMeasure<RatingControl>(NumberOfStarsProperty);
        }

        private static int ValidateNumberOfStars(RatingControl arg1, int val)
        {
            return val.LimitTo(1, 100);
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

        public IEnumerable<StarItem> StarItems
        {
            get { return _starItems; }
            private set { SetAndRaise(StarItemsProperty, ref _starItems, value); }
        }

        /// <summary>
        /// ItemsProperty property changed handler.
        /// </summary>
        /// <param name="e">AvaloniaPropertyChangedEventArgs.</param>
        private static void OnNumberOfStarsChanged(AvaloniaPropertyChangedEventArgs e)
        {
            if (e.Sender is RatingControl rating)
            {
                var newNumber = (int)e.NewValue;
                UpdateStars(rating, newNumber, rating.Value);
            }
        }

        /// <summary>
        /// ItemsProperty property changed handler.
        /// </summary>
        /// <param name="e">AvaloniaPropertyChangedEventArgs.</param>
        private static void OnValueChanged(AvaloniaPropertyChangedEventArgs e)
        {
            if (e.Sender is RatingControl rating)
            {
                var newValue = (double)e.NewValue;
                UpdateStars(rating, rating.NumberOfStars, newValue);
            }
        }

        private static void UpdateStars(RatingControl rating, int newNumber, double newValue)
        {
            // new items with events
            List<StarItem> ratingStarItems = Enumerable.Range(0, newNumber).Select(a => new StarItem(a) { IsSelected = a < newValue * newNumber }).ToList();

            // replace
            rating.StarItems = ratingStarItems;
        }

        protected override void OnTemplateApplied(TemplateAppliedEventArgs e)
        {
            base.OnTemplateApplied(e);
            _starsPresenter = e.NameScope.Get<ItemsPresenter>("PART_StarsPresenter");
            _starsPresenter.Tapped += _starsPresenter_Tapped;
        }

        private void _starsPresenter_Tapped(object sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            if (e.Source is IStyledElement star)
            {
                if (star.DataContext is StarItem staritem)
                {
                    double newRatingValue = (staritem.N + 1d) / this.NumberOfStars;
                    // how to get to 0 rating: Click the same.
                    if (this.Value == newRatingValue)
                    {
                        this.Value = 0;
                    }
                    else
                    {
                        this.Value = newRatingValue;
                    }
                }
            }
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();
            UpdateStars(this, NumberOfStars, Value);
        }

        public class StarItem : ISelectable
        {
            public StarItem(int n)
            {
                this.N = n;
            }

            public int N { get; }

            public bool IsSelected { get; set; }
        }
    }
}
