using AutoMapper;
using AW.Application.Services;
using AW.Application.Services.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace AW.Presentation
{
    public class ServiceIoc
    {

        public static void Ioc(IServiceCollection services)
        {
            services.AddScoped<IHttpContextAccessor, HttpContextAccessor>();

            services.AddScoped<IAuthor, AuthorService>();
            services.AddScoped<INewsCategory, NewsCategoryService>();
            services.AddScoped<INewsContent, NewsContentService>();
            services.AddScoped<ILabel, LabelService>();
            services.AddScoped<ILink, LinkService>();
            services.AddScoped<IComment, CommentService>();

            Mapper.Initialize(x => { x.AddProfiles(ProjectAssemblies.ServiceLayer); });

            var mapper = Mapper.Instance; // configuration.CreateMapper();

            services.AddSingleton(typeof(IMapper), mapper);

            var serviceProvider = services.BuildServiceProvider();

      
        }
    }
}
