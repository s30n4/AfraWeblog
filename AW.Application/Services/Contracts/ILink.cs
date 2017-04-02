using AW.Application.Dtos.Link;
using AW.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AW.Application.Services.Contracts
{
    public interface ILink
    {
        Task<ServiceResult<int>> AddAsync(LinkDto data, int id = 0);

        ServiceResult<LinkDto> GetById(int id);

        ServiceResult<int> DeleteById(int id);
    }
}
