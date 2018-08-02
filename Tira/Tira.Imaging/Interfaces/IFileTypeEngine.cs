namespace Tira.Imaging.Interfaces
{
    /// <summary>
    /// Interface for files operations engines
    /// </summary>
    public interface IFileTypeEngine
    {
        #region Methods

        /// <summary>
        /// Gets total pages number in file
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <returns></returns>
        int GetTotalPages(string filePath);

        /// <summary>
        /// Save page to jpeg file
        /// </summary>
        /// <param name="sourceFilePath">The source file path.</param>
        /// <param name="outputPath">The output path.</param>
        /// <param name="pageNumber">Page number</param>
        void SavePageToJpeg(string sourceFilePath, string outputPath, int pageNumber);

        #endregion
    }
}
