using AW.Application.Dtos.Comment;
using AW.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AW.Application.Services.Contracts
{
    public interface IComment
    {
        Task<ServiceResult<int>> AddAsync(CommentAddDto data, int id = 0);

        ServiceResult<CommentOutputDto> GetById(int id);

        ServiceResult<int> Confirm(CommentConfirmDto data);

        ServiceResult<int> DeleteById(int id);
    }
}
