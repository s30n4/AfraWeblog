using System;
using AW.Common.Extensions;
using AW.Entities.Domain.BaseEntites.Contracts;

namespace AW.Entities.Domain.BaseEntites
{
    /// <summary>
    ///     Some useful extension methods for Entities.
    /// </summary>
    public static class EntityExtensions
    {
        /// <summary>
        ///     Check if this Entity is null of marked as deleted.
        /// </summary>
        public static bool IsNullOrDeleted(this ISoftDelete entity)
        {
            return entity == null || entity.IsDeleted;
        }

        // <summary>
        //Undeletes this entity by setting<see cref="ISoftDelete.IsDeleted"/> to false and
        // <see cref = "IDeletionAudited" /> properties to null.
        // </summary>
        public static void UnDelete(this ISoftDelete entity)
        {
            entity.IsDeleted = false;
            var deletionAuditedEntity = entity.As<IDeletionAudited>();
            deletionAuditedEntity.DeletionTime = null;
            deletionAuditedEntity.DeleterUserId = null;
        }

        public static void Delete(this ISoftDelete entity)
        {
            entity.IsDeleted = true;
            var deletionAuditedEntity = entity.As<IDeletionAudited>();
            deletionAuditedEntity.DeletionTime = DateTime.Now;
            deletionAuditedEntity.DeleterUserId = null;
        }
    }
}