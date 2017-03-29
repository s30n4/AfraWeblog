using System;

namespace AW.Entities.Domain.BaseEntites.Contracts
{
    /// <summary>
    /// Adds navigation properties to <see cref="IFullAudited"/> interface for user.
    /// </summary>
    /// <typeparam name="TUser">Type of the user</typeparam>
    public interface IFullAudited<TUser> : IAudited<TUser>, IFullAudited, IDeletionAudited<TUser>, IApproverAudited, ICreateionNetwork
        where TUser : IEntity<Guid>
    {
    }
}