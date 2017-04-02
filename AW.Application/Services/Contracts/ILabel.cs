using AW.Application.Dtos.Label;
using AW.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AW.Application.Services.Contracts
{
    public interface ILabel
    {
        Task<ServiceResult<int>> AddAsync(LabelDto data, int id = 0);

        ServiceResult<LabelDto> GetById(int id);

        ServiceResult<int> DeleteById(int id);
    }
}
