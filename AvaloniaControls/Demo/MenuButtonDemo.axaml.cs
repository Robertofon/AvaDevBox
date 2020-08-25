using System.Windows.Input;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;

namespace AvaloniaControls.Demo
{
    public class MenuButtonDemo : UserControl
    {
        public MenuButtonDemo()
        {
            this.InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }


        private void ClickEvent(object sender, RoutedEventArgs e)
        {

        }

        public class MenuDemoVm
        {
            public MenuDemoVm()
            {
               // Command1 =  ReactiveCommand.Create(Save);
            }

            public ICommand Command1 { get; set; }
        }
    }
}
