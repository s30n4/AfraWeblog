using System.Collections.Generic;
using AW.Entities.Domain.Identity;

namespace AW.Application.Dtos.Identity
{
    public class OnlineUsersViewModel
    {
        public List<User> Users { set; get; }
        public int NumbersToTake { set; get; }
        public int MinutesToTake { set; get; }
        public bool ShowMoreItemsLink { set; get; }
    }
}