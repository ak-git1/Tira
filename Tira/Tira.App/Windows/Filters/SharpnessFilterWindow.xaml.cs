using System.Windows;
using System.Windows.Input;

namespace Tira.App.Windows.Filters
{
    /// <summary>
    /// Interaction logic for SharpnessFilterWindow.xaml
    /// </summary>
    public partial class SharpnessFilterWindow : Window
    {
        public SharpnessFilterWindow()
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
