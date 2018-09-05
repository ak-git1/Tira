using System;
using Ak.Framework.Core.Extensions;
using Ak.Framework.Wpf.ViewModels;
using Tira.App.Logic.Enums;
using Tira.Logic.Enums;
using Tira.Logic.Models;

namespace Tira.App.Logic.ViewModels
{
    /// <summary>
    /// ViewModel for project template
    /// </summary>
    /// <seealso cref="Ak.Framework.Wpf.ViewModels.ViewModelBase" />
    public class ProjectTemplateViewModel : ViewModelBase
    {
        #region Variables

        /// <summary>
        /// Template name
        /// </summary>
        private string _name;

        #endregion

        #region Properties

        /// <summary>
        /// Template name
        /// </summary>
        public string Name
        {
            get => _name;
            set
            {
                SetProperty(ref _name, value);
                OnPropertyChanged(() => Name);
                OnPropertyChanged(() => ValidationResult);
            }
        }

        /// <summary>
        /// Window mode
        /// </summary>
        public WindowMode WindowMode { get; }

        /// <summary>
        /// Window title
        /// </summary>
        public string WindowTitle
        {
            get
            {
                switch (WindowMode)
                {
                    case WindowMode.Create:
                        return Properties.Resources.ProjectTemplateWindow_Title_CreateTemplate;

                    case WindowMode.Edit:
                        return Properties.Resources.ProjectTemplateWindow_Title_EditTemplate;

                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        /// <summary>
        /// Fields validation result
        /// </summary>
        public ActionResult ValidationResult
        {
            get
            {
                if (Name.IsNullOrEmpty())
                    return new ActionResult(ActionResultType.Error, Properties.Resources.ProjectTemplateWindow_Error_Empty);

                if (ProjectTemplate.Exists(Name))
                    return new ActionResult(ActionResultType.Error, Properties.Resources.ProjectTemplateWindow_Error_Exists);

                return new ActionResult();
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ProjectTemplateViewModel"/> class.
        /// </summary>
        /// <param name="windowMode">Window mode</param>
        public ProjectTemplateViewModel(WindowMode windowMode)
        {
            WindowMode = windowMode;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProjectTemplateViewModel" /> class.
        /// </summary>
        /// <param name="projectTemplate">Project template</param>
        public ProjectTemplateViewModel(ProjectTemplate projectTemplate)
        {
            WindowMode = WindowMode.Edit;
            Name = projectTemplate.Name;
        }

        #endregion
    }
}