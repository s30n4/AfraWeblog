namespace AW.Application.Dtos.Identity.Emails
{
    public class TwoFactorSendCodeViewModel : EmailsBase
    {
        public string Token { set; get; }
    }
}