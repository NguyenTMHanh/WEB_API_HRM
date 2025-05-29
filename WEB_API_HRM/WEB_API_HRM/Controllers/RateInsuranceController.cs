using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WEB_API_HRM.DTO;
using WEB_API_HRM.Helpers;
using WEB_API_HRM.Models;
using WEB_API_HRM.Repositories;
using WEB_API_HRM.RSP;

namespace WEB_API_HRM.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RateInsuranceController : Controller
    {
        private readonly IRateInsuranceRepository _rateInsuranceRepository;
        public RateInsuranceController(IRateInsuranceRepository rateInsuranceRepository)
        {
            _rateInsuranceRepository = rateInsuranceRepository;
        }
        [HttpGet("GetRateInsurance")]
        [Authorize(Policy = "CanViewSettings")]
        public async Task<ActionResult<RateInsuranceModel>> GetRateInsuranceSetting()
        {
            try
            {
                var setting = await _rateInsuranceRepository.GetRateInsuranceAsync();
                if (setting == null)
                {
                    return NotFound(new Response(CustomCodes.NotFound, "rate insurance setting not found"));
                }
                return Ok(new Response(0, "rate insurance retrieved successfully", data: setting));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new Response(-1, "An error occurred while processing your request", errors: new List<string> { ex.Message }));
            }
        }


        [HttpPut("UpdateBHYTRate")]
        [Authorize(Policy = "CanUpdateSettings")]
        public async Task<ActionResult> UpdateBHYTRate(RateInsuranceModel model)
        {
            try
            {
                var result = await _rateInsuranceRepository.UpdateBHYTRateAsync(model);
                var errors = result.Errors.Select(e => e.Description).ToList();
                if (!result.Succeeded)
                {
                    if (errors.Any(e => e.Contains("Rate insurance setting not found.")))
                    {
                        return NotFound(new Response(CustomCodes.NotFound, "Rate insurance setting update failed:Rate insurance setting not found", errors: errors));
                    }
                    return BadRequest(new Response(CustomCodes.InvalidRequest, "Rate insurance setting update failed", errors: errors));
                }
                return Ok(new Response(0, "Rate insurance setting updated successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new Response(-1, "An error occurred while processing your request", errors: new List<string> { ex.Message }));
            }
        }


        [HttpPut("UpdateBHXHRate")]
        [Authorize(Policy = "CanUpdateSettings")]
        public async Task<ActionResult> UpdateBHXHRate(RateInsuranceModel model)
        {
            try
            {
                var result = await _rateInsuranceRepository.UpdateBHXHRateAsync(model);
                var errors = result.Errors.Select(e => e.Description).ToList();
                if (!result.Succeeded)
                {
                    if (errors.Any(e => e.Contains("Rate insurance setting not found.")))
                    {
                        return NotFound(new Response(CustomCodes.NotFound, "Rate insurance setting update failed:Rate insurance setting not found", errors: errors));
                    }
                    return BadRequest(new Response(CustomCodes.InvalidRequest, "Rate insurance setting update failed", errors: errors));
                }
                return Ok(new Response(0, "Rate insurance setting updated successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new Response(-1, "An error occurred while processing your request", errors: new List<string> { ex.Message }));
            }
        }


        [HttpPut("UpdateBHTNRate")]
        [Authorize(Policy = "CanUpdateSettings")]
        public async Task<ActionResult> UpdateBHTNRate(RateInsuranceModel model)
        {
            try
            {
                var result = await _rateInsuranceRepository.UpdateBHTNRateAsync(model);
                var errors = result.Errors.Select(e => e.Description).ToList();
                if (!result.Succeeded)
                {
                    if (errors.Any(e => e.Contains("Rate insurance setting not found.")))
                    {
                        return NotFound(new Response(CustomCodes.NotFound, "Rate insurance setting update failed:Rate insurance setting not found", errors: errors));
                    }
                    return BadRequest(new Response(CustomCodes.InvalidRequest, "Rate insurance setting update failed", errors: errors));
                }
                return Ok(new Response(0, "Rate insurance setting updated successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new Response(-1, "An error occurred while processing your request", errors: new List<string> { ex.Message }));
            }
        }
    }
}
