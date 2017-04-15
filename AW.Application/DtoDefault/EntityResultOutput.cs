using AW.Application.DtoDefault.Contracts;
using AW.Common.SerializationToolkit;

namespace AW.Application.DtoDefault
{
    /// <summary>
    /// This <see /> can be used to send Id of an entity as response from an <see /> method.
    /// </summary>
    [Serializable]
    public class EntityResultOutput : EntityResultOutput<int>, IEntityDto
    {
        /// <summary>
        /// Creates a new <see cref="EntityResultOutput"/> object.
        /// </summary>
        public EntityResultOutput()
        {
        }

        /// <summary>
        /// Creates a new <see cref="EntityResultOutput"/> object.
        /// </summary>
        /// <param name="id">Id of the entity</param>
        public EntityResultOutput(int id)
            : base(id)
        {
        }
    }
}