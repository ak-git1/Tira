using System.Windows;
using System.Windows.Input;

namespace Tira.App.Windows.Filters
{
    /// <summary>
    /// Interaction logic for RotationFilterWindow.xaml
    /// </summary>
    public partial class RotationFilterWindow : Window
    {
        public RotationFilterWindow()
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
