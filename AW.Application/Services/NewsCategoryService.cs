using AutoMapper;
using AutoMapper.QueryableExtensions;
using AW.Application.Dtos.NewsCategory;
using AW.Application.Services.Contracts;
using AW.Common;
using AW.DataLayer.Context;
using AW.Entities.Domain;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;
using AW.DataLayer.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace AW.Application.Services
{
    public class NewsCategoryService: INewsCategory
    {
        public static string EntityName { get; } = "NewsCategories";

        private readonly DbSet<NewsCategory> _dbSet;
        private IUnitOfWork UnitOfWork { get; set; }
        private IMapper MapperEngine { get; set; }

        public NewsCategoryService(IMapper mapper, IHttpContextAccessor httpContextAccessor
            , IHostingEnvironment hostingEnvironment, ILogger<ApplicationDbContextBase> logger, IConfigurationRoot configuration, IOptionsSnapshot<SiteSettings> siteSettings)
        {
            MapperEngine = mapper;
            UnitOfWork = new ApplicationDbContext(httpContextAccessor, hostingEnvironment, logger,configuration,siteSettings);
            _dbSet = UnitOfWork.Set<NewsCategory>();
        }

        public async Task<ServiceResult<int>> AddAsync(NewsCategoryDto data, int id = 0)
        {
            if (data == null) return ServiceResult<int>.Failed(new ServiceMessage { Description = "data is null" });
            NewsCategory category;
            if (id == 0)
            {
                category = MapperEngine.Map<NewsCategory>(data);
                _dbSet.Add(category);
            }
            else
            {
                category = _dbSet.SingleOrDefault(a => a.Id == id);
                if (category == null) return ServiceResult<int>.Failed(new ServiceMessage { Description = "id is incorrect" });

                Mapper.Map(data, category);
                _dbSet.Update(category);
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

        public ServiceResult<NewsCategoryDto> GetById(int id)
        {
            var query = _dbSet.Where(a => a.Id == id).ProjectTo<NewsCategoryDto>(MapperEngine).FirstOrDefault();
            return ServiceResult<NewsCategoryDto>.Success(query);
        }

        public ServiceResult<int> DeleteById(int id)
        {
            var res = 0;
            var query = _dbSet.Where(a => a.Id == id).FirstOrDefault();
            if (query != null && query.Id!=1)
            {
                //Update to Uncategory
                _dbSet.Remove(query);
                res = query.Id;
            }
            return ServiceResult<int>.Success(res);
        }

    }
}
