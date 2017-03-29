using System.Reflection;
using AW.Common.SerializationToolkit;
using AW.Entities.Domain.BaseEntites.Contracts;

namespace AW.Entities.Domain.BaseEntites
{
    /// <summary>
    ///     Basic implementation of IEntity interface.
    ///     An entity can inherit this class of directly implement to IEntity interface.
    /// </summary>
    /// <typeparam name="TPrimaryKey">Type of the primary key of the entity</typeparam>
    [Serializable]
    public abstract class Entity<TPrimaryKey> : IEntity<TPrimaryKey>
    {
        /// <summary>
        ///     Unique identifier for this entity.
        /// </summary>
        public virtual TPrimaryKey Id { get; set; }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            if (!(obj is Entity<TPrimaryKey>))
            {
                return false;
            }

            //Same instances must be considered as equal
            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            //Transient objects are not considered as equal
            var other = (Entity<TPrimaryKey>)obj;

            //Must have a IS-A relation of types or must be same type
            var typeOfThis = GetType();
            var typeOfOther = other.GetType();
            if (!typeOfThis.IsAssignableFrom(typeOfOther) && !typeOfOther.IsAssignableFrom(typeOfThis))
            {
                return false;
            }

            return Id.Equals(other.Id);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        /// <inheritdoc />
        public static bool operator ==(Entity<TPrimaryKey> left, Entity<TPrimaryKey> right)
        {
            return Equals(left, null) ? Equals(right, null) : left.Equals(right);
        }

        /// <inheritdoc />
        public static bool operator !=(Entity<TPrimaryKey> left, Entity<TPrimaryKey> right)
        {
            return !(left == right);
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return $"[{GetType().Name} {Id}]";
        }
    }
}