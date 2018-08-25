using Ak.Framework.Wpf.Culture;

namespace Tira.App.Logic.Helpers
{
    /// <summary>
    /// Class for localizing resources
    /// </summary>
    public class CultureResources : CultureResourcesBase
    {
        #region Public methods

        /// <summary>
        /// Gets resources
        /// </summary>
        /// <returns>Resources</returns>
        public Properties.Resources GetResourceInstance()
        {
            return new Properties.Resources();
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CultureResources"/> class.
        /// </summary>
        public CultureResources() : base("Resources", typeof(Properties.Resources)) { }

        #endregion
    }
}
