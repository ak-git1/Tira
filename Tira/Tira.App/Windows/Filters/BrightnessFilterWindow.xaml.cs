using System.Windows;
using System.Windows.Input;

namespace Tira.App.Windows.Filters
{
    /// <summary>
    /// Interaction logic for BrightnessFilterWindow.xaml
    /// </summary>
    public partial class BrightnessFilterWindow : Window
    {
        public BrightnessFilterWindow()
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
