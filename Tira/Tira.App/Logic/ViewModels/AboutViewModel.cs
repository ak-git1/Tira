using Ak.Framework.Core.Helpers;
using Tira.App.Properties;

namespace Tira.App.Logic.ViewModels
{
    /// <summary>
    /// ViewModel for introduction window
    /// </summary>
    public class AboutViewModel : TiraViewModelBase
    {
        #region Properties

        /// <summary>
        /// Application version
        /// </summary>
        public string Version => string.Format(Resources.IntroductionWindow_Label_Version, AssemblyInfoHelper.GetMainAssemblyVersion());

        #endregion
    }
}
