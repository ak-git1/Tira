using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using Ak.Framework.Core.Extensions;
using Ak.Framework.Core.Helpers;
using Tira.Logic.Enums;

namespace Tira.Logic.Models
{
    /// <summary>
    /// Data column
    /// </summary>
    [Serializable]
    public class DataColumn : ICloneable
    {
        #region Variables

        /// <summary>
        /// Data column types 
        /// </summary>
        public static List<DataColumnType> _dataColumnTypes;

        #endregion

        #region Properties

        /// <summary>
        /// Name
        /// </summary>
        [XmlElement]
        public string Name { get; set; }

        /// <summary>
        /// Column type
        /// </summary>
        [XmlElement]
        public DataColumnType ColumnType { get; set; }

        /// <summary>
        /// Column type name
        /// </summary>
        public string ColumnTypeName => GetColumnTypeName(ColumnType);

        /// <summary>
        /// Data column types 
        /// </summary>
        public static List<DataColumnType> DataColumnTypes
        {
            get
            {
                if (_dataColumnTypes == null)
                    _dataColumnTypes = EnumNamesHelper.GetValues<DataColumnType>().ToList();
                return _dataColumnTypes;
            }
        }

        /// <summary>
        /// List of data column types
        /// </summary>
        public static List<KeyValuePair<DataColumnType, string>> DataColumnTypesList => DataColumnTypes.Select(l => new KeyValuePair<DataColumnType, string>(l, GetColumnTypeName(l))).ToList();

        /// <summary>
        /// Flag for removing extra spaces
        /// </summary>
        [XmlElement]
        public bool RemoveExtraSpaces { get; set; }

        /// <summary>
        /// Flag for removing line breaks
        /// </summary>
        [XmlElement]
        public bool RemoveLineBreaks { get; set; }

        /// <summary>
        /// Flag for removing punctuation
        /// </summary>
        [XmlElement]
        public bool RemovePunctuation { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DataColumn"/> class.
        /// </summary>
        public DataColumn()
        {            
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataColumn"/> class.
        /// </summary>
        /// <param name="dataColumn">Data column</param>
        public DataColumn(DataColumn dataColumn)
        {
            Name = dataColumn.Name;
            ColumnType = dataColumn.ColumnType;
            RemoveExtraSpaces = dataColumn.RemoveExtraSpaces;
            RemoveLineBreaks = dataColumn.RemoveLineBreaks;
            RemovePunctuation = dataColumn.RemovePunctuation;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Gets the name of the column type
        /// </summary>
        /// <param name="dataColumnType">Data column type</param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException">dataColumnType - null</exception>
        public static string GetColumnTypeName(DataColumnType dataColumnType)
        {
            switch (dataColumnType)
            {
                case DataColumnType.AnySymbols:
                    return Properties.Resources.DataColumnType_AnySymbols;

                case DataColumnType.EmptyColumn:
                    return Properties.Resources.DataColumnType_EmptyColumn;

                case DataColumnType.LettersOnly:
                    return Properties.Resources.DataColumnType_LettersOnly;

                case DataColumnType.NumbersOnly:
                    return Properties.Resources.DataColumnType_NumbersOnly;

                default:
                    throw new ArgumentOutOfRangeException(nameof(dataColumnType), dataColumnType, null);
            }
        }

        /// <summary>
        /// Clones this instance
        /// </summary>
        /// <returns></returns>
        public object Clone()
        {
            return new DataColumn(this);
        }

        #endregion
    }
}