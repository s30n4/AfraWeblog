using AW.Application.Dtos.NewsContent;
using AW.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AW.Application.Services.Contracts
{
    public interface INewsContent
    {
        Task<ServiceResult<int>> AddAsync(ContentAddDto data, int id = 0);

        ServiceResult<ContentOutputDto> GetById(int id);

        ServiceResult<int> DeleteById(int id);
    }
}
