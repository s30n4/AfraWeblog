using AW.Common.SerializationToolkit;

namespace AW.Application.DtoDefault
{
    /// <summary>
    /// This DTO can be used to send Id of an entity to an <see /> method.
    /// </summary>
    /// <typeparam name="TPrimaryKey">Type of the primary key of entity</typeparam>
    [Serializable]
    public class EntityRequestInput<TPrimaryKey> : EntityDto<TPrimaryKey>
    {
        /// <summary>
        /// Creates a new <see cref="EntityRequestInput{TPrimaryKey}"/> object.
        /// </summary>
        public EntityRequestInput()
        {
        }

        /// <summary>
        /// Creates a new <see cref="EntityRequestInput{TPrimaryKey}"/> object.
        /// </summary>
        /// <param name="id">Id of the entity</param>
        public EntityRequestInput(TPrimaryKey id)
            : base(id)
        {
        }
    }
}