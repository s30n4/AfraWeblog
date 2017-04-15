using System.Collections.Generic;
using System.Threading.Tasks;
using AW.Common.SerializationToolkit;

namespace AW.Application.DtoDefault
{
    /// <summary>
    /// This class can be used to return a paged list from an <see>
    ///         <cref>IApplicationService</cref>
    ///     </see>
    ///     method.
    /// </summary>
    /// <typeparam name="T">Type of the items in the <see cref="ListResultDto{T}.Items"/> list</typeparam>
    [Serializable]
    public class PagedResultOutput<T> : PagedResultDto<T>
    {
        /// <summary>
        /// Creates a new <see cref="PagedResultOutput{T}"/> object.
        /// </summary>
        public PagedResultOutput()
        {
        }

        /// <summary>
        /// Creates a new <see cref="PagedResultOutput{T}"/> object.
        /// </summary>
        /// <param name="totalCount">Total count of Items</param>
        /// <param name="items">List of items in current page</param>
        public PagedResultOutput(Task<int> totalCount, IReadOnlyList<T> items)
            : base(totalCount, items)
        {
        }
    }
}