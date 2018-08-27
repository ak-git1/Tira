using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Tira.Logic.Enums
{
    /// <summary>
    /// Types of data column
    /// </summary>
    public enum DataColumnType
    {
        /// <summary>
        /// Any symbols column
        /// </summary>
        AnySymbols = 0,

        /// <summary>
        /// Letters only column
        /// </summary>
        LettersOnly = 1,

        /// <summary>
        /// Numbers only column
        /// </summary>
        NumbersOnly = 2,

        /// <summary>
        /// Empty column
        /// (data will not be recognized)
        /// </summary>
        EmptyColumn = 3
    }
}
