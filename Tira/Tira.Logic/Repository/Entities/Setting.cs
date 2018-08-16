namespace Tira.Logic.Repository.Entities
{
    /// <summary>
    /// Setting
    /// (Database entity)
    /// </summary>
    internal class Setting
    {
        #region Properties

        /// <summary>
        /// Identifier
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Value
        /// </summary>
        public string Value { get; set; }

        #endregion
    }
}
