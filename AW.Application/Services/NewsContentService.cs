using AutoMapper;
using AutoMapper.QueryableExtensions;
using AW.Application.Dtos.NewsContent;
using AW.Application.Services.Contracts;
using AW.Common;
using AW.DataLayer.Context;
using AW.Entities.Domain;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AW.Application.Services
{
    public class NewsContentService: INewsContent
    {
        private const string EntityName = "NewsContents";

        private readonly DbSet<NewsContent> _dbSet;
        private IUnitOfWork UnitOfWork { get; set; }
        private IMapper MapperEngine { get; set; }

        public NewsContentService(IMapper mapper, IHttpContextAccessor httpContextAccessor
            , IHostingEnvironment hostingEnvironment, ILogger<ApplicationDbContextBase> logger)
        {
            MapperEngine = mapper;
            UnitOfWork = new ApplicationDbContext(httpContextAccessor, hostingEnvironment, logger);
            _dbSet = UnitOfWork.Set<NewsContent>();
        }

        public async Task<ServiceResult<int>> AddAsync(ContentAddDto data, int id = 0)
        {
            if (data == null) return ServiceResult<int>.Failed(new ServiceMessage { Description = "data is null" });
            NewsContent content;
            if (id == 0)
            {
                content = MapperEngine.Map<NewsContent>(data);
                content.SubmitDate = DateTime.Now;
                _dbSet.Add(content);
            }
            else
            {
                content = _dbSet.SingleOrDefault(a => a.Id == id);
                if (content == null) return ServiceResult<int>.Failed(new ServiceMessage { Description = "id is incorrect" });

                Mapper.Map(data, content);
                content.Id = id;
                _dbSet.Update(content);
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

        public ServiceResult<ContentOutputDto> GetById(int id)
        {
            var query = _dbSet.Where(a => a.Id == id).ProjectTo<ContentOutputDto>(MapperEngine).FirstOrDefault();
            return ServiceResult<ContentOutputDto>.Success(query);
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
