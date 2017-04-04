using AW.Application.Dtos.NewsCategory;
using AW.Common;
using System.Threading.Tasks;

namespace AW.Application.Services.Contracts
{
    public interface INewsCategory
    {
        Task<ServiceResult<int>> AddAsync(NewsCategoryDto data, int id = 0);

        ServiceResult<NewsCategoryDto> GetById(int id);

        ServiceResult<int> DeleteById(int id);
    }
}
