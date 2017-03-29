using System;

namespace AW.Entities.Domain.BaseEntites
{
    /// <summary>
    ///     Used to identify an entity.
    ///     Can be used to store an entity <see cref="Type" /> and <see cref="Id" />.
    /// </summary>
    [Common.SerializationToolkit.Serializable]
    public class EntityIdentifier
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="EntityIdentifier" /> class.
        /// </summary>
        /// <param name="type">Entity type.</param>
        /// <param name="id">Id of the entity.</param>
        public EntityIdentifier(Type type, object id)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            if (id == null)
            {
                throw new ArgumentNullException(nameof(id));
            }

            Type = type;
            Id = id;
        }

        /// <summary>
        ///     Entity Type.
        /// </summary>
        public Type Type { get; private set; }

        /// <summary>
        ///     Entity's Id.
        /// </summary>
        public object Id { get; private set; }
    }
}