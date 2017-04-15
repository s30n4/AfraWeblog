using AW.Common.SerializationToolkit;

namespace AW.Application.DtoDefault
{
    /// <summary>
    /// This DTO can be used to send Id of an entity as response from an <see /> method.
    /// </summary>
    /// <typeparam name="TPrimaryKey">Type of the primary key of entity</typeparam>
    [Serializable]
    public class EntityResultOutput<TPrimaryKey> : EntityDto<TPrimaryKey>
    {
        /// <summary>
        /// Creates a new <see cref="EntityResultOutput{TPrimaryKey}"/> object.
        /// </summary>
        public EntityResultOutput()
        {
        }

        /// <summary>
        /// Creates a new <see cref="EntityResultOutput{TPrimaryKey}"/> object.
        /// </summary>
        /// <param name="id">Id of the entity</param>
        public EntityResultOutput(TPrimaryKey id)
            : base(id)
        {
        }
    }
}