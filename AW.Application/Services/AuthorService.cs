using AutoMapper;
using AutoMapper.QueryableExtensions;
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
using Microsoft.Extensions.Configuration;

namespace AW.Application.Services
{
    public class AuthorService: IAuthor
    {
        private const string EntityName = "Authors";

        private readonly DbSet<Author> _dbSet;
        private IUnitOfWork UnitOfWork { get; set; }
        private IMapper MapperEngine { get; set; }

        public AuthorService(IMapper mapper, IHttpContextAccessor httpContextAccessor
            , IHostingEnvironment hostingEnvironment, ILogger<ApplicationDbContextBase> logger, IConfigurationRoot configuration)
        { 
            MapperEngine = mapper;
            UnitOfWork = new ApplicationDbContext(httpContextAccessor, hostingEnvironment, logger,configuration);
            _dbSet = UnitOfWork.Set<Author>();
        }


        public async Task<ServiceResult<int>> AddAsync(AuthorDto data, int id = 0)
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
                aut.Id = id;
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

        public ServiceResult<AuthorDto> GetById(int id)
        {
            var query = _dbSet.Where(a => a.Id == id).ProjectTo<AuthorDto>(MapperEngine).FirstOrDefault(); 
            return ServiceResult<AuthorDto>.Success(query);
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
