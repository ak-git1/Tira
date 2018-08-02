using GdPicture14;
using Tira.Imaging.Interfaces;

namespace Tira.Imaging.Engines
{
    /// <summary>
    /// Engine for operations with tiff files
    /// </summary>
    /// <seealso cref="IFileTypeEngine" />
    public class PdfEngine : IFileTypeEngine
    {
        #region Public methods

        /// <summary>
        /// Gets total pages number in file
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <returns></returns>
        public int GetTotalPages(string filePath)
        {
            int totalPages = 0;

            using (GdPicturePDF pdf = new GdPicturePDF())
            {
                pdf.LoadFromFile(filePath, false);
                totalPages = pdf.GetPageCount();
            }

            return totalPages;
        }

        /// <summary>
        /// Save page to jpeg file with 200 dpi
        /// </summary>
        /// <param name="sourceFilePath">The source file path.</param>
        /// <param name="outputPath">The output path.</param>
        /// <param name="pageNumber">Page number</param>
        public void SavePageToJpeg(string sourceFilePath, string outputPath, int pageNumber)
        {
            SavePageToJpeg(sourceFilePath, outputPath, pageNumber, 200);
        }

        /// <summary>
        /// Save page to jpeg file
        /// </summary>
        /// <param name="sourceFilePath">The source file path.</param>
        /// <param name="outputPath">The output path.</param>
        /// <param name="pageNumber">Page number</param>
        /// <param name="dpi">Dpi of the generated file</param>
        public void SavePageToJpeg(string sourceFilePath, string outputPath, int pageNumber, int dpi)
        {
            using (GdPicturePDF pdf = new GdPicturePDF())
            {
                pdf.LoadFromFile(sourceFilePath, false);
                pdf.SelectPage(pageNumber);
                int imageId = pdf.RenderPageToGdPictureImage(200, true);
                using (GdPictureImaging image = new GdPictureImaging())
                {
                    image.SaveAsJPEG(imageId, outputPath);
                    image.ReleaseGdPictureImage(imageId);
                }
            }
        }

        #endregion
    }
}
