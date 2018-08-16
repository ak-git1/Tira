using System.ComponentModel;

namespace Tira.Logic.Enums
{
    /// <summary>
    /// Языки интерфейса
    /// </summary>
    public enum Languages
    {
        /// <summary>
        /// Язык не выбран
        /// </summary>
        [Description("en-US")]
        None = 0,

        /// <summary>
        /// Английский
        /// </summary>
        [Description("en-US")]
        En = 1,

        /// <summary>
        /// Русский
        /// </summary>
        [Description("ru-RU")]
        Ru = 2
    }
}
