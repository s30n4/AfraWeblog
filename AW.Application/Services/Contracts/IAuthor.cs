using AW.Application.Dtos.Author;
using AW.Common;
using System.Threading.Tasks;

namespace AW.Application.Services.Contracts
{
    public interface IAuthor
    {
        Task<ServiceResult<int>> AddAsync(AuthorDto data, int id = 0);

        ServiceResult<AuthorDto> GetById(int id);

        ServiceResult<int> DeleteById(int id);
    }
}
