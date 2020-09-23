using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Markup.Xaml;
using Avalonia.Media;

namespace AvaloniaControls.Demo
{
    public class PanelDemo : UserControl
    {
        public PanelDemo()
        {
            this.InitializeComponent();
        }
        
        private Panel panel;

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
            panel = this.Find<Panel>("panel");
            for (int i = 0; i < 27; i++)
            {
                panel.Children.Add(new Button() { Content = "bla"+i });
                panel.Children.Add(new Ellipse() { Fill = Brushes.Aquamarine, Width = 30, Height = 40 });
            }
        }

        
    }
}
