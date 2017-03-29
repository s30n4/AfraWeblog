using AW.Entities.AuditableEntity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace AW.Entities.Domain.Identity
{
    
    public class UserClaim : IdentityUserClaim<int>, IAuditableEntity
    {
        public virtual User User { get; set; }
    }
}