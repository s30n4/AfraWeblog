using AutoMapper;
using AutoMapper.QueryableExtensions;
using AW.Application.Dtos.Comment;
using AW.Application.Services.Contracts;
using AW.Common;
using AW.DataLayer.Context;
using AW.Entities.Domain;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AW.Application.Services
{
    public class CommentService: IComment
    {
        private const string EntityName = "Comments";

        private readonly DbSet<Comment> _dbSet;
        private IUnitOfWork UnitOfWork { get; set; }
        private IMapper MapperEngine { get; set; }

        public CommentService(IMapper mapper, IHttpContextAccessor httpContextAccessor
            , IHostingEnvironment hostingEnvironment, ILogger<ApplicationDbContextBase> logger)
        {
            MapperEngine = mapper;
            UnitOfWork = new ApplicationDbContext(httpContextAccessor, hostingEnvironment, logger);
            _dbSet = UnitOfWork.Set<Comment>();
        }


        public async Task<ServiceResult<int>> AddAsync(CommentAddDto data, int id = 0)
        {
            if (data == null) return ServiceResult<int>.Failed(new ServiceMessage { Description = "data is null" });
            Comment comm;
            if (id == 0)
            {
                comm = MapperEngine.Map<Comment>(data);
                comm.SubmitDate = DateTime.Now;
                comm.IsConfirm = false;
                _dbSet.Add(comm);
            }
            else
            {
                comm = _dbSet.SingleOrDefault(a => a.Id == id);
                if (comm == null) return ServiceResult<int>.Failed(new ServiceMessage { Description = "id is incorrect" });

                Mapper.Map(data, comm);
                comm.Id = id;
                _dbSet.Update(comm);
            }

            var res = await UnitOfWork.SaveChangesAsync();
            return ServiceResult<int>.Success(res);
        }

        //public async Task<ServiceResult<SystemListDto>> GetAsync(BaseSearchRequest searchRequest)
        //{
        //    var query = _dbSet.ProjectTo<SystemDto>(MapperEngine);
        //    searchRequest = (BaseSearchRequest)SgeCore.SetBaseSearchRequest(searchRequest, query.CountAsync());

        //    var systems = new SystemListDto
        //    {
        //        Data = await query.Skip(searchRequest.SkipCount).Take(searchRequest.PageSize).ToListAsync(),
        //        SearchRequest = searchRequest
        //    };

        //    systems.Headers = SgeCore.RefineEntityHeader(systems.Headers, EntityName, searchRequest.LanguageId);
        //    return ServiceResult<SystemListDto>.Success(systems);
        //}

        public ServiceResult<CommentOutputDto> GetById(int id)
        {
            var query = _dbSet.Where(a => a.Id == id).ProjectTo<CommentOutputDto>(MapperEngine).FirstOrDefault();
            return ServiceResult<CommentOutputDto>.Success(query);
        }

        public ServiceResult<int> Confirm(CommentConfirmDto data)
        {
            if (data == null) return ServiceResult<int>.Failed(new ServiceMessage { Description = "data is null" });
            int res = 0;
            var query = _dbSet.Where(a => a.Id == data.Id).FirstOrDefault();
            if (query != null)
            {
                query.IsConfirm = data.IsConfirm;
                _dbSet.Update(query);
            }
            return ServiceResult<int>.Success(res);
        }

        public ServiceResult<int> DeleteById(int id)
        {
            int res = 0;
            var query = _dbSet.Where(a => a.Id == id).FirstOrDefault();
            if (query != null)
            {
                _dbSet.Remove(query);
                res = query.Id;
            }
            return ServiceResult<int>.Success(res);
        }
    }
}
