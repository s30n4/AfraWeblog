using System.Collections.Generic;
using AW.Common.SerializationToolkit;

namespace AW.Application.DtoDefault
{
    /// <summary>
    /// This class can be used to return a list from an <see /> method.
    /// </summary>
    /// <typeparam name="T">Type of the items in the <see cref="ListResultDto{T}.Items"/> list</typeparam>
    [Serializable]
    public class ListResultOutput<T> : ListResultDto<T>
    {
        /// <summary>
        /// Creates a new <see cref="ListResultOutput{T}"/> object.
        /// </summary>
        public ListResultOutput()
        {
        }

        /// <summary>
        /// Creates a new <see cref="ListResultOutput{T}"/> object.
        /// </summary>
        /// <param name="items">List of items</param>
        public ListResultOutput(IReadOnlyList<T> items)
            : base(items)
        {
        }
    }
}