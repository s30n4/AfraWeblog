using AW.DataLayer.Mappings;
using AW.DataLayer.Settings;
using AW.Entities.AuditableEntity;
using AW.Entities.Domain;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace AW.DataLayer.Context
{
 
    public class ApplicationDbContext : ApplicationDbContextBase
    {
        public ApplicationDbContext(
          
            IHttpContextAccessor httpContextAccessor,
            IHostingEnvironment hostingEnvironment,
            ILogger<ApplicationDbContextBase> logger, IConfigurationRoot configuration  ,  IOptionsSnapshot<SiteSettings> siteSettings)
            : base( httpContextAccessor, hostingEnvironment, logger, configuration,siteSettings)
        {
        }

        public virtual DbSet<Author> Authors { get; set; }
        public virtual DbSet<Comment> Comments { get; set; }
        public virtual DbSet<Label> Labels { get; set; }
        public virtual DbSet<Link> Links { get; set; } 
        public virtual DbSet<NewsCategory> NewsCategories { get; set; } 
        public virtual DbSet<NewsContent> NewsContents { get; set; } 
        public virtual DbSet<NewsLabel> NewsLabels { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            // it should be placed here, otherwise it will rewrite the following settings!
            base.OnModelCreating(builder);

            // Adds all of the ASP.NET Core Identity related mappings at once.
            builder.AddCustomIdentityMappings(SiteSettings.Value);

       
            // This should be placed here, at the end.
            builder.AddAuditableShadowProperties();
        }
    }
}