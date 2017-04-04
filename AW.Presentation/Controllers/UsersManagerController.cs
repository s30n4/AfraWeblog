using System.Data.SqlClient;
using System.Threading.Tasks;
using AW.Application.Dtos.Identity;
using AW.Application.Services.Contracts.Identity;
using AW.Application.Services.Identity;
using AW.Common.GuardToolkit;
using AW.Common.IdentityToolkit;
using AW.Common.WebToolkit;
using AW.Entities.Domain.Identity;
using DNTBreadCrumb.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AW.Presentation.Controllers
{
    [Authorize(Roles = ConstantRoles.Admin)]
    [BreadCrumb(Title = "مدیریت کاربران", UseDefaultRouteUrl = true, Order = 0)]
    public class UsersManagerController : Controller
    {
        private const int DefaultPageSize = 7;

        private readonly IApplicationRoleManager _roleManager;
        private readonly IApplicationUserManager _userManager;
        public UsersManagerController(
            IApplicationUserManager userManager,
            IApplicationRoleManager roleManager)
        {
            _userManager = userManager;
            _userManager.CheckArgumentIsNull(nameof(_userManager));

            _roleManager = roleManager;
            _roleManager.CheckArgumentIsNull(nameof(_roleManager));
        }

        [AjaxOnly, HttpPost, ValidateAntiForgeryToken]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<IActionResult> ActivateUserEmailStat(int userId)
        {
            User thisUser = null;
            var result = await _userManager.UpdateUserAndSecurityStampAsync(
                userId, user =>
                {
                    user.EmailConfirmed = true;
                    thisUser = user;
                }).ConfigureAwait(false);
            if (!result.Succeeded)
            {
                return BadRequest(error: result.DumpErrors(useHtmlNewLine: true));
            }

            return await ReturnUserCardPartialView(thisUser).ConfigureAwait(false);
        }

        [AjaxOnly, HttpPost, ValidateAntiForgeryToken]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<IActionResult> ChangeUserLockoutMode(int userId, bool activate)
        {
            User thisUser = null;
            var result = await _userManager.UpdateUserAndSecurityStampAsync(
                userId, user =>
                {
                    user.LockoutEnabled = activate;
                    thisUser = user;
                }).ConfigureAwait(false);
            if (!result.Succeeded)
            {
                return BadRequest(error: result.DumpErrors(useHtmlNewLine: true));
            }

            return await ReturnUserCardPartialView(thisUser).ConfigureAwait(false);
        }

        [AjaxOnly, HttpPost, ValidateAntiForgeryToken]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<IActionResult> ChangeUserRoles(int userId, int[] roleIds)
        {
            User thisUser = null;
            var result = await _userManager.AddOrUpdateUserRolesAsync(
                userId, roleIds, user => thisUser = user).ConfigureAwait(false);
            if (!result.Succeeded)
            {
                return BadRequest(error: result.DumpErrors(useHtmlNewLine: true));
            }

            return await ReturnUserCardPartialView(thisUser).ConfigureAwait(false);
        }

        [AjaxOnly, HttpPost, ValidateAntiForgeryToken]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<IActionResult> ChangeUserStat(int userId, bool activate)
        {
            User thisUser = null;
            var result = await _userManager.UpdateUserAndSecurityStampAsync(
                userId, user =>
                        {
                            user.IsActive = activate;
                            thisUser = user;
                        }).ConfigureAwait(false);
            if (!result.Succeeded)
            {
                return BadRequest(error: result.DumpErrors(useHtmlNewLine: true));
            }

            return await ReturnUserCardPartialView(thisUser).ConfigureAwait(false);
        }

        [AjaxOnly, HttpPost, ValidateAntiForgeryToken]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<IActionResult> ChangeUserTwoFactorAuthenticationStat(int userId, bool activate)
        {
            User thisUser = null;
            var result = await _userManager.UpdateUserAndSecurityStampAsync(
                userId, user =>
                {
                    user.TwoFactorEnabled = activate;
                    thisUser = user;
                }).ConfigureAwait(false);
            if (!result.Succeeded)
            {
                return BadRequest(error: result.DumpErrors(useHtmlNewLine: true));
            }

            return await ReturnUserCardPartialView(thisUser).ConfigureAwait(false);
        }

        [AjaxOnly, HttpPost, ValidateAntiForgeryToken]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<IActionResult> EndUserLockout(int userId)
        {
            User thisUser = null;
            var result = await _userManager.UpdateUserAndSecurityStampAsync(
                userId, user =>
                {
                    user.LockoutEnd = null;
                    thisUser = user;
                }).ConfigureAwait(false);
            if (!result.Succeeded)
            {
                return BadRequest(error: result.DumpErrors(useHtmlNewLine: true));
            }

            return await ReturnUserCardPartialView(thisUser).ConfigureAwait(false);
        }

        [BreadCrumb(Title = "ایندکس", Order = 1)]
        public async Task<IActionResult> Index(int? page = 1, string field = "Id", SortOrder order = SortOrder.Ascending)
        {
            var model = await _userManager.GetPagedUsersListAsync(
                pageNumber: page.Value - 1,
                recordsPerPage: DefaultPageSize,
                sortByField: field,
                sortOrder: order,
                showAllUsers: true).ConfigureAwait(false);

            model.Paging.CurrentPage = page.Value;
            model.Paging.ItemsPerPage = DefaultPageSize;
            model.Paging.ShowFirstLast = true;

            if (HttpContext.Request.IsAjaxRequest())
            {
                return PartialView("_UsersList", model);
            }
            return View(model);
        }

        [AjaxOnly, HttpPost, ValidateAntiForgeryToken]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<IActionResult> SearchUsers(SearchUsersViewModel model)
        {
            var pagedUsersList = await _userManager.GetPagedUsersListAsync(
                pageNumber: 0,
                model: model).ConfigureAwait(false);

            pagedUsersList.Paging.CurrentPage = 1;
            pagedUsersList.Paging.ItemsPerPage = model.MaxNumberOfRows;
            pagedUsersList.Paging.ShowFirstLast = true;

            model.PagedUsersList = pagedUsersList;
            return PartialView("_SearchUsers", model);
        }

        private async Task<IActionResult> ReturnUserCardPartialView(User thisUser)
        {
            var roles = await _roleManager.GetAllCustomRolesAsync().ConfigureAwait(false);
            return PartialView(@"~/Areas/Identity/Views/UserCard/_UserCardItem.cshtml",
                new UserCardItemViewModel
                {
                    User = thisUser,
                    ShowAdminParts = true,
                    Roles = roles,
                    ActiveTab = UserCardItemActiveTab.UserAdmin
                });
        }
    }
}