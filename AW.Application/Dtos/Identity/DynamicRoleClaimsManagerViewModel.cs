using System.Collections.Generic;
using AW.Common.WebToolkit;
using AW.Entities.Domain.Identity;

namespace AW.Application.Dtos.Identity
{
    public class DynamicRoleClaimsManagerViewModel
    {
        public string[] ActionIds { set; get; }

        public int RoleId { set; get; }

        public Role RoleIncludeRoleClaims { set; get; }

        public ICollection<MvcControllerViewModel> SecuredControllerActions { set; get; }
    }
}