using Tira.Logic.Enums;

namespace Tira.Logic.Models
{
    /// <summary>
    /// Result of the action performed
    /// </summary>
    internal class ActionResult
    {
        #region Properties

        public ActionResultType Result { get; }

        public string Message { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ActionResult" /> class.
        /// </summary>
        /// <param name="result">Result type</param>
        /// <param name="message">Message.</param>
        public ActionResult(ActionResultType result, string message)
        {
            Result = result;
            Message = message;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ActionResult"/> class.
        /// </summary>
        public ActionResult()
        {
            Result = ActionResultType.Ok;
            Message = string.Empty;
        }

        #endregion
    }
}
