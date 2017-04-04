using AutoMapper;
using AutoMapper.QueryableExtensions;
using AW.Application.Dtos.Label;
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
    public class LabelService: ILabel
    {
        public static string EntityName { get; } = "Labels";

        private readonly DbSet<Label> _dbSet;
        private IUnitOfWork UnitOfWork { get; set; }
        private IMapper MapperEngine { get; set; }

        public LabelService(IMapper mapper, IHttpContextAccessor httpContextAccessor
            , IHostingEnvironment hostingEnvironment, ILogger<ApplicationDbContextBase> logger)
        {
            MapperEngine = mapper;
            UnitOfWork = new ApplicationDbContext(httpContextAccessor, hostingEnvironment, logger);
            _dbSet = UnitOfWork.Set<Label>();
        }


        public async Task<ServiceResult<int>> AddAsync(LabelDto data, int id = 0)
        {
            if (data == null) return ServiceResult<int>.Failed(new ServiceMessage { Description = "data is null" });
            Label lab;
            if (id == 0)
            {
                lab = MapperEngine.Map<Label>(data);
                _dbSet.Add(lab);
            }
            else
            {
                lab = _dbSet.SingleOrDefault(a => a.Id == id);
                if (lab == null) return ServiceResult<int>.Failed(new ServiceMessage { Description = "id is incorrect" });

                Mapper.Map(data, lab);
                lab.Id = id;
                _dbSet.Update(lab);
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

        public ServiceResult<LabelDto> GetById(int id)
        {
            var query = _dbSet.Where(a => a.Id == id).ProjectTo<LabelDto>(MapperEngine).FirstOrDefault();
            return ServiceResult<LabelDto>.Success(query);
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
