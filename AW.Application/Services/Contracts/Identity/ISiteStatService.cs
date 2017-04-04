using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AW.Application.Dtos.Identity;
using AW.Entities.Domain.Identity;

namespace AW.Application.Services.Contracts.Identity
{
    public interface ISiteStatService
    {
        Task<List<User>> GetOnlineUsersListAsync(int numbersToTake, int minutesToTake);

        Task<List<User>> GetTodayBirthdayListAsync();

        Task UpdateUserLastVisitDateTimeAsync(ClaimsPrincipal claimsPrincipal);

        Task<AgeStatViewModel> GetUsersAverageAge();
    }
}