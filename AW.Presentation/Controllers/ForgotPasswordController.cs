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
using DNTCaptcha.Core;
using DNTPersianUtils.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace AW.Presentation.Controllers
{
    [AllowAnonymous]
    [BreadCrumb(Title = "بازیابی کلمه‌ی عبور", UseDefaultRouteUrl = true, Order = 0)]
    public class ForgotPasswordController : Controller
    {
        private readonly IEmailSender _emailSender;
        private readonly IApplicationUserManager _userManager;
        private readonly IPasswordValidator<User> _passwordValidator;
        private readonly IOptionsSnapshot<SiteSettings> _siteOptions;

        public ForgotPasswordController(
            IApplicationUserManager userManager,
            IPasswordValidator<User> passwordValidator,
            IEmailSender emailSender,
            IOptionsSnapshot<SiteSettings> siteOptions)
        {
            _userManager = userManager;
            _userManager.CheckArgumentIsNull(nameof(_userManager));

            _passwordValidator = passwordValidator;
            _passwordValidator.CheckArgumentIsNull(nameof(_passwordValidator));

            _emailSender = emailSender;
            _emailSender.CheckArgumentIsNull(nameof(_emailSender));

            _siteOptions = siteOptions;
            _siteOptions.CheckArgumentIsNull(nameof(_siteOptions));
        }

        [BreadCrumb(Title = "تائید کلمه‌ی عبور فراموش شده", Order = 1)]
        public IActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        [BreadCrumb(Title = "ایندکس", Order = 1)]
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// For [Remote] validation
        /// </summary>
        [AjaxOnly, HttpPost, ValidateAntiForgeryToken]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<IActionResult> ValidatePassword(string password, string email)
        {
            var user = await _userManager.FindByEmailAsync(email).ConfigureAwait(false);
            if (user == null)
            {
                return Json("ایمیل وارد شده معتبر نیست.");
            }

            var result = await _passwordValidator.ValidateAsync(
                (UserManager<User>)_userManager, user, password).ConfigureAwait(false);
            return Json(result.Succeeded ? "true" : result.DumpErrors(true));
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateDNTCaptcha(CaptchaGeneratorLanguage = DNTCaptcha.Core.Providers.Language.Persian)]
        public async Task<IActionResult> Index(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email).ConfigureAwait(false);
                if (user == null || !await _userManager.IsEmailConfirmedAsync(user).ConfigureAwait(false))
                {
                    return View("Error");
                }

                var code = await _userManager.GeneratePasswordResetTokenAsync(user).ConfigureAwait(false);
                await _emailSender.SendEmailAsync(
                   model.Email,
                   "بازیابی کلمه‌ی عبور",
                   "~/Areas/Identity/Views/EmailTemplates/_PasswordReset.cshtml",
                   new PasswordResetViewModel
                    {
                        UserId = user.Id,
                        Token = code,
                        EmailSignature = _siteOptions.Value.Smtp.FromName,
                        MessageDateTime = DateTime.UtcNow.ToLongPersianDateTimeString()
                    })
                    .ConfigureAwait(false);

                return View("ForgotPasswordConfirmation");
            }
            return View(model);
        }

        [BreadCrumb(Title = "تغییر کلمه‌ی عبور", Order = 1)]
        public IActionResult ResetPassword(string code = null)
        {
            return code == null ? View("Error") : View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.FindByEmailAsync(model.Email).ConfigureAwait(false);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToAction(nameof(ResetPasswordConfirmation));
            }

            var result = await _userManager.ResetPasswordAsync(user, model.Code, model.Password).ConfigureAwait(false);
            if (result.Succeeded)
            {
                return RedirectToAction(nameof(ResetPasswordConfirmation));
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View();
        }

        [BreadCrumb(Title = "تائیدیه تغییر کلمه‌ی عبور", Order = 1)]
        public IActionResult ResetPasswordConfirmation()
        {
            return View();
        }
    }
}