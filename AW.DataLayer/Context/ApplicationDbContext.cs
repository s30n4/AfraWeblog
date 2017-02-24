using AW.Entities.AuditableEntity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace AW.DataLayer.Context
{
 
    public class ApplicationDbContext : ApplicationDbContextBase
    {
        public ApplicationDbContext(
          
            IHttpContextAccessor httpContextAccessor,
            IHostingEnvironment hostingEnvironment,
            ILogger<ApplicationDbContextBase> logger)
            : base( httpContextAccessor, hostingEnvironment, logger)
        {
        }

   

        protected override void OnModelCreating(ModelBuilder builder)
        {
            // it should be placed here, otherwise it will rewrite the following settings!
            base.OnModelCreating(builder);



            // This should be placed here, at the end.
            builder.AddAuditableShadowProperties();
        }
    }
}