using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using AW.Common.GuardToolkit;
using AW.Common.PersianToolkit;
using AW.DataLayer.Settings;
using AW.Entities.AuditableEntity;
using AW.Entities.Domain.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace AW.DataLayer.Context
{
 
    public abstract class ApplicationDbContextBase : IdentityDbContext<User, Role, int, UserClaim, UserRole, UserLogin, RoleClaim, UserToken>,  IUnitOfWork
    {
        protected readonly IHostingEnvironment HostingEnvironment;
        protected readonly IHttpContextAccessor HttpContextAccessor;
        protected readonly ILogger<ApplicationDbContextBase> Logger;
        private readonly IConfigurationRoot _configuration;
        protected readonly IOptionsSnapshot<SiteSettings> SiteSettings;


        protected ApplicationDbContextBase(
           
            IHttpContextAccessor httpContextAccessor,
            IHostingEnvironment hostingEnvironment,
            ILogger<ApplicationDbContextBase> logger, IConfigurationRoot configuration, IOptionsSnapshot<SiteSettings> siteSettings)
        {
            SiteSettings = siteSettings;
            SiteSettings.CheckArgumentIsNull(nameof(SiteSettings));

            HttpContextAccessor = httpContextAccessor;
            HttpContextAccessor.CheckArgumentIsNull(nameof(HttpContextAccessor));

            HostingEnvironment = hostingEnvironment;
            HostingEnvironment.CheckArgumentIsNull(nameof(HostingEnvironment));

            Logger = logger;
            _configuration = configuration;
            Logger.CheckArgumentIsNull(nameof(Logger));
        }

       

        public void AddRange<TEntity>(IEnumerable<TEntity> entities) where TEntity : class
        {
            Set<TEntity>().AddRange(entities);
        }

        public void ExecuteSqlCommand(string query)
        {
            Database.ExecuteSqlCommand(query);
        }

        public void ExecuteSqlCommand(string query, params object[] parameters)
        {
            Database.ExecuteSqlCommand(query, parameters);
        }

        public T GetShadowPropertyValue<T>(object entity, string propertyName) where T : IConvertible
        {
            var value = Entry(entity).Property(propertyName).CurrentValue;
            return value != null
                ? (T)Convert.ChangeType(value, typeof(T), CultureInfo.InvariantCulture)
                : default(T);
        }

        public object GetShadowPropertyValue(object entity, string propertyName)
        {
            return Entry(entity).Property(propertyName).CurrentValue;
        }

        public void MarkAsChanged<TEntity>(TEntity entity) where TEntity : class
        {
            Update(entity);
        }

        public void RemoveRange<TEntity>(IEnumerable<TEntity> entities) where TEntity : class
        {
            Set<TEntity>().RemoveRange(entities);
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            ChangeTracker.DetectChanges();

            BeforeSaveTriggers();

            ChangeTracker.AutoDetectChangesEnabled = false; // for performance reasons, to avoid calling DetectChanges() again.
            var result = base.SaveChanges(acceptAllChangesOnSuccess);
            ChangeTracker.AutoDetectChangesEnabled = true;
            return result;
        }

        public override int SaveChanges()
        {
            ChangeTracker.DetectChanges();

            BeforeSaveTriggers();

            ChangeTracker.AutoDetectChangesEnabled = false; // for performance reasons, to avoid calling DetectChanges() again.
            var result = base.SaveChanges();
            ChangeTracker.AutoDetectChangesEnabled = true;
            return result;
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            ChangeTracker.DetectChanges();

            BeforeSaveTriggers();

            ChangeTracker.AutoDetectChangesEnabled = false; // for performance reasons, to avoid calling DetectChanges() again.
            var result = base.SaveChangesAsync(cancellationToken);
            ChangeTracker.AutoDetectChangesEnabled = true;
            return result;
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = new CancellationToken())
        {
            ChangeTracker.DetectChanges();

            BeforeSaveTriggers();

            ChangeTracker.AutoDetectChangesEnabled = false; // for performance reasons, to avoid calling DetectChanges() again.
            var result = base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
            ChangeTracker.AutoDetectChangesEnabled = true;
            return result;
        }

        protected void BeforeSaveTriggers()
        {
            ValidateEntities();
            SetShadowProperties();
            this.ApplyCorrectYeKe();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var siteSettingsValue = SiteSettings.Value;
            siteSettingsValue.CheckArgumentIsNull(nameof(siteSettingsValue));
            var connectionString = siteSettingsValue.GetDbConnectionString(HostingEnvironment.WebRootPath);

            switch (siteSettingsValue.ActiveDatabase)
            {
                case ActiveDatabase.InMemoryDatabase:
                    optionsBuilder.UseInMemoryDatabase();
                    break;

                case ActiveDatabase.LocalDb:
                case ActiveDatabase.SqlServer:
                    optionsBuilder.UseSqlServer(
                        connectionString
                        , serverDbContextOptionsBuilder =>
                        {
                            var minutes = (int)TimeSpan.FromMinutes(3).TotalSeconds;
                            serverDbContextOptionsBuilder.CommandTimeout(minutes);
                            serverDbContextOptionsBuilder.EnableRetryOnFailure();
                        });
                    break;

                default:
                    throw new NotSupportedException("Please set the ActiveDatabase in appsettings.json file.");
            }


        }

        protected void SetShadowProperties()
        {
            ChangeTracker.SetAuditableEntityPropertyValues(HttpContextAccessor);
        }

        protected void ValidateEntities()
        {
            var errors = this.GetValidationErrors();
            if (!string.IsNullOrWhiteSpace(errors))
            {
                Logger.LogError(errors);
                throw new InvalidOperationException(errors);
            }
        }

        public void MarkAsDeleted<TEntity>(TEntity entity) where TEntity : class
        {
            Entry(entity).State = EntityState.Deleted;
        }

      
    }
}