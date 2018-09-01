using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using Ak.Framework.Core.Extensions;
using Ak.Framework.Core.Utils;
using Tira.Imaging.Classes;
using Tira.Imaging.Converters;
using Tira.Imaging.Enums;
using Tira.Imaging.Helpers;
using Tira.Logic.Enums;
using Tira.Logic.Helpers;
using Tira.Logic.Models.Markup;
using Tira.Logic.Settings;
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
        /// Original image file name
        /// </summary>
        [XmlElement]
        public string OriginalFileName { get; set; }

        /// <summary>
        /// Original image file path
        /// </summary>
        [XmlIgnore]
        public string OriginalFilePath => Path.Combine(ImageFolderPath, OriginalFileName);

        /// <summary>
        /// Actual image file path
        /// </summary>
        [XmlIgnore]
        public string ActualImageFilePath => TempFilePath.NotEmpty() ? TempFilePath : ImageFilePath;

        /// <summary>
        /// Actual image thumbnail file path
        /// </summary>
        [XmlIgnore]
        public string ActualThumbnailFilePath => TempThumbnailFilePath.NotEmpty() ? TempThumbnailFilePath : ThumbnailFilePath;

        /// <summary>
        /// Temporary file path
        /// </summary>
        [XmlIgnore]
        public string TempFilePath { get; set; }

        /// <summary>
        /// Thumbnail temporary file path
        /// </summary>
        [XmlIgnore]
        public string TempThumbnailFilePath { get; set; }

        /// <summary>
        /// Applied filters to image
        /// </summary>
        [XmlElement]
        public List<Filter> AppliedFilters { get; set; }

        /// <summary>
        /// Drawing objects on the image for OCR 
        /// </summary>
        [XmlIgnore]
        public MarkupObjects MarkupObjects { get; set; }

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

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="GalleryImage"/> class.
        /// </summary>
        public GalleryImage()
        {
            MarkupObjects = new MarkupObjects();
            AppliedFilters = new List<Filter>();
            OriginalFileName = string.Empty;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Performs OCR recognition
        /// </summary>
        /// <param name="dataColumns">Data columns.</param>
        public void PerformOcr(List<DataColumn> dataColumns)
        {
            RecognitionCompleted = false;
            TesseractEngine tesserartEngine = new TesseractEngine(RecognitionLanguage.Russian, RecognitionLanguage.English);

            using (Bitmap bitmap = new Bitmap(Image.FromFile(ImageFilePath)))
            {
                RecognizedData = new DataTable("RecognizedData");
                foreach (DataColumn dataColumn in dataColumns)
                    RecognizedData.Columns.Add(dataColumn.Name);

                Rectangle[,] areas = GetRecognitionAreas();
                for (int j = 0; j < areas.GetLength(1); j++)
                {
                    DataRow row = RecognizedData.NewRow();
                    for (int i = 0; i < areas.GetLength(0); i++)
                    {
                        Rectangle area = areas[i, j];
                        using (Bitmap croppedBitmap = BitmapHelper.Crop(bitmap, area))
                        {
                            DataColumn dataColumn = dataColumns[i]; // TODO добавить проверки dataColumn
                            row[i] = tesserartEngine.Process(croppedBitmap);
                        }                        
                    }
                    RecognizedData.Rows.Add(row);
                }                
            }

            RecognitionCompleted = true;
        }

        /// <summary>
        /// Save gallery image
        /// </summary>
        public void Save()
        {
            if (AppliedFilters.Count == 0)
            {
                if (File.Exists(OriginalFilePath))
                    File.Delete(OriginalFilePath);
                OriginalFileName = string.Empty;
            }

            if (TempFilePath.NotEmpty() && File.Exists(TempFilePath))
            {
                File.Copy(TempFilePath, ImageFilePath, true);
                File.Delete(TempFilePath);
                TempFilePath = string.Empty;
            }

            if (TempThumbnailFilePath.NotEmpty() && File.Exists(TempThumbnailFilePath))
            {
                File.Copy(TempThumbnailFilePath, ThumbnailFilePath, true);
                File.Delete(TempThumbnailFilePath);
                TempThumbnailFilePath = string.Empty;
            }
        }

        /// <summary>
        /// Applies filters
        /// </summary>
        /// <param name="filter">Filter</param>
        public void ApplyFilter(Filter filter)
        {
            ApplyFilters(new List<Filter>{ filter });
        }

        /// <summary>
        /// Applies filters
        /// </summary>
        /// <param name="filters">Filters</param>
        public void ApplyFilters(List<Filter> filters)
        {
            try
            {
                // Saving original image
                if (AppliedFilters.Count == 0)
                {
                    OriginalFileName = GenerateOriginalFileName(ImageFileName);
                    File.Copy(ImageFilePath, OriginalFilePath, true);
                }

                // Creating temp image
                if (TempFilePath.IsNullOrEmpty())
                {
                    TempFilePath = Path.Combine(ImageFolderPath, GenerateTempFileName(ImageFileName));
                    File.Copy(ImageFilePath, TempFilePath, true);
                }

                foreach (Filter filter in filters)
                {
                    try
                    {
                        // Applying filter
                        Bitmap original = new Bitmap(TempFilePath);
                        Bitmap bitmap = new Bitmap(original);
                        original.Dispose();
                        File.Delete(TempFilePath);
                        ApplyFilter(bitmap, filter).Save(TempFilePath, ImageFormat.Jpeg);
                        bitmap.Dispose();
                        GarbageCollector.Collect();

                        AppliedFilters.Add(filter);                    
                    }
                    catch (Exception e)
                    {
                        LogHelper.Logger.Error(e, $"Unable to apply filter to image. Filter name: {filter.FilterType}");
                    }
                }

                // Create thumbnail for filtered image
                if (TempThumbnailFilePath.IsNullOrEmpty())
                    TempThumbnailFilePath = Path.Combine(ImageFolderPath, GenerateTempThumbnailFileName(ImageFileName));
                else
                    if (File.Exists(TempThumbnailFilePath))
                        File.Delete(TempThumbnailFilePath);
                new ImagesConverter(TempFilePath).CreateThumbnail(TempThumbnailFilePath, CommonSettings.ThumbnailWidth, CommonSettings.ThumbnailHeight);
            }
            catch (Exception e)
            {
                LogHelper.Logger.Error(e, "Unable to apply filters to image.");
            }

            GarbageCollector.Collect();
        }

        /// <summary>
        /// Rolls back to original image
        /// </summary>
        public void RollBackToOriginal()
        {
            try
            {
                if (OriginalFilePath.NotEmpty() && File.Exists(OriginalFilePath))
                {
                    File.Copy(OriginalFilePath, ImageFilePath, true);
                    ClearServiceImages();
                    AppliedFilters = new List<Filter>();
                }
            }
            catch (Exception e)
            {
                LogHelper.Logger.Error(e, "Unable to roll back image to original state.");
            }            
        }

        /// <summary>
        /// Rolls image filters one step back.
        /// </summary>
        public void RollBackFiltersOneStepBack()
        {
            try
            {
                if (AppliedFilters.Any())
                {
                    AppliedFilters.RemoveAt(AppliedFilters.Count - 1);
                    ClearServiceImages();
                    ApplyFilters(AppliedFilters);
                }
            }
            catch (Exception e)
            {
                LogHelper.Logger.Error(e, "Unable to roll image filters one step back.");
            }
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Gets the recognition areas.
        /// </summary>
        /// <returns></returns>
        private Rectangle[,] GetRecognitionAreas()
        {
            List<int> verticalLinesCoordinates = MarkupObjects.VerticalLinesCoordinates.OrderBy(x => x).ToList();
            List<int> horizontalLinesCoordinates = MarkupObjects.HorizontalLinesCoordinates.OrderBy(x => x).ToList();
            int columnsNumber = verticalLinesCoordinates.Count + 1;
            int rowsNumber = horizontalLinesCoordinates.Count + 1;
            Rectangle[,] areas = new Rectangle[columnsNumber, rowsNumber];

            for (int i = 0; i < rowsNumber; i++)
            {
                int y = i == 0 ? MarkupObjects.RectangleArea.Y : horizontalLinesCoordinates[i - 1];
                int h = 0;
                if (rowsNumber == 1)
                    h = MarkupObjects.RectangleArea.Width;
                else if (i == rowsNumber - 1)
                    h = MarkupObjects.RectangleArea.Bottom - y;
                else
                    h = horizontalLinesCoordinates[i] - y;

                for (int j = 0; j < columnsNumber; j++)
                {
                    int x = j == 0 ? MarkupObjects.RectangleArea.X : verticalLinesCoordinates[j - 1];
                    int w = 0;
                    if (columnsNumber == 1)
                        w = MarkupObjects.RectangleArea.Width;
                    else if (j == columnsNumber - 1)
                        w = MarkupObjects.RectangleArea.Right - x;
                    else
                        w = verticalLinesCoordinates[j] - x;

                    areas[j, i] = new Rectangle(x, y, w, h);
                }
            }
        
            return areas;
        }

        /// <summary>
        /// Applies filter to image
        /// </summary>
        /// <param name="bitmap">Bitmap</param>
        /// <param name="filter">Filter</param>
        private Bitmap ApplyFilter(Bitmap bitmap, Filter filter)
        {
            switch (filter.FilterType)
            {
                case FilterType.Binarization:
                    return BitmapHelper.Binarize(bitmap, (byte) filter.Parameters);

                case FilterType.Brightness:
                    return BitmapHelper.SetBrightness(bitmap, (int)filter.Parameters);

                case FilterType.Contrast:
                    return BitmapHelper.SetContrast(bitmap, (int)filter.Parameters);

                case FilterType.GammaCorrection:
                    return BitmapHelper.SetGammaCorrection(bitmap, (int)filter.Parameters);

                case FilterType.AutoCrop:
                    return BitmapHelper.AutoCropBorders(bitmap);

                case FilterType.Crop:
                    return BitmapHelper.Crop(bitmap, (Rectangle)filter.Parameters);

                case FilterType.Dilation:
                    return BitmapHelper.Dilate(bitmap);

                case FilterType.Erosion:
                    return BitmapHelper.Erode(bitmap);

                case FilterType.HolesRemoval:
                    return BitmapHelper.RemoveHoles(bitmap, (HolesPosition)filter.Parameters);

                case FilterType.BlobsRemoval:
                    return BitmapHelper.RemoveBlobs(bitmap, (BlobsDimensions)filter.Parameters);

                case FilterType.LinesRemoval:
                    return BitmapHelper.RemoveLines(bitmap, (LineRemoveOrientation)filter.Parameters);

                case FilterType.StapleMarksRemoval:
                    return BitmapHelper.RemoveStapleMarks(bitmap);

                case FilterType.NoiseRemoval:
                    return BitmapHelper.RemoveNoise(bitmap);

                case FilterType.AutoDeskew:
                    return BitmapHelper.AutoDeskew(bitmap);

                case FilterType.Rotation:
                    return BitmapHelper.Rotate(bitmap, (float)filter.Parameters, Color.Black);

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        /// <summary>
        /// Generates name of the temporary file
        /// </summary>
        /// <param name="fileName">Name of the file</param>
        /// <returns></returns>
        private string GenerateTempFileName(string fileName)
        {
            return $"{Path.GetFileNameWithoutExtension(fileName)}_temp.jpg";
        }

        /// <summary>
        /// Generates name of the temporary thumbnail file
        /// </summary>
        /// <param name="fileName">Name of the file</param>
        /// <returns></returns>
        private string GenerateTempThumbnailFileName(string fileName)
        {
            return $"{Path.GetFileNameWithoutExtension(fileName)}_thumb_temp.jpg";
        }

        /// <summary>
        /// Generates name of the original file
        /// </summary>
        /// <param name="fileName">Name of the file</param>
        /// <returns></returns>
        private string GenerateOriginalFileName(string fileName)
        {
            return $"{Path.GetFileNameWithoutExtension(fileName)}_original.jpg";
        }

        /// <summary>
        /// Clears service images
        /// </summary>
        private void ClearServiceImages()
        {
            File.Delete(OriginalFilePath);
            OriginalFileName = string.Empty;
            File.Delete(TempFilePath);
            TempFilePath = string.Empty;
            File.Delete(TempThumbnailFilePath);
            TempThumbnailFilePath = string.Empty;
        }

        #endregion
    }
}
