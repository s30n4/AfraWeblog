using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AW.Presentation
{
    public class ServiceIoc
    {

        public static void Ioc(IServiceCollection services)
        {
            services.AddScoped<IHttpContextAccessor, HttpContextAccessor>();
                        

            Mapper.Initialize(x => { x.AddProfiles(ProjectAssemblies.ServiceLayer); });

            var mapper = Mapper.Instance; // configuration.CreateMapper();

            services.AddSingleton(typeof(IMapper), mapper);

            var serviceProvider = services.BuildServiceProvider();

      
        }
    }
}
