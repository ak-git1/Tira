using Tira.App.Logic.Enums;

namespace Tira.App.Logic.Helpers
{
    /// <summary>
    /// Manager of zooming values
    /// </summary>
    public class ZoomManager
    {
        #region Properties

        /// <summary>
        /// Scale value
        /// </summary>
        public double Scale { get; set; } = 1;

        /// <summary>
        /// Fit mode
        /// </summary>
        public FitOption FitMode { get; set; } = FitOption.FitSize;

        /// <summary>
        /// Zoom in step
        /// </summary>
        public double ZoomInStep { get; set; } = 1.2;

        /// <summary>
        /// Zoom out step
        /// </summary>
        public double ZoomOutStep { get; set; } = 0.8;

        #endregion

        #region Public methods

        /// <summary>
        /// Zooms in
        /// </summary>
        public void ZoomIn()
        {
            Scale *= ZoomInStep;
        }

        /// <summary>
        /// Zooms out
        /// </summary>
        public void ZoomOut()
        {
            Scale *= ZoomOutStep;
        }

        #endregion
    }
}
