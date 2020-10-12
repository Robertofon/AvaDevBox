using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;

namespace AvaloniaControls.Demo
{
    public class MenuButtonDemo : UserControl
    {
        private ListBox _listbox;
        private ObservableCollection<string> _items;

        public MenuButtonDemo()
        {
            this.InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
            _listbox = this.Find<ListBox>("listbox");
            _listbox.Items = _items = new ObservableCollection<string>();
        }


        protected void ClickEvent(object sender, RoutedEventArgs e)
        {
            _items.Add("MainButton");
        }

        protected void ClickTest1(object sender, RoutedEventArgs e)
        {
            _items.Add("Test1");
            e.Handled = true;
        }

        protected void ClickTest2(object sender, RoutedEventArgs e)
        {
            _items.Add("Test2");
            e.Handled = true;
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
