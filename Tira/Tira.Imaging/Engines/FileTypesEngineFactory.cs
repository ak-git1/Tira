using System;
using Tira.Imaging.Enums;
using Tira.Imaging.Helpers;
using Tira.Imaging.Interfaces;

namespace Tira.Imaging.Engines
{
    /// <summary>
    /// Factory for file types engine
    /// </summary>
    public static class FileTypesEngineFactory
    {
        #region Public methods

        /// <summary>
        /// Gets file types engine.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <returns></returns>
        /// <exception cref="Exception">Unsupported format</exception>
        public static IFileTypeEngine GetFileTypesEngine(string filePath)
        {
            IFileTypeEngine engine = null;

            FileType fileType = FilesHelper.GetFileType(filePath);
            switch (fileType)
            {
                case FileType.Bmp:
                case FileType.Gif:
                case FileType.Jpeg:
                case FileType.Png:
                    engine = new SinglePageImageEngine();
                    break;

                case FileType.Tiff:
                    engine = new TiffEngine();
                    break;

                case FileType.Pdf:
                    engine = new PdfEngine();
                    break;

                case FileType.Unsupported:
                    throw new Exception("Unsupported format");
            }

            return engine;
        }

        #endregion
    }
}
