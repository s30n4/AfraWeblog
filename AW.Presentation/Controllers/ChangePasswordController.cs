using System;
using System.Threading.Tasks;
using AW.Application.Dtos.Identity;
using AW.Application.Dtos.Identity.Emails;
using AW.Application.Services.Contracts.Identity;
using AW.Common.GuardToolkit;
using AW.Common.IdentityToolkit;
using AW.Common.WebToolkit;
using AW.DataLayer.Settings;
using AW.Entities.Domain.Identity;
using DNTBreadCrumb.Core;
using DNTPersianUtils.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace AW.Presentation.Controllers
{
    [Authorize]
    [BreadCrumb(Title = "تغییر کلمه‌ی عبور", UseDefaultRouteUrl = true, Order = 0)]
    public class ChangePasswordController: Controller
    {
        private readonly IEmailSender _emailSender;
        private readonly IApplicationUserManager _userManager;
        private readonly IApplicationSignInManager _signInManager;
        private readonly IPasswordValidator<User> _passwordValidator;
        private readonly IUsedPasswordsService _usedPasswordsService;
        private readonly IOptionsSnapshot<SiteSettings> _siteOptions;

        public ChangePasswordController(
            IApplicationUserManager userManager,
            IApplicationSignInManager signInManager,
            IEmailSender emailSender,
            IPasswordValidator<User> passwordValidator,
            IUsedPasswordsService usedPasswordsService,
            IOptionsSnapshot<SiteSettings> siteOptions)
        {
            _userManager = userManager;
            _userManager.CheckArgumentIsNull(nameof(_userManager));

            _signInManager = signInManager;
            _signInManager.CheckArgumentIsNull(nameof(_signInManager));

            _passwordValidator = passwordValidator;
            _passwordValidator.CheckArgumentIsNull(nameof(_passwordValidator));

            _usedPasswordsService = usedPasswordsService;
            _usedPasswordsService.CheckArgumentIsNull(nameof(_usedPasswordsService));

            _emailSender = emailSender;
            _emailSender.CheckArgumentIsNull(nameof(_emailSender));

            _siteOptions = siteOptions;
            _siteOptions.CheckArgumentIsNull(nameof(_siteOptions));
        }

        [BreadCrumb(Title = "ایندکس", Order = 1)]
        public async Task<IActionResult> Index()
        {
            var userId = this.User.Identity.GetUserId<int>();
            var passwordChangeDate = await _usedPasswordsService.GetLastUserPasswordChangeDateAsync(userId).ConfigureAwait(false);
            return View(new ChangePasswordViewModel
            {
                LastUserPasswordChangeDate = passwordChangeDate
            });
        }

        /// <summary>
        /// For [Remote] validation
        /// </summary>
        [AjaxOnly, HttpPost, ValidateAntiForgeryToken]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<IActionResult> ValidatePassword(string newPassword)
        {
            var user = await _userManager.GetCurrentUserAsync().ConfigureAwait(false);
            var result = await _passwordValidator.ValidateAsync(
                (UserManager<User>)_userManager, user, newPassword).ConfigureAwait(false);
            return Json(result.Succeeded ? "true" : result.DumpErrors(true));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.GetCurrentUserAsync().ConfigureAwait(false);
            if (user == null)
            {
                return View("NotFound");
            }

            var result = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword).ConfigureAwait(false);
            if (result.Succeeded)
            {
                await _userManager.UpdateSecurityStampAsync(user).ConfigureAwait(false);

                // reflect the changes in the Identity cookie
                await _signInManager.RefreshSignInAsync(user).ConfigureAwait(false);

                await _emailSender.SendEmailAsync(
                           user.Email,
                           "اطلاع رسانی تغییر کلمه‌ی عبور",
                           "~/Areas/Identity/Views/EmailTemplates/_ChangePasswordNotification.cshtml",
                           new ChangePasswordNotificationViewModel
                           {
                               User = user,
                               EmailSignature = _siteOptions.Value.Smtp.FromName,
                               MessageDateTime = DateTime.UtcNow.ToLongPersianDateTimeString()
                           }).ConfigureAwait(false);

                return RedirectToAction(nameof(Index), "UserCard", new { id = user.Id });
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View(model);
        }
    }
}