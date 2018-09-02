using System.Windows;
using System.Windows.Input;

namespace Tira.App.Windows.Filters
{
    /// <summary>
    /// Interaction logic for ContrastFilterWindow.xaml
    /// </summary>
    public partial class ContrastFilterWindow : Window
    {
        public ContrastFilterWindow()
        {
            InitializeComponent();
        }

        private void Window_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            OnMouseLeftButtonDown(e);
            DragMove();
        }
    }
}
