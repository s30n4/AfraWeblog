using AW.Entities.AuditableEntity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace AW.Entities.Domain.Identity
{
   
    public class RoleClaim : IdentityRoleClaim<int>, IAuditableEntity
    {
        public virtual Role Role { get; set; }
    }
}