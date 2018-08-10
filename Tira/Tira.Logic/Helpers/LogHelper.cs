using NLog;

namespace Tira.Logic.Helpers
{
    /// <summary>
    /// Class for logging
    /// </summary>
    public static class LogHelper
    {
        #region Properties

        /// <summary>
        /// Logger
        /// </summary>
        public static Logger Logger => LogManager.GetCurrentClassLogger();

        #endregion

        #region Public methods

        /// <summary>
        /// Appying changes, clearing logger
        /// </summary>
        public static void Flush()
        {
            LogManager.Flush();
        }

        #endregion
    }
}
