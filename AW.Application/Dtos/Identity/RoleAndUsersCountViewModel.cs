using AW.Entities.Domain.Identity;

namespace AW.Application.Dtos.Identity
{
    public class RoleAndUsersCountViewModel
    {
        public Role Role { set; get; }
        public int UsersCount { set; get; }
    }
}