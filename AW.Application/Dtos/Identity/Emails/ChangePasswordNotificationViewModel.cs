using AW.Entities.Domain.Identity;

namespace AW.Application.Dtos.Identity.Emails
{
    public class ChangePasswordNotificationViewModel : EmailsBase
    {
        public User User { set; get; }
    }
}