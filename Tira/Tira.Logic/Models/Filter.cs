using System;
using System.Xml.Serialization;
using Tira.Logic.Enums;

namespace Tira.Logic.Models
{
    /// <summary>
    /// Image filter
    /// </summary>
    [Serializable]
    public class Filter
    {
        #region Properties

        /// <summary>
        /// Uid
        /// </summary>
        [XmlElement]
        public FilterType FilterType { get; set; }

        /// <summary>
        /// Parameters
        /// </summary>
        [XmlElement]
        public object Parameters { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Filter"/> class.
        /// </summary>
        public Filter()
        {            
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Filter"/> class.
        /// </summary>
        /// <param name="filterType">Filter type</param>
        /// <param name="parameters">Parameters</param>
        public Filter(FilterType filterType, object parameters)
        {
            FilterType = filterType;
            Parameters = parameters;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Checks for drawing objects removal
        /// </summary>
        /// <param name="filterType">Filter type</param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException">filterType - null</exception>
        public static bool RemoveDrawingObjects(FilterType filterType)
        {
            switch (filterType)
            {
                case FilterType.Binarization:
                case FilterType.Brightness:
                case FilterType.Contrast:
                case FilterType.GammaCorrection:
                case FilterType.Dilation:
                case FilterType.Erosion:
                case FilterType.HolesRemoval:
                case FilterType.BlobsRemoval:
                case FilterType.LinesRemoval:
                case FilterType.StapleMarksRemoval:
                case FilterType.NoiseRemoval:
                    return false;

                case FilterType.AutoCrop:
                case FilterType.Crop:
                case FilterType.AutoDeskew:
                case FilterType.Rotation:
                    return true;

                default:
                    throw new ArgumentOutOfRangeException(nameof(filterType), filterType, null);
            }
        }

        #endregion
    }
}