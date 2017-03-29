using AW.Entities.AuditableEntity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace AW.Entities.Domain.Identity
{
   
    public class Role : IdentityRole<int, UserRole, RoleClaim>, IAuditableEntity
    {
        public Role()
        {
        }

        public Role(string name)
            : this()
        {
            Name = name;
        }

        public Role(string name, string description)
            : this(name)
        {
            Description = description;
        }

        public string Description { get; set; }
    }
}