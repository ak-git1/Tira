namespace Tira.Logic.Repository.Entities
{
    /// <summary>
    /// Recently used project
    /// (Database entity)
    /// </summary>
    internal class RecentProject
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
        /// Path
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// Last access date
        /// </summary>
        public string LastAccessDate { get; set; }

        #endregion
    }
}
