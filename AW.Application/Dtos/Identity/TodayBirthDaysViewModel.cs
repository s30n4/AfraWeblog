using System.Collections.Generic;
using AW.Entities.Domain.Identity;

namespace AW.Application.Dtos.Identity
{
    public class TodayBirthDaysViewModel
    {
        public List<User> Users { set; get; }

        public AgeStatViewModel AgeStat { set; get; }
    }
}