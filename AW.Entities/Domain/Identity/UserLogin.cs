using AW.Entities.AuditableEntity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace AW.Entities.Domain.Identity
{

    public class UserLogin : IdentityUserLogin<int>, IAuditableEntity
    {
        public virtual User User { get; set; }
    }
}