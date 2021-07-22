using System;
using System.Collections.Generic;
using System.Text;

namespace PipServices3.Commons.Data
{
    /// <summary>
    /// Interface for data objects that contain their latest change time.
    /// </summary>
    /// <example>
    /// <code>
    /// export class MyData implements IStringIdentifiable, IChangeable {
    ///     public id: string;
    ///     public field1: string;
    ///     public field2: number;
    ///     public change_time: Date;
    ///     ...
    /// }
    /// </code>
    /// </example>
    public interface IChangeable
    {
        /// <summary>
        /// The UTC time at which the object was last changed (created or updated).
        /// </summary>
        DateTime ChangeTime { get; set; }
    }
}
