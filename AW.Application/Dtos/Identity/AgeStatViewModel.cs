using AW.Entities.Domain.Identity;
using DNTPersianUtils.Core;

namespace AW.Application.Dtos.Identity
{
    public class AgeStatViewModel
    {
        const char RleChar = (char)0x202B;

        public int UsersCount { set; get; }
        public int AverageAge { set; get; }
        public User MaxAgeUser { set; get; }
        public User MinAgeUser { set; get; }

        public string MinMax => $"{RleChar}جوان‌ترین عضو: {string.Format("{0} {1}", MinAgeUser.FirstName,MinAgeUser.LastName) } ({MinAgeUser.BirthDate.Value.GetAge()})، مسن‌ترین عضو: {string.Format("{0} {1}", MaxAgeUser.FirstName, MaxAgeUser.LastName)} ({MaxAgeUser.BirthDate.Value.GetAge()})، در بین {UsersCount} نفر";
    }
}