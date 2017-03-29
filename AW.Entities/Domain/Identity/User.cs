using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AW.Entities.AuditableEntity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace AW.Entities.Domain.Identity
{

    public class User : IdentityUser<int, UserClaim, UserRole, UserLogin>, IAuditableEntity
    {
        public User()
        {
            UserUsedPasswords = new HashSet<UserUsedPassword>();
            UserTokens = new HashSet<UserToken>();
        }

        [StringLength(450)]
        public string FirstName { get; set; }

        [StringLength(450)]
        public string LastName { get; set; }

        [StringLength(450)]
        public string PhotoFileName { get; set; }

        public DateTimeOffset? BirthDate { get; set; }

        public DateTimeOffset? CreatedDateTime { get; set; }

        public DateTimeOffset? LastVisitDateTime { get; set; }

        public DateTimeOffset? LastUpdatePasswordDateTime { get; set; }

        public bool IsEmailPublic { get; set; }

        public string Location { set; get; }

        public bool IsActive { get; set; } = true;

        [StringLength(500)]
        public string Information { get; set; }

        public virtual ICollection<UserUsedPassword> UserUsedPasswords { get; set; }

        public virtual ICollection<UserToken> UserTokens { get; set; }
    }
}