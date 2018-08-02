using System.IO;
using Tira.Imaging.Enums;

namespace Tira.Imaging.Helpers
{
    /// <summary>
    /// Helper for operations with files
    /// </summary>
    public static class FilesHelper
    {
        #region Public methods

        /// <summary>
        /// Gets the type of the file.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <returns></returns>
        public static FileType GetFileType(string fileName)
        {
            string fileExtension = Path.GetExtension(fileName).ToLower();

            switch (fileExtension)
            {
                case ".jpg":
                case ".jpeg":
                    return FileType.Jpeg;

                case ".png":
                    return FileType.Png;

                case ".gif":
                    return FileType.Gif;

                case ".bmp":
                    return FileType.Bmp;

                case ".pdf":
                    return FileType.Pdf;

                case ".tif":
                case ".tiff":
                    return FileType.Tiff;

                default:
                    return FileType.Unsupported;
            }
        }

        /// <summary>
        /// Gets the file format.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <returns></returns>
        public static FileFormat GetFileFormat(string fileName)
        {
            return GetFileFormat(GetFileType(fileName));
        }

        /// <summary>
        /// Gets the file format.
        /// </summary>
        /// <param name="fileType">Type of the file.</param>
        /// <returns></returns>
        public static FileFormat GetFileFormat(FileType fileType)
        {
            switch (fileType)
            {
                case FileType.Jpeg:
                case FileType.Png:
                case FileType.Bmp:
                case FileType.Gif:
                    return FileFormat.SingePage;

                case FileType.Pdf:
                case FileType.Tiff:
                    return FileFormat.MultiPage;

                default:
                    return FileFormat.Unknown;
            }
        }

        #endregion
    }
}
