using Microsoft.AspNetCore.Identity;
using WEB_API_HRM.Models;
using System.Collections.Generic;

namespace WEB_API_HRM.Data
{
    public class ApplicationRole : IdentityRole
    {
        public string Description { get; set; }

        public ICollection<RoleModuleActionModel> RoleModuleActions { get; set; }

        public ApplicationRole()
        {
            RoleModuleActions = new List<RoleModuleActionModel>();
        }

        public ApplicationRole(ApplicationRole role)
        {
            if (role == null) return;

            this.Id = role.Id;
            this.Name = role.Name;
            this.Description = role.Description;
            this.RoleModuleActions = role.RoleModuleActions?.ToList() ?? new List<RoleModuleActionModel>();
        }
    }
}