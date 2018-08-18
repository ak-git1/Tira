using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using Tira.Imaging.Helpers;
using Tira.Logic.Models.Drawing;
using Tira.OCR;
using Tira.OCR.Enums;

namespace Tira.Logic.Models
{
    /// <summary>
    /// Gallery image
    /// </summary>
    [Serializable]
    public class GalleryImage
    {
        #region Properties

        /// <summary>
        /// Uid
        /// </summary>
        [XmlElement]
        public Guid Uid { get; set; }

        /// <summary>
        /// Order number of the image
        /// </summary>
        [XmlElement]
        public int OrderNumber { get; set; }

        /// <summary>
        /// Displayed name
        /// </summary>
        [XmlElement]
        public string DisplayedName { get; set; }

        /// <summary>
        /// The image folder path.
        /// </summary>
        public string ImageFolderPath { get; set; }

        /// <summary>
        /// Thumbnail file name
        /// </summary>
        [XmlElement]
        public string ThumbnailFileName { get; set; }

        /// <summary>
        /// Thumbnail file path
        /// </summary>
        [XmlIgnore]
        public string ThumbnailFilePath => Path.Combine(ImageFolderPath, ThumbnailFileName);

        /// <summary>
        /// Image file name
        /// </summary>
        [XmlElement]
        public string ImageFileName { get; set; }

        /// <summary>
        /// Image file path
        /// </summary>
        [XmlIgnore]
        public string ImageFilePath => Path.Combine(ImageFolderPath, ImageFileName);

        /// <summary>
        /// Drawing objects on the image for OCR 
        /// </summary>
        [XmlIgnore]
        public DrawingObjects DrawingObjects { get; set; }

        /// <summary>
        /// Recognition completed
        /// </summary>
        [XmlIgnore]
        public bool RecognitionCompleted { get; set; }

        /// <summary>
        /// Recognized data
        /// </summary>
        [XmlIgnore]
        public DataTable RecognizedData { get; set; }

        #endregion

        #region Public methods

        /// <summary>
        /// Performs OCR recognition
        /// </summary>
        /// <param name="dataColumns">Data columns.</param>
        public void PerformOcr(List<string> dataColumns)
        {
            RecognitionCompleted = false;
            TesseractEngine tesserartEngine = new TesseractEngine(RecognitionLanguage.Russian, RecognitionLanguage.English);

            using (Bitmap bitmap = new Bitmap(Image.FromFile(ImageFilePath)))
            {
                RecognizedData = new DataTable("RecognizedData");
                foreach (string dataColumn in dataColumns)
                    RecognizedData.Columns.Add(dataColumn);

                Rectangle[,] areas = GetRecognitionAreas();
                for (int j = 0; j < areas.GetLength(1); j++)
                {
                    DataRow row = RecognizedData.NewRow();
                    for (int i = 0; i < areas.GetLength(0); i++)
                    {
                        Rectangle area = areas[i, j];
                        using (Bitmap croppedBitmap = BitmapHelper.Crop(bitmap, area))
                        {
                            row[i] = tesserartEngine.Process(croppedBitmap);
                        }
                    }
                    RecognizedData.Rows.Add(row);
                }                
            }

            RecognitionCompleted = true;
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Gets the recognition areas.
        /// </summary>
        /// <returns></returns>
        private Rectangle[,] GetRecognitionAreas()
        {
            List<int> verticalLinesCoordinates = DrawingObjects.VerticalLinesCoordinates.OrderBy(x => x).ToList();
            List<int> horizontalLinesCoordinates = DrawingObjects.HorizontalLinesCoordinates.OrderBy(x => x).ToList();
            int columnsNumber = verticalLinesCoordinates.Count + 1;
            int rowsNumber = horizontalLinesCoordinates.Count + 1;
            Rectangle[,] areas = new Rectangle[columnsNumber, rowsNumber];

            for (int i = 0; i < rowsNumber; i++)
            {
                int y = i == 0 ? DrawingObjects.RectangleArea.Y : horizontalLinesCoordinates[i - 1];
                int h = 0;
                if (rowsNumber == 1)
                    h = DrawingObjects.RectangleArea.Width;
                else if (i == rowsNumber - 1)
                    h = DrawingObjects.RectangleArea.Bottom - y;
                else
                    h = horizontalLinesCoordinates[i] - y;

                for (int j = 0; j < columnsNumber; j++)
                {
                    int x = j == 0 ? DrawingObjects.RectangleArea.X : verticalLinesCoordinates[j - 1];
                    int w = 0;
                    if (columnsNumber == 1)
                        w = DrawingObjects.RectangleArea.Width;
                    else if (j == columnsNumber - 1)
                        w = DrawingObjects.RectangleArea.Right - x;
                    else
                        w = verticalLinesCoordinates[j] - x;

                    areas[j, i] = new Rectangle(x, y, w, h);
                }
            }
        
            return areas;
        }

        #endregion
    }
}
