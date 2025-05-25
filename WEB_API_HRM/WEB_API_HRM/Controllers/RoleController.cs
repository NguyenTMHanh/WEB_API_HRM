using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WEB_API_HRM.Data;
using WEB_API_HRM.DTO;
using WEB_API_HRM.Helpers;
using WEB_API_HRM.Models;
using WEB_API_HRM.Repositories;
using WEB_API_HRM.RSP;

namespace WEB_API_HRM.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly IRoleRepository _roleRepository;

        public RoleController(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }

        [HttpPost]
        [Authorize(Policy = "CanCreateRoles")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateRole([FromBody] CreateRoleDto model)
        {
            try
            {
                var role = new ApplicationRole
                {
                    Id = model.Id,
                    Name = model.Name,
                    NormalizedName = model.NormalizedName,
                    ConcurrencyStamp = model.ConcurrencyStamp,
                    Description = model.Description,
                    RoleModuleActions = model.RoleModuleActions?.Select(rma => new RoleModuleActionModel
                    {
                        ModuleId = rma.ModuleId,
                        ActionId = rma.ActionId,
                        RoleId = rma.RoleId
                    }).ToList() ?? new List<RoleModuleActionModel>()
                };

                var result = await _roleRepository.CreateRoleAsync(role);
                if (result.Succeeded)
                {
                    return Ok(new Response(0, "Role created successfully", data: role));
                }

                var errors = result.Errors.Select(e => e.Description).ToList();
                if (errors.Any(e => e.Contains("Role already exists")))
                {
                    return BadRequest(new Response(CustomCodes.RoleExists, "Role creation failed", errors: errors));
                }
                else if (errors.Any(e => e.Contains("ModuleId")))
                {
                    return BadRequest(new Response(CustomCodes.ModuleNotFound, "Role creation failed", errors: errors));
                }
                else if (errors.Any(e => e.Contains("ActionId")))
                {
                    return BadRequest(new Response(CustomCodes.ActionNotFound, "Role creation failed", errors: errors));
                }
                else
                {
                    return BadRequest(new Response(-1, "Role creation failed", errors: errors));
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response(-1, "An error occurred while processing your request.", errors: new List<string> { ex.Message }));
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllRoles()
        {
            var roles = await _roleRepository.GetAllRolesAsync();
            return Ok(roles);
        }

        [HttpGet("RoleUser/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetRole(string id)
        {
            var role = await _roleRepository.GetRoleUserAsync(id);
            if (role == null)
            {
                return NotFound(new Response(CustomCodes.RoleNotFound, "Role not found", errors: new List<string> { "No role data found for the user." }));
            }
            return Ok(new Response(0, "Role data retrieved successfully", data: role));
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "CanUpdateRoles")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateRole(string id, [FromBody] UpdateRoleDto model)
        {
            var role = await _roleRepository.GetRoleAsync(id);
            if (role == null)
            {
                return NotFound(new Response(CustomCodes.RoleNotFound, "Role not found"));
            }

            

            role.Name = model.Name;
            role.NormalizedName = model.NormalizedName;
            role.ConcurrencyStamp = model.ConcurrencyStamp;
            role.Description = model.Description;

            role.RoleModuleActions.Clear();

            if (model.RoleModuleActions != null)
            {
                role.RoleModuleActions = model.RoleModuleActions.Select(rma => new RoleModuleActionModel
                {
                    ModuleId = rma.ModuleId,
                    ActionId = rma.ActionId,
                    RoleId = id
                }).ToList();
            }

            var (result, affectedUserId) = await _roleRepository.UpdateRoleAsync(role);
            if (result.Succeeded)
            {
                return Ok(new Response(0, "Role updated successfully", data: new { AffectedUserId = affectedUserId }));
            }

            var errors = result.Errors.Select(e => e.Description).ToList();
            return BadRequest(new Response(-1, "Role update failed", errors: errors));
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "CanDeleteRoles")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteRole(string id)
        {
            var role = await _roleRepository.GetRoleAsync(id);
            if (role == null)
            {
                return NotFound(new Response(CustomCodes.RoleNotFound, "Role not found"));
            }

            var result = await _roleRepository.DeleteRoleAsync(id);
            if (result.Succeeded)
            {
                return Ok(new Response(0, "Role deleted successfully"));
            }

            var errors = result.Errors.Select(e => e.Description).ToList();
            return BadRequest(new Response(-1, "Role deletion failed", errors: errors));
        }
        [HttpGet("GetCodeRole")]
        [Authorize(Policy = "CanCreateRoles")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetCodeRole()
        {
            try
            {
                var roleCode = await _roleRepository.GetCodeRole();
                return Ok(new Response(0, "Role code retrieved successfully", data: roleCode));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response(-1, "An error occurred while retrieving role code.", errors: new List<string> { ex.Message }));
            }
        }
    }
}