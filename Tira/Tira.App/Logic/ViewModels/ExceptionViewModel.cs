using System;

namespace Tira.App.Logic.ViewModels
{
    /// <summary>
    /// ViewModel for exception
    /// </summary>
    public sealed class ExceptionViewModel
    {
        #region Properties

        /// <summary>
        /// Title
        /// </summary>
        public string Title { get; }

        /// <summary>
        /// Message
        /// </summary>
        public string Message { get; }

        /// <summary>
        /// Exception
        /// </summary>
        public Exception Exception { get; }

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="title">Title</param>
        /// <param name="message">Message</param>
        /// <param name="exception">Exception</param>
        public ExceptionViewModel(string title, string message, Exception exception)
        {
            Title = title;
            Message = message;
            Exception = exception;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public ExceptionViewModel()
        {            
        }

        #endregion
    }
}