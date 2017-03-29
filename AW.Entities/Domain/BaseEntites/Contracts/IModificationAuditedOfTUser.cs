using System;

namespace AW.Entities.Domain.BaseEntites.Contracts
{
    /// <summary>
    /// Adds navigation properties to <see cref="IModificationAudited"/> interface for user.
    /// </summary>
    /// <typeparam name="TUser">Type of the user</typeparam>
    public interface IModificationAudited<TUser> : IModificationAudited
        where TUser : IEntity<Guid>
    {
        /// <summary>
        /// Reference to the last modifier user of this entity.
        /// </summary>
        TUser LastModifierUser { get; set; }
    }
}