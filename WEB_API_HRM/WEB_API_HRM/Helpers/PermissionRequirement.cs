using Microsoft.AspNetCore.Authorization;

namespace WEB_API_HRM.Helpers
{
    public class PermissionRequirement : IAuthorizationRequirement
    {
        public string ModuleId { get; }
        public string ActionId { get; }

        public PermissionRequirement(string moduleId, string actionId)
        {
            ModuleId = moduleId;
            ActionId = actionId;
        }
    }
}
