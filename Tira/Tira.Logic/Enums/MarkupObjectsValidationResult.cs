namespace Tira.Logic.Enums
{
    /// <summary>
    /// Validation results for drawing objects
    /// </summary>
    public enum MarkupObjectsValidationResult
    {
        /// <summary>
        /// Ok
        /// </summary>
        Ok = 0,

        /// <summary>
        /// The rectangle not set
        /// </summary>
        RectangleNotSet = 1,

        /// <summary>
        /// Wrong number of vertical lines
        /// </summary>
        WrongNumberOfVerticalLines = 2
    }
}
