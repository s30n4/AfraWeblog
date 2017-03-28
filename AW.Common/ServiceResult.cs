using AW.Common.SerializationToolkit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AW.Common
{
    [Serializable]
    public class ServiceResult<TEntity> : IServiceResult
    {
        public TEntity Entity { get; private set; }

        private readonly List<ServiceMessage> _messages = new List<ServiceMessage>();

        /// <summary>
        /// Flag indicating whether if the operation succeeded or not.
        /// </summary>
        /// <value>True if the operation succeeded, otherwise false.</value>
        public bool Succeeded { get; set; }

        /// <summary>
        /// An <see cref="IEnumerable{T}"/> of <see cref="ServiceMessage"/>s containing an errors
        /// that occurred during the identity operation.
        /// </summary>
        /// <value>An <see cref="IEnumerable{T}"/> of <see cref="ServiceMessage"/>s.</value>
        public List<ServiceMessage> Messages => _messages;

        /// <summary>
        /// Returns an <see cref="ServiceResult"/> indicating a successful identity operation.
        /// </summary>
        /// <returns>An <see cref="ServiceResult"/> indicating a successful operation.</returns>
        public static ServiceResult<TEntity> Success(TEntity entity, params ServiceMessage[] messages)
        {
            var result = new ServiceResult<TEntity>
            {
                Succeeded = true,
                Entity = entity
            };

            if (messages != null)
            {
                result._messages.AddRange(messages);
            }

            return result;
        }

        /// <summary>
        /// Creates an <see cref="ServiceResult"/> indicating a failed identity operation, with a list of <paramref name="errors"/> if applicable.
        /// </summary>
        /// <param name="errors">An optional array of <see cref="ServiceMessage"/>s which caused the operation to fail.</param>
        /// <returns>An <see cref="ServiceResult"/> indicating a failed identity operation, with a list of <paramref name="errors"/> if applicable.</returns>
        public static ServiceResult<TEntity> Failed(params ServiceMessage[] errors)
        {
            var result = new ServiceResult<TEntity>
            {
                Succeeded = false
            };

            if (errors != null)
            {
                result._messages.AddRange(errors);
            }

            return result;
        }

        /// <summary>
        /// Converts the value of the current <see cref="ServiceResult"/> object to its equivalent string representation.
        /// </summary>
        /// <returns>A string representation of the current <see cref="ServiceResult"/> object.</returns>
        /// <remarks>
        /// If the operation was successful the ToString() will return "Succeeded" otherwise it returned
        /// "Failed : " followed by a comma delimited list of error codes from its <see cref="Messages"/> collection, if any.
        /// </remarks>
        public override string ToString()
        {
            return Succeeded
                ? "Succeeded"
                : $"Failed : {string.Join(",", Messages.Select(x => x.Code).ToList())}";
        }
    }

    public class ServiceResult : IServiceResult
    {
        private readonly List<ServiceMessage> _messages = new List<ServiceMessage>();

        public bool Succeeded { get; set; }

        public List<ServiceMessage> Messages => _messages;

        /// <summary>
        /// Returns an <see cref="ServiceResult"/> indicating a successful identity operation.
        /// </summary>
        /// <returns>An <see cref="ServiceResult"/> indicating a successful operation.</returns>
        public static ServiceResult Success(params ServiceMessage[] messages)
        {
            var result = new ServiceResult
            {
                Succeeded = true,
            };

            if (messages != null)
            {
                result._messages.AddRange(messages);
            }

            return result;
        }

        /// <summary>
        /// Creates an <see cref="ServiceResult"/> indicating a failed identity operation, with a list of <paramref name="errors"/> if applicable.
        /// </summary>
        /// <param name="errors">An optional array of <see cref="ServiceMessage"/>s which caused the operation to fail.</param>
        /// <returns>An <see cref="ServiceResult"/> indicating a failed identity operation, with a list of <paramref name="errors"/> if applicable.</returns>
        public static ServiceResult Failed(params ServiceMessage[] errors)
        {
            var result = new ServiceResult
            {
                Succeeded = false
            };

            if (errors != null)
            {
                result._messages.AddRange(errors);
            }

            return result;
        }

        /// <summary>
        /// Converts the value of the current <see cref="ServiceResult"/> object to its equivalent string representation.
        /// </summary>
        /// <returns>A string representation of the current <see cref="ServiceResult"/> object.</returns>
        /// <remarks>
        /// If the operation was successful the ToString() will return "Succeeded" otherwise it returned
        /// "Failed : " followed by a comma delimited list of error codes from its <see cref="Messages"/> collection, if any.
        /// </remarks>
        public override string ToString()
        {
            return Succeeded
                ? "Succeeded"
                : $"Failed : {string.Join(",", Messages.Select(x => x.Code).ToList())}";
        }
    }
}
