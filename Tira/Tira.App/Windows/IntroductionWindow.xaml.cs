using Tira.App.Logic.ViewModels;

namespace Tira.App.Windows
{
    /// <summary>
    /// Interaction logic for IntroductionWindow.xaml
    /// </summary>
    public partial class IntroductionWindow
    {
        public IntroductionWindow(IntroductionViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
            Show();
        }

        public IntroductionWindow()
        {            
        }
    }
}
