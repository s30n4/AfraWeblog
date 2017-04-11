using System;
using System.Security.Claims;
using System.Security.Principal;
using AW.Application.Services.Contracts.Identity;
using AW.Application.Services.Identity.Logger;
using AW.Common.GuardToolkit;
using AW.DataLayer.Context;
using AW.DataLayer.Settings;
using AW.Entities.Domain.Identity;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace AW.Application.Services.Identity
{
    public static class IdentityServicesRegistry
    {
        public const string EmailConfirmationTokenProviderName = "ConfirmEmail";

        /// <summary>
        /// Adds all of the ASP.NET Core Identity related services and configurations at once.
        /// </summary>
        public static void AddCustomIdentityServices(this IServiceCollection services)
        {
            var siteSettings = GetSiteSettings(services);

            SetTicketStore(services, siteSettings);

            AddDynamicPermissionsPolicy(services);

            services.AddScoped<IUnitOfWork, ApplicationDbContext>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IPrincipal>(provider =>
                provider.GetService<IHttpContextAccessor>()?.HttpContext?.User ?? ClaimsPrincipal.Current);

            services.AddScoped<ILookupNormalizer, CustomNormalizer>();
            services.AddScoped<UpperInvariantLookupNormalizer, CustomNormalizer>();

            services.AddScoped<ISecurityStampValidator, CustomSecurityStampValidator>();
            services.AddScoped<SecurityStampValidator<User>, CustomSecurityStampValidator>();

            services.AddScoped<IPasswordValidator<User>, CustomPasswordValidator>();
            services.AddScoped<PasswordValidator<User>, CustomPasswordValidator>();

            services.AddScoped<IUserValidator<User>, CustomUserValidator>();
            services.AddScoped<UserValidator<User>, CustomUserValidator>();

            services.AddScoped<IUserClaimsPrincipalFactory<User>, ApplicationClaimsPrincipalFactory>();
            services.AddScoped<UserClaimsPrincipalFactory<User, Role>, ApplicationClaimsPrincipalFactory>();

            services.AddScoped<IdentityErrorDescriber, CustomIdentityErrorDescriber>();

            services.AddScoped<IApplicationUserStore, ApplicationUserStore>();
            services.AddScoped<UserStore<User, Role, ApplicationDbContext, int, UserClaim, UserRole, UserLogin, UserToken, RoleClaim>, ApplicationUserStore>();

            services.AddScoped<IApplicationUserManager, ApplicationUserManager>();
            services.AddScoped<UserManager<User>, ApplicationUserManager>();

            services.AddScoped<IApplicationRoleManager, ApplicationRoleManager>();
            services.AddScoped<RoleManager<Role>, ApplicationRoleManager>();

            services.AddScoped<IApplicationSignInManager, ApplicationSignInManager>();
            services.AddScoped<SignInManager<User>, ApplicationSignInManager>();

            services.AddScoped<IApplicationRoleStore, ApplicationRoleStore>();
            services.AddScoped<RoleStore<Role, ApplicationDbContext, int, UserRole, RoleClaim>, ApplicationRoleStore>();

            services.AddScoped<IEmailSender, AuthMessageSender>();
            services.AddScoped<ISmsSender, AuthMessageSender>();

            services.AddConfirmEmailDataProtectorTokenOptions(siteSettings);

            services.AddScoped<IIdentityDbInitializer, IdentityDbInitializer>();
            services.AddScoped<IUsedPasswordsService, UsedPasswordsService>();
            services.AddScoped<ISiteStatService, SiteStatService>();
            services.AddScoped<IUsersPhotoService, UsersPhotoService>();
            services.AddScoped<ISecurityTrimmingService, SecurityTrimmingService>();
            services.AddScoped<IAppLogItemsService, AppLogItemsService>();

            services.AddIdentity<User, Role>(identityOptions =>
            {
                var provider = services.BuildServiceProvider();
                EnableIimmediateLogout(identityOptions);
                SetPasswordOptions(identityOptions.Password, siteSettings);
                SetSignInOptions(identityOptions.SignIn, siteSettings);
                SetUserOptions(identityOptions.User);
                SetCookieOptions(provider, identityOptions.Cookies, siteSettings);
                SetLockoutOptions(identityOptions.Lockout, siteSettings);
            }).AddUserStore<ApplicationUserStore>()
              .AddUserManager<ApplicationUserManager>()
              .AddRoleStore<ApplicationRoleStore>()
              .AddRoleManager<ApplicationRoleManager>()
              .AddSignInManager<ApplicationSignInManager>()
              .AddErrorDescriber<CustomIdentityErrorDescriber>()
              // You **cannot** use .AddEntityFrameworkStores() when you customize everything
              //.AddEntityFrameworkStores<ApplicationDbContext, int>()
              .AddDefaultTokenProviders()
              .AddTokenProvider<ConfirmEmailDataProtectorTokenProvider<User>>(EmailConfirmationTokenProviderName);
        }

        /// <summary>
        /// Adds all of the ASP.NET Core Identity related initializations at once.
        /// </summary>
        public static void UseCustomIdentityServices(this IApplicationBuilder app)
        {
            app.UseIdentity();

            var identityDbInitialize = app.ApplicationServices.GetService<IIdentityDbInitializer>();
            identityDbInitialize.Initialize();
            identityDbInitialize.SeedData();
        }

        private static void AddConfirmEmailDataProtectorTokenOptions(this IServiceCollection services, SiteSettings siteSettings)
        {
            services.Configure<IdentityOptions>(options =>
            {
                options.Tokens.EmailConfirmationTokenProvider = EmailConfirmationTokenProviderName;
            });

            services.Configure<ConfirmEmailDataProtectionTokenProviderOptions>(options =>
            {
                options.TokenLifespan = siteSettings.EmailConfirmationTokenProviderLifespan;
            });
        }

        private static void AddDynamicPermissionsPolicy(this IServiceCollection services)
        {
            services.AddScoped<IAuthorizationHandler, DynamicPermissionsAuthorizationHandler>();
            services.AddAuthorization(opts =>
            {
                opts.AddPolicy(
                    ConstantPolicies.DynamicPermission,
                    policy =>
                    {
                        policy.RequireAuthenticatedUser();
                        policy.Requirements.Add(new DynamicPermissionRequirement());
                    });
            });
        }

        private static void EnableIimmediateLogout(IdentityOptions identityOptions)
        {
            identityOptions.SecurityStampValidationInterval = TimeSpan.Zero;
            // enables immediate logout, after updating the user's stat.
        }

        private static SiteSettings GetSiteSettings(this IServiceCollection services)
        {
            var provider = services.BuildServiceProvider();
            var siteSettingsOptions = provider.GetService<IOptionsSnapshot<SiteSettings>>();
            siteSettingsOptions.CheckArgumentIsNull(nameof(siteSettingsOptions));
            var siteSettings = siteSettingsOptions.Value;
            siteSettings.CheckArgumentIsNull(nameof(siteSettings));
            return siteSettings;
        }

        private static void SetCookieOptions(IServiceProvider provider, IdentityCookieOptions identityOptionsCookies, SiteSettings siteSettings)
        {
            identityOptionsCookies.ApplicationCookie.CookieName = siteSettings.CookieOptions.CookieName;
            identityOptionsCookies.ApplicationCookie.CookieHttpOnly = true;
            identityOptionsCookies.ApplicationCookie.ExpireTimeSpan = siteSettings.CookieOptions.ExpireTimeSpan;
            identityOptionsCookies.ApplicationCookie.SlidingExpiration = siteSettings.CookieOptions.SlidingExpiration;
            identityOptionsCookies.ApplicationCookie.LoginPath = siteSettings.CookieOptions.LoginPath;
            identityOptionsCookies.ApplicationCookie.LogoutPath = siteSettings.CookieOptions.LogoutPath;
            identityOptionsCookies.ApplicationCookie.AccessDeniedPath = siteSettings.CookieOptions.AccessDeniedPath;
            identityOptionsCookies.ApplicationCookie.AutomaticChallenge = true; // For Policy-Based Authorization Handler
            identityOptionsCookies.ApplicationCookie.AutomaticAuthenticate = true;

            var ticketStore = provider.GetService<ITicketStore>();
            ticketStore.CheckArgumentIsNull(nameof(ticketStore));
            identityOptionsCookies.ApplicationCookie.SessionStore = ticketStore; // To manage large identity cookies
        }

        private static void SetLockoutOptions(LockoutOptions identityOptionsLockout, SiteSettings siteSettings)
        {
            identityOptionsLockout.AllowedForNewUsers = siteSettings.LockoutOptions.AllowedForNewUsers;
            identityOptionsLockout.DefaultLockoutTimeSpan = siteSettings.LockoutOptions.DefaultLockoutTimeSpan;
            identityOptionsLockout.MaxFailedAccessAttempts = siteSettings.LockoutOptions.MaxFailedAccessAttempts;
        }

        private static void SetPasswordOptions(PasswordOptions identityOptionsPassword, SiteSettings siteSettings)
        {
            identityOptionsPassword.RequireDigit = siteSettings.PasswordOptions.RequireDigit;
            identityOptionsPassword.RequireLowercase = siteSettings.PasswordOptions.RequireLowercase;
            identityOptionsPassword.RequireNonAlphanumeric = siteSettings.PasswordOptions.RequireNonAlphanumeric;
            identityOptionsPassword.RequireUppercase = siteSettings.PasswordOptions.RequireUppercase;
            identityOptionsPassword.RequiredLength = siteSettings.PasswordOptions.RequiredLength;
        }

        private static void SetSignInOptions(SignInOptions identityOptionsSignIn, SiteSettings siteSettings)
        {
            identityOptionsSignIn.RequireConfirmedEmail = siteSettings.EnableEmailConfirmation;
        }

        private static void SetTicketStore(this IServiceCollection services, SiteSettings siteSettings)
        {
            // To manage large identity cookies
            var cookieOptions = siteSettings.CookieOptions;
            if (cookieOptions.UseDistributedCacheTicketStore)
            {
                services.AddDistributedSqlServerCache(options =>
                {
                    var cacheOptions = cookieOptions.DistributedSqlServerCacheOptions;
                    var connectionString = string.IsNullOrWhiteSpace(cacheOptions.ConnectionString) ?
                            siteSettings.GetDbConnectionString(string.Empty) :
                            cacheOptions.ConnectionString;
                    options.ConnectionString = connectionString;
                    options.SchemaName = cacheOptions.SchemaName;
                    options.TableName = cacheOptions.TableName;
                });
                services.AddScoped<ITicketStore, DistributedCacheTicketStore>();
            }
            else
            {
                services.AddMemoryCache();
                services.AddScoped<ITicketStore, MemoryCacheTicketStore>();
            }
        }

        private static void SetUserOptions(UserOptions identityOptionsUser)
        {
            identityOptionsUser.RequireUniqueEmail = true;
        }
    }
}