using System;

namespace PipServices.Commons.Data
{
    /// <summary>
    /// Interface for data objects that can track their changes including logical deletion 
    /// </summary>
    public interface ITrackable
    {
        /// <summary>
        /// Gets the time when the object was created
        /// </summary>
        DateTime CreatedTime { get; set; }

        /// <summary>
        /// Gets the last time when the object was changed (created, updated or deleted)
        /// </summary>
        DateTime LastChangeTime { get; set; }

        /// <summary>
        /// Gets the logical deletion flag
        /// </summary>
        bool IsDeleted { get; set; }
    }
}