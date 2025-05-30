using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WEB_API_HRM.Helpers;
using WEB_API_HRM.Models;
using WEB_API_HRM.Repositories;
using WEB_API_HRM.RSP;

namespace WEB_API_HRM.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class DeductionLevelController : Controller
    {
        private readonly IDeductionLevelRepository _deductionLevelRepository;
        public DeductionLevelController(IDeductionLevelRepository denductionLevelRepository)
        {
            _deductionLevelRepository = denductionLevelRepository;
        }

        [HttpGet]
        public async Task<ActionResult<DeductionLevelModel>> GetDeductionLevel()
        {
            try
            {
                var setting = await _deductionLevelRepository.GetDeductionLevelAsync();
                if (setting == null)
                {
                    return NotFound(new Response(CustomCodes.NotFound, "deduction level setting not found"));
                }
                return Ok(new Response(0, "deduction level setting retrieved successfully", data: setting));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new Response(-1, "An error occurred while processing your request", errors: new List<string> { ex.Message }));
            }
        }


        [HttpPut()]
        [Authorize(Policy = "CanUpdateSettings")]
        public async Task<ActionResult> UpdateDeductionLevel(DeductionLevelModel model)
        {
            try
            {
                var result = await _deductionLevelRepository.UpdateDeductionLevelAsync(model);
                var errors = result.Errors.Select(e => e.Description).ToList();
                if (!result.Succeeded)
                {
                    if (errors.Any(e => e.Contains("Deduction level setting not found.")))
                    {
                        return NotFound(new Response(CustomCodes.NotFound, "Deduction level setting update failed:Deduction level setting not found", errors: errors));
                    }
                    return BadRequest(new Response(CustomCodes.InvalidRequest, "Deduction level setting update failed", errors: errors));
                }
                return Ok(new Response(0, "Deduction level setting updated successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new Response(-1, "An error occurred while processing your request", errors: new List<string> { ex.Message }));
            }
        }
    }
}
