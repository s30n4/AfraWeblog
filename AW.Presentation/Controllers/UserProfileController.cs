﻿using System;
using System.IO;
using System.Threading.Tasks;
using AW.Application.Dtos.Identity;
using AW.Application.Dtos.Identity.Emails;
using AW.Application.Dtos.Identity.Settings;
using AW.Application.Services.Contracts.Identity;
using AW.Application.Services.Identity;
using AW.Common.GuardToolkit;
using AW.Common.IdentityToolkit;
using AW.Common.WebToolkit;
using AW.Entities.Domain.Identity;
using DNTBreadCrumb.Core;
using DNTPersianUtils.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace AW.Presentation.Controllers
{
    [Authorize]
    [BreadCrumb(Title = "مشخصات کاربری", UseDefaultRouteUrl = true, Order = 0)]
    public class UserProfileController : Controller
    {
        private readonly IEmailSender _emailSender;
        private readonly IProtectionProviderService _protectionProviderService;
        private readonly IApplicationRoleManager _roleManager;
        private readonly IApplicationSignInManager _signInManager;
        private readonly IOptionsSnapshot<SiteSettings> _siteOptions;
        private readonly IUsedPasswordsService _usedPasswordsService;
        private readonly IApplicationUserManager _userManager;
        private readonly IUsersPhotoService _usersPhotoService;
        private readonly IUserValidator<User> _userValidator;
        private readonly ILogger<UserProfileController> _logger;

        public UserProfileController(
            IApplicationUserManager userManager,
            IApplicationRoleManager roleManager,
            IApplicationSignInManager signInManager,
            IProtectionProviderService protectionProviderService,
            IUserValidator<User> userValidator,
            IUsedPasswordsService usedPasswordsService,
            IUsersPhotoService usersPhotoService,
            IOptionsSnapshot<SiteSettings> siteOptions,
            IEmailSender emailSender,
            ILogger<UserProfileController> logger)
        {
            _userManager = userManager;
            _userManager.CheckArgumentIsNull(nameof(_userManager));

            _roleManager = roleManager;
            _roleManager.CheckArgumentIsNull(nameof(_roleManager));

            _signInManager = signInManager;
            _signInManager.CheckArgumentIsNull(nameof(_signInManager));

            _protectionProviderService = protectionProviderService;
            _protectionProviderService.CheckArgumentIsNull(nameof(_protectionProviderService));

            _userValidator = userValidator;
            _userValidator.CheckArgumentIsNull(nameof(_userValidator));

            _usedPasswordsService = usedPasswordsService;
            _usedPasswordsService.CheckArgumentIsNull(nameof(_usedPasswordsService));

            _usersPhotoService = usersPhotoService;
            _usersPhotoService.CheckArgumentIsNull(nameof(_usersPhotoService));

            _siteOptions = siteOptions;
            _siteOptions.CheckArgumentIsNull(nameof(_siteOptions));

            _emailSender = emailSender;
            _emailSender.CheckArgumentIsNull(nameof(_emailSender));

            _logger = logger;
            _logger.CheckArgumentIsNull(nameof(_logger));
        }

        [Authorize(Roles = ConstantRoles.Admin)]
        [BreadCrumb(Title = "ایندکس", Order = 1)]
        public async Task<IActionResult> AdminEdit(int? id)
        {
            if (!id.HasValue)
            {
                return View("Error");
            }

            var user = await _userManager.FindByIdAsync(id.ToString()).ConfigureAwait(false);
            return await RenderForm(user, isAdminEdit: true).ConfigureAwait(false);
        }

        [BreadCrumb(Title = "ایندکس", Order = 1)]
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetCurrentUserAsync().ConfigureAwait(false);
            return await RenderForm(user, isAdminEdit: false).ConfigureAwait(false);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(UserProfileViewModel model)
        {
            if (this.ModelState.IsValid)
            {
                var pid = _protectionProviderService.Decrypt(model.Pid);
                if (string.IsNullOrWhiteSpace(pid))
                {
                    return View("Error");
                }

                if (pid != _userManager.GetCurrentUserId() &&
                    !_roleManager.IsCurrentUserInRole(ConstantRoles.Admin))
                {
                    _logger.LogWarning($"سعی در دسترسی غیرمجاز به ویرایش اطلاعات کاربر {pid}");
                    return View("Error");
                }

                var user = await _userManager.FindByIdAsync(pid).ConfigureAwait(false);
                if (user == null)
                {
                    return View("NotFound");
                }

                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.IsEmailPublic = model.IsEmailPublic;
                user.TwoFactorEnabled = model.TwoFactorEnabled;
                user.Location = model.Location;

                UpdateUserBirthDate(model, user);

                if (!await UpdateUserName(model, user).ConfigureAwait(false))
                {
                    return View(viewName: nameof(Index), model: model);
                }

                if (!await UpdateUserAvatarImage(model, user).ConfigureAwait(false))
                {
                    return View(viewName: nameof(Index), model: model);
                }

                if (!await UpdateUserEmail(model, user).ConfigureAwait(false))
                {
                    return View(viewName: nameof(Index), model: model);
                }

                var updateResult = await _userManager.UpdateAsync(user).ConfigureAwait(false);
                if (updateResult.Succeeded)
                {
                    if (!model.IsAdminEdit)
                    {
                        // reflect the changes in the current user's Identity cookie
                        await _signInManager.RefreshSignInAsync(user).ConfigureAwait(false);
                    }

                    await _emailSender.SendEmailAsync(
                           email: user.Email,
                           subject: "اطلاع رسانی به روز رسانی مشخصات کاربری",
                           viewNameOrPath: "~/Areas/Identity/Views/EmailTemplates/_UserProfileUpdateNotification.cshtml",
                           model: new UserProfileUpdateNotificationViewModel
                           {
                               User = user,
                               EmailSignature = _siteOptions.Value.Smtp.FromName,
                               MessageDateTime = DateTime.UtcNow.ToLongPersianDateTimeString()
                           }).ConfigureAwait(false);

                    return RedirectToAction(nameof(Index), "UserCard", routeValues: new { id = user.Id });
                }

                ModelState.AddModelError("", updateResult.DumpErrors(useHtmlNewLine: true));
            }
            return View(viewName: nameof(Index), model: model);
        }

        /// <summary>
        /// For [Remote] validation
        /// </summary>
        [AjaxOnly, HttpPost, ValidateAntiForgeryToken]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<IActionResult> ValidateUsername(string username, string email, string pid)
        {
            pid = _protectionProviderService.Decrypt(pid);
            if (string.IsNullOrWhiteSpace(pid))
            {
                return Json("اطلاعات وارد شده معتبر نیست.");
            }

            var user = await _userManager.FindByIdAsync(pid).ConfigureAwait(false);
            user.UserName = username;
            user.Email = email;

            var result = await _userValidator.ValidateAsync((UserManager<User>)_userManager, user).ConfigureAwait(false);
            return Json(result.Succeeded ? "true" : result.DumpErrors(useHtmlNewLine: true));
        }

        private static void UpdateUserBirthDate(UserProfileViewModel model, User user)
        {
            if (model.DateOfBirthYear.HasValue &&
                model.DateOfBirthMonth.HasValue &&
                model.DateOfBirthDay.HasValue)
            {
                var date =
                    $"{model.DateOfBirthYear.Value.ToString()}/{model.DateOfBirthMonth.Value.ToString("00")}/{model.DateOfBirthDay.Value.ToString("00")}";
                user.BirthDate = date.ToGregorianDateTimeOffset();
            }
            else
            {
                user.BirthDate = null;
            }
        }

        private async Task<IActionResult> RenderForm(User user, bool isAdminEdit)
        {
            _usersPhotoService.SetUserDefaultPhoto(user);

            var userProfile = new UserProfileViewModel
            {
                IsAdminEdit = isAdminEdit,
                Email = user.Email,
                PhotoFileName = user.PhotoFileName,
                Location = user.Location,
                UserName = user.UserName,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Pid = _protectionProviderService.Encrypt(user.Id.ToString()),
                IsEmailPublic = user.IsEmailPublic,
                TwoFactorEnabled = user.TwoFactorEnabled,
                IsPasswordTooOld = await _usedPasswordsService.IsLastUserPasswordTooOldAsync(user.Id).ConfigureAwait(false)
            };

            if (user.BirthDate.HasValue)
            {
                var pDateParts = user.BirthDate.Value.ToPersianYearMonthDay(DateTimeOffsetPart.DateTime);
                userProfile.DateOfBirthYear = pDateParts.Item1;
                userProfile.DateOfBirthMonth = pDateParts.Item2;
                userProfile.DateOfBirthDay = pDateParts.Item3;
            }

            return View(viewName: nameof(Index), model: userProfile);
        }

        private async Task<bool> UpdateUserAvatarImage(UserProfileViewModel model, User user)
        {
            _usersPhotoService.SetUserDefaultPhoto(user);

            var photoFile = model.Photo;
            if (photoFile != null && photoFile.Length > 0)
            {
                var imageOptions = _siteOptions.Value.UserAvatarImageOptions;
                if (!photoFile.IsValidImageFile(maxWidth: imageOptions.MaxWidth, maxHeight: imageOptions.MaxHeight))
                {
                    this.ModelState.AddModelError("",
                        $"حداکثر اندازه تصویر قابل ارسال {imageOptions.MaxHeight} در {imageOptions.MaxWidth} پیکسل است");
                    model.PhotoFileName = user.PhotoFileName;
                    return false;
                }

                var uploadsRootFolder = _usersPhotoService.GetUsersAvatarsFolderPath();
                var photoFileName = $"{user.Id}{Path.GetExtension(photoFile.FileName)}";
                var filePath = Path.Combine(uploadsRootFolder, photoFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await photoFile.CopyToAsync(fileStream).ConfigureAwait(false);
                }
                user.PhotoFileName = photoFileName;
            }
            return true;
        }

        private async Task<bool> UpdateUserEmail(UserProfileViewModel model, User user)
        {
            if (user.Email != model.Email)
            {
                user.Email = model.Email;
                var userValidator =
                    await _userValidator.ValidateAsync((UserManager<User>)_userManager, user).ConfigureAwait(false);
                if (!userValidator.Succeeded)
                {
                    ModelState.AddModelError("", userValidator.DumpErrors(useHtmlNewLine: true));
                    return false;
                }

                user.EmailConfirmed = false;

                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user).ConfigureAwait(false);
                await _emailSender.SendEmailAsync(
                    email: user.Email,
                    subject: "لطفا اکانت خود را تائید کنید",
                    viewNameOrPath: "~/Areas/Identity/Views/EmailTemplates/_RegisterEmailConfirmation.cshtml",
                    model: new RegisterEmailConfirmationViewModel
                    {
                        User = user,
                        EmailConfirmationToken = code,
                        EmailSignature = _siteOptions.Value.Smtp.FromName,
                        MessageDateTime = DateTime.UtcNow.ToLongPersianDateTimeString()
                    }).ConfigureAwait(false);
            }

            return true;
        }

        private async Task<bool> UpdateUserName(UserProfileViewModel model, User user)
        {
            if (user.UserName != model.UserName)
            {
                user.UserName = model.UserName;
                var userValidator =
                    await _userValidator.ValidateAsync((UserManager<User>)_userManager, user).ConfigureAwait(false);
                if (!userValidator.Succeeded)
                {
                    ModelState.AddModelError("", userValidator.DumpErrors(useHtmlNewLine: true));
                    return false;
                }
            }
            return true;
        }
    }
}