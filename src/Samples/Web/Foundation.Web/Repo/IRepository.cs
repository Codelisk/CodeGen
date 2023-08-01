namespace Foundation.Web.Repo
{
    public enum MyEntityState
    {
        /// <summary>
        /// see entity framework.
        /// </summary>
        Detached,

        /// <summary>
        /// see entity framework.
        /// </summary>
        Unchanged,

        /// <summary>
        /// see entity framework.
        /// </summary>
        Deleted,

        /// <summary>
        /// see entity framework.
        /// </summary>
        Modified,

        /// <summary>
        /// see entity framework.
        /// </summary>
        Added,
    }

    public interface IRepository
    {
        // We use this just as a marker for all our repositories to set a constraint on the generic RepositoryManager.
    }
}
