using System.Windows;
using System.Windows.Input;

namespace Tira.App.Windows.Filters
{
    /// <summary>
    /// Interaction logic for PunchHolesRemovalFilterWindow.xaml
    /// </summary>
    public partial class PunchHolesRemovalFilterWindow : Window
    {
        public PunchHolesRemovalFilterWindow()
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
