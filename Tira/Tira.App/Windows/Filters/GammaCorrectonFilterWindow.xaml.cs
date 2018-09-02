using System.Windows;
using System.Windows.Input;

namespace Tira.App.Windows.Filters
{
    /// <summary>
    /// Interaction logic for GammaCorrectonFilterWindow.xaml
    /// </summary>
    public partial class GammaCorrectonFilterWindow : Window
    {
        public GammaCorrectonFilterWindow()
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
