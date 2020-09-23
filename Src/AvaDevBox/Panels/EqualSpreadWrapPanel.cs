using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Layout;
using System;

namespace AvaDevBox.Panels
{
	/// <summary>
	/// This is primarily a wrap panel <see cref="WrapPanel"/> that holds an arbitrary number
	/// of childs. The difference to WrapPanel is that it spreads its childs to the whole width
	/// (if orientation is Horizontal) or height respectively. That for every line individually.
	/// This happens if <see cref="ItemWidth"/> or <see cref="ItemHeight"/> are set to double.NaN.
	/// If you set an  <see cref="ItemWidth"/> or <see cref="ItemHeight"/>, though, all items will have that
	/// height/width applied. Since items are still spread equally about the space (but in the last line),
	/// this is the right panel to use, if you want to make a gallery like listing.
	/// </summary>
	/// <note>Positions child elements in sequential position from left to right, 
	/// breaking content to the next line at the edge of the containing box. Subsequent 
	/// ordering happens sequentially from top to bottom or from right to left, 
	/// depending on the value of the <see cref="P:EqualSpreadWrapPanel.Orientation" /> 
	/// property. This panel spreads elements equally in the available space per line/column</note>
	public class EqualSpreadWrapPanel : Panel, INavigableContainer
	{
		/// <summary>
		/// simple wrapper to abstract away variables in favor of height and width properties
		/// </summary>
		private struct UVSize
		{
			internal double U;
			internal double V;
			private readonly Orientation _orientation;

			internal double Width
			{
				get
				{
					if (this._orientation != Orientation.Horizontal)
					{
						return this.V;
					}
					return this.U;
				}
				set
				{
					if (this._orientation == Orientation.Horizontal)
					{
						this.U = value;
						return;
					}
					this.V = value;
				}
			}

			internal double Height
			{
				get
				{
					if (this._orientation != Orientation.Horizontal)
					{
						return this.U;
					}
					return this.V;
				}
				private set
				{
					if (this._orientation == Orientation.Horizontal)
					{
						this.V = value;
						return;
					}
					this.U = value;
				}
			}

			internal UVSize(Orientation orientation, double width, double height)
			{
				this.U = (this.V = 0.0);
				this._orientation = orientation;
				this.Width = width;
				this.Height = height;
			}

			internal UVSize(Orientation orientation)
			{
				this.U = (this.V = 0.0);
				this._orientation = orientation;
			}
		}


		/// <summary>Identifies the <see cref="P:System.Windows.Controls.WrapPanel.ItemWidth" />  dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.WrapPanel.ItemWidth" />  dependency property.</returns>
		public static readonly StyledProperty<double> ItemWidthProperty = 
            AvaloniaProperty.Register<EqualSpreadWrapPanel,double>(nameof(ItemWidth), double.NaN, validate: IsWidthHeightValid);

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.WrapPanel.ItemHeight" />  dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.WrapPanel.ItemHeight" />  dependency property.</returns>
		public static readonly StyledProperty<double> ItemHeightProperty =
            AvaloniaProperty.Register<EqualSpreadWrapPanel, double>(nameof(ItemHeight), double.NaN, validate: IsWidthHeightValid);

        /// <summary>
        /// Defines the <see cref="Orientation"/> property.
        /// </summary>
        public static readonly StyledProperty<Orientation> OrientationProperty =
            WrapPanel.OrientationProperty.AddOwner<EqualSpreadWrapPanel>();


		/// <summary>Gets or sets a value that specifies the width of all items that are contained within a <see cref="T:System.Windows.Controls.WrapPanel" />. </summary>
		/// <returns>A <see cref="T:System.Double" /> that represents the uniform width of all items that are contained within the <see cref="T:System.Windows.Controls.WrapPanel" />. The default value is <see cref="F:System.Double.NaN" />.</returns>
		public double ItemWidth
		{
			get
			{
				return GetValue(ItemWidthProperty);
			}
			set
			{
				SetValue(ItemWidthProperty, value);
			}
		}

		/// <summary>Gets or sets a value that specifies the height of all items that are contained within a <see cref="T:System.Windows.Controls.WrapPanel" />. </summary>
		/// <returns>The <see cref="T:System.Double" /> that represents the uniform height of all items that are contained within the <see cref="T:System.Windows.Controls.WrapPanel" />. The default value is <see cref="F:System.Double.NaN" />.</returns>
		public double ItemHeight
        {
            get { return GetValue(ItemHeightProperty); }
            set { SetValue(ItemHeightProperty, value); }
        }

        /// <summary>Gets or sets a value that specifies the dimension in which child content is arranged. </summary>
		/// <returns>An <see cref="T:System.Windows.Controls.Orientation" /> value that represents the physical orientation of content within the <see cref="T:System.Windows.Controls.WrapPanel" /> as horizontal or vertical. The default value is <see cref="F:System.Windows.Controls.Orientation.Horizontal" />.</returns>
		public Orientation Orientation
        {
            get { return GetValue(OrientationProperty); }
            set { SetValue(OrientationProperty, value); }
        }

        /// <summary>Initializes a new instance of the <see cref="T:System.Windows.Controls.WrapPanel" /> class.</summary>
		public EqualSpreadWrapPanel()
		{
			AffectsMeasure<EqualSpreadWrapPanel>(OrientationProperty, ItemWidthProperty, ItemHeightProperty);
			AffectsArrange<EqualSpreadWrapPanel>(OrientationProperty, ItemWidthProperty, ItemHeightProperty);
			OrientationProperty.OverrideDefaultValue<EqualSpreadWrapPanel>(Orientation.Horizontal);
		}

		private static bool IsWidthHeightValid(double d)
		{
			double num = d;
			return double.IsNaN(num) || (num >= 0.0 && !double.IsPositiveInfinity(num));
		}

		//private static void OnOrientationChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		//{
		//	EqualSpreadWrapPanel wrapPanel = (EqualSpreadWrapPanel)d;
		//	wrapPanel.m_orientation = (Orientation)e.NewValue;
		//	wrapPanel.InvalidateArrange();
		//}

		/// <summary>Measures the child elements of a <see cref="T:System.Windows.Controls.OrientedWrapPanel" /> in anticipation of arranging them during the <see cref="M:System.Windows.Controls.OrientedWrapPanel.ArrangeOverride(System.Windows.Size)" /> pass.</summary>
		/// <returns>The <see cref="T:System.Windows.Size" /> that represents the desired size of the element.</returns>
		/// <param name="constraint">An upper limit <see cref="T:System.Windows.Size" /> that should not be exceeded.</param>
		protected override Size MeasureOverride(Size constraint)
		{
            var orientation = Orientation;
			UVSize uVSize = new UVSize(this.Orientation);
			UVSize uVSize2 = new UVSize(this.Orientation);
			UVSize uVSize3 = new UVSize(this.Orientation, constraint.Width, constraint.Height);
			double itemWidth = this.ItemWidth;
			double itemHeight = this.ItemHeight;
			bool flag = !double.IsNaN(itemWidth);
			bool flag2 = !double.IsNaN(itemHeight);
			Size availableSize = new Size(flag ? itemWidth : constraint.Width, flag2 ? itemHeight : constraint.Height);
            Avalonia.Controls.Controls internalChildren = this.Children;
			int i = 0;
			int count = internalChildren.Count;
			while (i < count)
			{
				var uIElement = internalChildren[i];
				if (uIElement != null)
				{
					uIElement.Measure(availableSize);
					UVSize uVSize4 = new UVSize(orientation, flag ? itemWidth : uIElement.DesiredSize.Width, flag2 ? itemHeight : uIElement.DesiredSize.Height);
					if (uVSize.U + uVSize4.U > uVSize3.U)
					{
						uVSize2.U = Math.Max(uVSize.U, uVSize2.U);
						uVSize2.V += uVSize.V;
						uVSize = uVSize4;
						if (uVSize4.U > uVSize3.U)
						{
							uVSize2.U = Math.Max(uVSize4.U, uVSize2.U);
							uVSize2.V += uVSize4.V;
							uVSize = new UVSize(orientation);
						}
					}
					else
					{
						uVSize.U += uVSize4.U;
						uVSize.V = Math.Max(uVSize4.V, uVSize.V);
					}
				}
				i++;
			}
			uVSize2.U = Math.Max(uVSize.U, uVSize2.U);
			uVSize2.V += uVSize.V;
			return new Size(uVSize2.Width, uVSize2.Height);
		}
		/// <summary>Arranges the content of a <see cref="T:EqualSpreadWrapPanel" /> element.</summary>
		/// <returns>The <see cref="T:System.Windows.Size" /> that represents the arranged size of this <see cref="T:EqualSpreadWrapPanel" /> element and its children.</returns>
		/// <param name="finalSize">The <see cref="T:System.Windows.Size" /> that this element should use to arrange its child elements.</param>
		protected override Size ArrangeOverride(Size finalSize)
		{
			int num = 0;
            var orientation = Orientation;
			double itemWidth = this.ItemWidth;
			double itemHeight = this.ItemHeight;
			double num2 = 0.0;
			double itemU = (orientation == Orientation.Horizontal) ? itemWidth : itemHeight;
			UVSize uVSize = new UVSize(orientation);
			UVSize uVSizeFinal = new UVSize(orientation, finalSize.Width, finalSize.Height);
			bool flagW = !double.IsNaN(itemWidth);
			bool flagH = !double.IsNaN(itemHeight);
			bool useItemU = (orientation == Orientation.Horizontal) ? flagW : flagH;
			var internalChildren = this.Children;
			int i = 0;
			int count = internalChildren.Count;
			while (i < count)
			{
				IControl uIElement = internalChildren[i];
				if (uIElement != null)
				{
					UVSize uVSize3 = new UVSize(orientation, flagW ? itemWidth : uIElement.DesiredSize.Width, flagH ? itemHeight : uIElement.DesiredSize.Height);
					if (uVSize.U + uVSize3.U > uVSizeFinal.U)
					{
						this.ArrangeLine(num2, uVSize.V, num, i, useItemU, itemU, uVSizeFinal);
						num2 += uVSize.V;
						uVSize = uVSize3;
						if (uVSize3.U > uVSizeFinal.U)
						{
							this.ArrangeLine(num2, uVSize3.V, i, ++i, useItemU, itemU, uVSizeFinal);
							num2 += uVSize3.V;
							uVSize = new UVSize(orientation);
						}
						num = i;
					}
					else
					{
						uVSize.U += uVSize3.U;
						uVSize.V = Math.Max(uVSize3.V, uVSize.V);
					}
				}
				i++;
			}
			if (num < internalChildren.Count)
			{
				this.ArrangeLine(num2, uVSize.V, num, internalChildren.Count, useItemU, itemU, uVSizeFinal);
			}
			return finalSize;
		}

		private void ArrangeLine(double v, double lineV, int start, int end, bool useItemU, double itemU, UVSize lineExtent)
		{
			var internalChildren = this.Children;
			double itemUspacing;

			// count space and determine leftover if DesiredSize is to be used
			if (useItemU)
			{
				itemUspacing = lineExtent.U / ((int)(lineExtent.U / itemU));
			}
			else
			{
				double necessaryLineSpace = 0;
				for (int i = start; i < end; i++)
				{
					IControl uIElement = internalChildren[i];
					if (uIElement != null)
					{
						var uVSize = new UVSize(this.Orientation, uIElement.DesiredSize.Width, uIElement.DesiredSize.Height);
						necessaryLineSpace += uVSize.U;
					}
				}
				itemUspacing = (lineExtent.U - necessaryLineSpace) / (end - start);
			}

			// start value - center items
			double num = useItemU ? (itemUspacing - itemU) / 2 : itemUspacing / 2;

			// arrange items in line
			for (int i = start; i < end; i++)
			{
				IControl uIElement = internalChildren[i];
				if (uIElement != null)
				{
					var uVSize = new UVSize(this.Orientation, uIElement.DesiredSize.Width, uIElement.DesiredSize.Height);
					double incrementU = useItemU ? itemUspacing : uVSize.U + itemUspacing;
					double sizeU = useItemU ? itemU : uVSize.U;
					if (this.Orientation == Orientation.Horizontal)
					{
						uIElement.Arrange(new Rect(num, v, sizeU, lineV));
					}
					else
					{
						uIElement.Arrange(new Rect(v, num, lineV, sizeU));
					}
					num += incrementU;
				}
			}
		}
		
		/// <inheritdoc cref="INavigableContainer.GetControl"/>
        public IInputElement GetControl(NavigationDirection direction, IInputElement @from, bool wrap)
        {
            var orientation = Orientation;
            var children = Children;
            bool horiz = orientation == Orientation.Horizontal;
            int index = Children.IndexOf((IControl)from);

            switch (direction)
            {
                case NavigationDirection.First:
                    index = 0;
                    break;
                case NavigationDirection.Last:
                    index = children.Count - 1;
                    break;
                case NavigationDirection.Next:
                    ++index;
                    break;
                case NavigationDirection.Previous:
                    --index;
                    break;
                case NavigationDirection.Left:
                    index = horiz ? index - 1 : -1;
                    break;
                case NavigationDirection.Right:
                    index = horiz ? index + 1 : -1;
                    break;
                case NavigationDirection.Up:
                    index = horiz ? -1 : index - 1;
                    break;
                case NavigationDirection.Down:
                    index = horiz ? -1 : index + 1;
                    break;
            }

            if (index >= 0 && index < children.Count)
            {
                return children[index];
            }
            else
            {
                return null;
            }
        }
	}
}