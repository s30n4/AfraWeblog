using AutoMapper;
using AutoMapper.QueryableExtensions;
using AW.Application.Dtos.Link;
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

namespace AW.Application.Services
{
    public class LinkService: ILink
    {
        public static string EntityName { get; } = "Links";

        private readonly DbSet<Link> _dbSet;
        private IUnitOfWork UnitOfWork { get; set; }
        private IMapper MapperEngine { get; set; }

        public LinkService(IMapper mapper, IHttpContextAccessor httpContextAccessor
            , IHostingEnvironment hostingEnvironment, ILogger<ApplicationDbContextBase> logger)
        {
            MapperEngine = mapper;
            UnitOfWork = new ApplicationDbContext(httpContextAccessor, hostingEnvironment, logger);
            _dbSet = UnitOfWork.Set<Link>();
        }


        public async Task<ServiceResult<int>> AddAsync(LinkDto data, int id = 0)
        {
            if (data == null) return ServiceResult<int>.Failed(new ServiceMessage { Description = "data is null" });
            Link lin;
            if (id == 0)
            {
                lin = MapperEngine.Map<Link>(data);
                _dbSet.Add(lin);
            }
            else
            {
                lin = _dbSet.SingleOrDefault(a => a.Id == id);
                if (lin == null) return ServiceResult<int>.Failed(new ServiceMessage { Description = "id is incorrect" });

                Mapper.Map(data, lin);
                lin.Id = id;
                _dbSet.Update(lin);
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

        public ServiceResult<LinkDto> GetById(int id)
        {
            var query = _dbSet.Where(a => a.Id == id).ProjectTo<LinkDto>(MapperEngine).FirstOrDefault();
            return ServiceResult<LinkDto>.Success(query);
        }

        public ServiceResult<int> DeleteById(int id)
        {
            var res = 0;
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
