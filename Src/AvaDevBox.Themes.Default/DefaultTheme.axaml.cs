using Avalonia;
using Avalonia.Markup.Xaml;
using Avalonia.Styling;

namespace AvaDevBox.Themes.Default
{
    /// <summary>
    /// The default AvaDevBox theme.
    /// </summary>
    public class DefaultTheme : Styles
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultTheme"/> class.
        /// </summary>
        public DefaultTheme()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
