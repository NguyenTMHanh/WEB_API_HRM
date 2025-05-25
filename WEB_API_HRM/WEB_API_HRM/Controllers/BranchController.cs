using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WEB_API_HRM.DTO;
using WEB_API_HRM.Helpers;
using WEB_API_HRM.Repositories;
using WEB_API_HRM.RSP;

namespace WEB_API_HRM.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BranchController : Controller
    {
        private readonly IBranchRepository _branchRepository;
        public BranchController (IBranchRepository branchRepository)
        {
            _branchRepository = branchRepository;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<BranchDto>>> GetAllBranchs()
        {
            try
            {
                var branchs = await _branchRepository.GetAllBranchesAsync();
                return Ok(branchs);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new Response(-1, "An error occurred while processing your request", errors: new List<string> { ex.Message }));
            }
        }
        [HttpGet("{id}")]
        [Authorize(Policy = "CanViewSettings")]
        public async Task<ActionResult<BranchDto>> GetBranch(string id)
        {
            try
            {
                var branch = await _branchRepository.GetBranchAsync(id);
                if (branch == null)
                {
                    return NotFound(new Response(CustomCodes.NotFound, "branch not found"));
                }
                return Ok(new Response(0, "branch retrieved successfully", data: branch));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new Response(-1, "An error occurred while processing your request", errors: new List<string> { ex.Message }));
            }
        }

        [HttpPost]
        [Authorize(Policy = "CanCreateSettings")]
        public async Task<IActionResult> CreateBranch([FromBody] BranchDto model)
        {
            try
            {
                var result = await _branchRepository.CreateBranchAsync(model);
                var errors = result.Errors.Select(e => e.Description).ToList();
                if (!result.Succeeded)
                {
                    if (errors.Any(e => e.Contains("Branch already exists in the system.")))
                    {
                        return BadRequest(new Response(CustomCodes.Exists, "Branch creation failed", errors: errors));
                    }
                    else if (errors.Any(e => e.Contains("Department not found.")))
                    {
                        return BadRequest(new Response(CustomCodes.NotFound, "Branch creation failed: Department not found", errors: errors));
                    }                   
                }
                return Ok(new Response(0, "Branch created successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new Response(-1, "An error occurred while processing your request", errors: new List<string> { ex.Message }));
            }
        }
        [HttpPut("{id}")]
        [Authorize(Policy = "CanUpdateSettings")]
        public async Task<ActionResult> UpdateBranch(string id, [FromBody] BranchDto model)
        {
            try
            {
                var result = await _branchRepository.UpdateBranchAsync(id, model);
                var errors = result.Errors.Select(e => e.Description).ToList();
                if (!result.Succeeded)
                {
                    if (errors.Any(e => e.Contains("Branch not found.")))
                    {
                        return NotFound(new Response(CustomCodes.NotFound, "Branch update failed: Branch not found", errors: errors));
                    }                                  
                    return BadRequest(new Response(CustomCodes.InvalidRequest, "branch update failed", errors: errors));
                }
                return Ok(new Response(0, "branch updated successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new Response(-1, "An error occurred while processing your request", errors: new List<string> { ex.Message }));
            }
        }
        [HttpDelete("{id}")]
        [Authorize(Policy = "CanDeleteSettings")]
        public async Task<ActionResult> DeleteBranch(string id)
        {
            try
            {
                var result = await _branchRepository.DeleteBranchAsync(id);
                var errors = result.Errors.Select(e => e.Description).ToList();
                if (!result.Succeeded)
                {
                    if (errors.Any(e => e.Contains("Branch not found")))
                    {
                        return NotFound(new Response(CustomCodes.NotFound, "Branch deletion failed: Branch not found", errors: errors));
                    }
                    return BadRequest(new Response(CustomCodes.InvalidRequest, "Branch deletion failed", errors: errors));
                }
                return Ok(new Response(0, "Branch deleted successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new Response(-1, "An error occurred while processing your request", errors: new List<string> { ex.Message }));
            }
        }
        [HttpGet("GetCodeBranch")]
        [Authorize(Policy = "CanCreateSettings")]
        public async Task<IActionResult> GetCodeBranch()
        {
            try
            {
                var branchCode = await _branchRepository.GetCodeBranch();
                return Ok(new Response(0, "branch code retrieved successfully", data: branchCode));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new Response(-1, "An error occurred while retrieving job title code", errors: new List<string> { ex.Message }));
            }
        }
    }
}
