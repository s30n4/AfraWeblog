using System;

namespace AW.Entities.Domain.BaseEntites.Contracts
{
    /// <summary>
    /// Adds navigation properties to <see cref="IAudited"/> interface for user.
    /// </summary>
    /// <typeparam name="TUser">Type of the user</typeparam>
    public interface IAudited<TUser> : IAudited, IModificationAudited<TUser>
        where TUser : IEntity<Guid>
    {
    }
}