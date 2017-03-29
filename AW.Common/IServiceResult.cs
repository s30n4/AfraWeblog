using System.Collections.Generic;

namespace AW.Common
{
    public interface IServiceResult
    {
        /// <summary>
        /// Flag indicating whether if the operation succeeded or not.
        /// </summary>
        /// <value>True if the operation succeeded, otherwise false.</value>
        bool Succeeded { get; set; }

        /// <summary>
        /// An <see cref="IEnumerable{T}"/> of <see cref="ServiceMessage"/>s containing messages or errors
        /// that occurred during the identity operation.
        /// </summary>
        /// <value>An <see cref="IEnumerable{T}"/> of <see cref="ServiceMessage"/>s.</value>
        List<ServiceMessage> Messages { get; }
    }
}
