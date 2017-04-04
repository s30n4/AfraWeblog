using AW.Entities.Domain.Identity;

namespace AW.Application.Dtos.Identity.Emails
{
    public class UserProfileUpdateNotificationViewModel : EmailsBase
    {
        public User User { set; get; }
    }
}