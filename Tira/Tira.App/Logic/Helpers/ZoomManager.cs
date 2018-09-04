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
        public double Scale { get; set; }

        /// <summary>
        /// Fit mode
        /// </summary>
        public FitOption FitMode { get; set; }

        /// <summary>
        /// Zoom in step
        /// </summary>
        public double ZoomInStep { get; set; }

        /// <summary>
        /// Zoom out step
        /// </summary>
        public double ZoomOutStep { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ZoomManager"/> class.
        /// </summary>
        public ZoomManager()
        {
            Scale = 1;
            FitMode = FitOption.FitSize;
            ZoomInStep = 1.2;
            ZoomOutStep = 0.8;
        }

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
