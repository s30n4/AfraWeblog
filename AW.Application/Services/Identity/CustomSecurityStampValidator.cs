﻿using System;
using System.Threading.Tasks;
using AW.Application.Services.Contracts.Identity;
using AW.Common.GuardToolkit;
using AW.Entities.Domain.Identity;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace AW.Application.Services.Identity
{
    /// <summary>
    /// Keep track of on-line users
    /// </summary>
    public class CustomSecurityStampValidator : SecurityStampValidator<User>
    {
        private readonly IOptions<IdentityOptions> _options;
        private readonly IApplicationSignInManager _signInManager;
        private readonly ISiteStatService _siteStatService;

        public CustomSecurityStampValidator(
            IOptions<IdentityOptions> options,
            IApplicationSignInManager signInManager,
            ISiteStatService siteStatService)
            : base(options, (SignInManager<User>)signInManager)
        {
            _options = options;
            _options.CheckArgumentIsNull(nameof(_options));

            _signInManager = signInManager;
            _signInManager.CheckArgumentIsNull(nameof(_signInManager));

            _siteStatService = siteStatService;
            _siteStatService.CheckArgumentIsNull(nameof(_siteStatService));
        }

        public TimeSpan UpdateLastModifiedDate { get; set; } = TimeSpan.FromMinutes(2);

        public override async Task ValidateAsync(CookieValidatePrincipalContext context)
        {
            await base.ValidateAsync(context).ConfigureAwait(false);
            await UpdateUserLastVisitDateTimeAsync(context).ConfigureAwait(false);
        }

        private async Task UpdateUserLastVisitDateTimeAsync(CookieValidatePrincipalContext context)
        {
            var currentUtc = DateTimeOffset.UtcNow;
            if (context.Options?.SystemClock != null)
            {
                currentUtc = context.Options.SystemClock.UtcNow;
            }
            var issuedUtc = context.Properties.IssuedUtc;

            // Only validate if enough time has elapsed
            if (issuedUtc == null || context.Principal == null)
            {
                return;
            }

            var timeElapsed = currentUtc.Subtract(issuedUtc.Value);
            if (timeElapsed > UpdateLastModifiedDate)
            {
                await _siteStatService.UpdateUserLastVisitDateTimeAsync(context.Principal).ConfigureAwait(false);
            }
        }
    }
}