namespace ProLab.Infrastructure.Core
{
    /// <summary>
    /// Represents a soft-deleted (without actually deleting from storage) entity
    /// </summary>
    public interface IDataIntegrityEntity
    {
        /// <summary>
        /// Gets or sets a value indicating whether the entity has been deleted
        /// </summary>
        string DataIntegrity { get; set; }


        /// <summary>
        /// Gets hash code of entity
        /// </summary>
        void ApplyDataIntegrity();
       
    }
}
