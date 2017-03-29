using AutoMapper;
using AW.Application.Dtos.Author;
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
    public class AuthorService: IAuthor
    {
        private const string EntityName = "Authors";

        private readonly DbSet<Author> _dbSet;
        private IUnitOfWork UnitOfWork { get; set; }
        private IMapper MapperEngine { get; set; }

        public AuthorService(IMapper mapper, IHttpContextAccessor httpContextAccessor
            , IHostingEnvironment hostingEnvironment, ILogger<ApplicationDbContextBase> logger)
        { 
            MapperEngine = mapper;
            UnitOfWork = new ApplicationDbContext(httpContextAccessor, hostingEnvironment, logger);
            _dbSet = UnitOfWork.Set<Author>();
        }


        public async Task<ServiceResult<int>> AddAuthor(AuthorDto data, int id = 0)
        {
            if (data == null) return ServiceResult<int>.Failed(new ServiceMessage {Description = "data is null"});
            Author aut;
            if (id == 0)
            {
                aut = MapperEngine.Map<Author>(data);
                _dbSet.Add(aut);                    
            }
            else
            {
                aut = _dbSet.SingleOrDefault(a => a.Id == id);
                if (aut == null) return ServiceResult<int>.Failed(new ServiceMessage { Description = "id is incorrect" });

                Mapper.Map(data, aut);
                _dbSet.Update(aut);
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

        //public ServiceResult<SystemOutputDto> GetById(int id)
        //{
        //    var query = _dbSet.Where(a => a.Id == id).ProjectTo<SystemDto>(MapperEngine).FirstOrDefault();

        //    var system = new SystemOutputDto
        //    {
        //        Data = query
        //    };

        //    system.Headers = SgeCore.RefineEntityHeader(system.Headers, EntityName, 1);
        //    return ServiceResult<SystemOutputDto>.Success(system);
        //}


    }
}
