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
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace AW.Presentation.Controllers
{
    [AllowAnonymous]
    [BreadCrumb(Title = "ثبت نام", UseDefaultRouteUrl = true, Order = 0)]
    public class RegisterController : Controller
    {
        private readonly IEmailSender _emailSender;
        private readonly ILogger<RegisterController> _logger;
        private readonly IApplicationUserManager _userManager;
        private readonly IPasswordValidator<User> _passwordValidator;
        private readonly IUserValidator<User> _userValidator;
        private readonly IOptionsSnapshot<SiteSettings> _siteOptions;

        public RegisterController(
            IApplicationUserManager userManager,
            IPasswordValidator<User> passwordValidator,
            IUserValidator<User> userValidator,
            IEmailSender emailSender,
            IOptionsSnapshot<SiteSettings> siteOptions,
            ILogger<RegisterController> logger)
        {
            _userManager = userManager;
            _userManager.CheckArgumentIsNull(nameof(_userManager));

            _passwordValidator = passwordValidator;
            _passwordValidator.CheckArgumentIsNull(nameof(_passwordValidator));

            _userValidator = userValidator;
            _userValidator.CheckArgumentIsNull(nameof(_userValidator));

            _emailSender = emailSender;
            _emailSender.CheckArgumentIsNull(nameof(_emailSender));

            _logger = logger;
            _logger.CheckArgumentIsNull(nameof(_logger));

            _siteOptions = siteOptions;
            _siteOptions.CheckArgumentIsNull(nameof(_siteOptions));
        }

        /// <summary>
        /// For [Remote] validation
        /// </summary>
        [AjaxOnly, HttpPost, ValidateAntiForgeryToken]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<IActionResult> ValidateUsername(string username, string email)
        {
            var result = await _userValidator.ValidateAsync(
                (UserManager<User>)_userManager, new User { UserName = username, Email = email }).ConfigureAwait(false);
            return Json(result.Succeeded ? "true" : result.DumpErrors(true));
        }

        /// <summary>
        /// For [Remote] validation
        /// </summary>
        [AjaxOnly, HttpPost, ValidateAntiForgeryToken]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<IActionResult> ValidatePassword(string password, string username)
        {
            var result = await _passwordValidator.ValidateAsync(
                (UserManager<User>)_userManager, new User { UserName = username }, password).ConfigureAwait(false);
            return Json(result.Succeeded ? "true" : result.DumpErrors(true));
        }

        [BreadCrumb(Title = "تائید ایمیل", Order = 1)]
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return View("Error");
            }

            var user = await _userManager.FindByIdAsync(userId).ConfigureAwait(false);
            if (user == null)
            {
                return View("NotFound");
            }

            var result = await _userManager.ConfirmEmailAsync(user, code).ConfigureAwait(false);
            return View(result.Succeeded ? nameof(ConfirmEmail) : "Error");
        }

        [BreadCrumb(Title = "ایندکس", Order = 1)]
        public IActionResult Index()
        {
            return View();
        }

        [BreadCrumb(Title = "تائیدیه ایمیل", Order = 1)]
        public IActionResult ConfirmedRegisteration()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateDNTCaptcha(CaptchaGeneratorLanguage = DNTCaptcha.Core.Providers.Language.Persian)]
        public async Task<IActionResult> Index(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new User
                {
                    UserName = model.Username,
                    Email = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName
                };
                var result = await _userManager.CreateAsync(user, model.Password).ConfigureAwait(false);
                if (result.Succeeded)
                {
                    _logger.LogInformation(3, $"{user.UserName} created a new account with password.");

                    if (_siteOptions.Value.EnableEmailConfirmation)
                    {
                        var code = await _userManager.GenerateEmailConfirmationTokenAsync(user).ConfigureAwait(false);
                        //ControllerExtensions.ShortControllerName<RegisterController>(), //todo: use everywhere .................

                        await _emailSender.SendEmailAsync(
                           user.Email,
                           "لطفا اکانت خود را تائید کنید",
                           "~/Areas/Identity/Views/EmailTemplates/_RegisterEmailConfirmation.cshtml",
                           new RegisterEmailConfirmationViewModel
                           {
                               User = user,
                               EmailConfirmationToken = code,
                               EmailSignature = _siteOptions.Value.Smtp.FromName,
                               MessageDateTime = DateTime.UtcNow.ToLongPersianDateTimeString()
                           }).ConfigureAwait(false);

                        return RedirectToAction(nameof(ConfirmYourEmail));
                    }
                    return RedirectToAction(nameof(ConfirmedRegisteration));
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View(model);
        }

        [BreadCrumb(Title = "ایمیل خود را تائید کنید", Order = 1)]
        public IActionResult ConfirmYourEmail()
        {
            return View();
        }
    }
}