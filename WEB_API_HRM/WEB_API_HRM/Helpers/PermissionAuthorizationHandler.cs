using Microsoft.AspNetCore.Authorization;
using WEB_API_HRM.Repositories;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;

namespace WEB_API_HRM.Helpers
{
    public class PermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirement>
    {
        private readonly IServiceProvider _serviceProvider;

        public PermissionAuthorizationHandler(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
        {
            var userId = context.User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return;
            }

            using (var scope = _serviceProvider.CreateScope())
            {
                var roleRepository = scope.ServiceProvider.GetRequiredService<IRoleRepository>();
                var permissions = await roleRepository.GetRoleUserAsync(userId);
                if (permissions == null)
                {
                    return;
                }

                foreach (var permission in permissions)
                {
                    var perm = permission as dynamic;
                    string moduleId = perm.ModuleId;
                    string actionId = perm.ActionId;


                    if (moduleId == "allModule" && actionId == "fullAuthority")
                    {
                        context.Succeed(requirement);
                        return;
                    }

                    if (moduleId == requirement.ModuleId && actionId == requirement.ActionId)
                    {
                        context.Succeed(requirement);
                        return;
                    }
                }
            }
        }
    }
}