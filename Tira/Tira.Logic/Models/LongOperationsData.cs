using System;

namespace Tira.Logic.Models
{
    /// <summary>
    /// Data container for long running operations
    /// (can be used in progressbar scenarios)
    /// </summary>
    [Serializable]
    public class LongOperationsData
    {
        #region Variables

        /// <summary>
        /// Current iteration message template
        /// </summary>
        private readonly string _currentIterationMessageTemplate;

        #endregion

        #region Properties

        /// <summary>
        /// Current iteration number
        /// </summary>
        public int CurrentIteration { get; set; }

        /// <summary>
        /// Maximum ierations quantity
        /// </summary>
        public int IterationsQuantity { get; }

        /// <summary>
        /// Current iteration message
        /// </summary>
        public string CurrentIterationMessage => string.Format(_currentIterationMessageTemplate, CurrentIteration, IterationsQuantity);

        /// <summary>
        /// Description
        /// </summary>
        public string Description { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="LongOperationsData"/> class.
        /// </summary>
        /// <param name="currentIteration">Current iteration number</param>
        /// <param name="iterationsQuantity">Maximum ierations quantity</param>        
        /// <param name="currentIterationMessageTemplate">Current iteration message template</param>
        /// <param name="description">Description</param>
        public LongOperationsData(int currentIteration, int iterationsQuantity, string currentIterationMessageTemplate, string description)
        {
            CurrentIteration = currentIteration;
            IterationsQuantity = iterationsQuantity;
            _currentIterationMessageTemplate = currentIterationMessageTemplate;
            Description = description;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Iterates
        /// </summary>
        public void Iterate()
        {
            if (CurrentIteration < IterationsQuantity)
                CurrentIteration++;
        }

        #endregion
    }
}
