using System;

namespace PipServices3.Commons.Data
{
    /// <summary>
    /// Interface for data objects that can track their changes, including logical deletion. 
    /// </summary>
    /// <example>
    /// <code>
    /// public class MyData: IStringIdentifiable, ITrackable 
    /// {
    ///     string id {get; set;}
    ///     string field1;
    ///     int field2;
    ///     ...
    ///     DateTime change_time {get; set;}
    ///     DateTime create_time {get; set;}
    ///     bool deleted {get; set;}
    /// }
    /// </code>
    /// </example>
    public interface ITrackable
    {
        /** The UTC time at which the object was created. */
        DateTime CreatedTime { get; set; }

        /** The UTC time at which the object was last changed (created, updated, or deleted). */
        DateTime LastChangeTime { get; set; }

        /** The logical deletion flag. True when object is deleted and null or false otherwise */
        bool IsDeleted { get; set; }
    }
}