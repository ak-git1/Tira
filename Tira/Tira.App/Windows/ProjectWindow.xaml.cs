using Tira.App.Logic.ViewModels;

namespace Tira.App.Windows
{
    /// <summary>
    /// Interaction logic for ProjectWindow.xaml
    /// </summary>
    public partial class ProjectWindow
    {
        public ProjectWindow(ProjectViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }

        public ProjectWindow()
        {
        }
    }
}
