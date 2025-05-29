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
    public class CheckInOutSettingController : Controller
    {
        private readonly ICheckInOutSettingRepository _checkInOutSettingRepository;
        public CheckInOutSettingController (ICheckInOutSettingRepository checkInOutSettingRepository)
        {
            _checkInOutSettingRepository = checkInOutSettingRepository;
        }

        [HttpGet("GetCheckInOutTime")]
        [Authorize(Policy = "CanViewSettings")]
        public async Task<ActionResult<CheckInOutSetitngDto>> GetCheckInOutSetting()
        {
            try
            {
                var setting = await _checkInOutSettingRepository.GetCheckInOutSettingAsync();
                if (setting == null)
                {
                    return NotFound(new Response(CustomCodes.NotFound, "setting checkin checkout not found"));
                }
                return Ok(new Response(0, "setting checkin checkout retrieved successfully", data: setting));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new Response(-1, "An error occurred while processing your request", errors: new List<string> { ex.Message }));
            }
        }

        [HttpGet("GetBreakTime")]
        [Authorize(Policy = "CanViewSettings")]
        public async Task<ActionResult<BreakTimeDto>> GetBreakTimeSetting()
        {
            try
            {
                var setting = await _checkInOutSettingRepository.GetBreakTimeAsync();
                if (setting == null)
                {
                    return NotFound(new Response(CustomCodes.NotFound, "setting break time not found"));
                }
                return Ok(new Response(0, "setting break time retrieved successfully", data: setting));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new Response(-1, "An error occurred while processing your request", errors: new List<string> { ex.Message }));
            }
        }

        [HttpPut("UpdateCheckInOutTime")]
        [Authorize(Policy = "CanUpdateSettings")]
        public async Task<ActionResult> UpdateCheckInOut(CheckInOutSetitngDto model)
        {
            try
            {
                var result = await _checkInOutSettingRepository.UpdateCheckInOutSettingAsync(model);
                var errors = result.Errors.Select(e => e.Description).ToList();
                if (!result.Succeeded)
                {
                    if (errors.Any(e => e.Contains("CheckIn CheckOut setting not found.")))
                    {
                        return NotFound(new Response(CustomCodes.NotFound, "CheckIn CheckOut setting update failed: CheckIn CheckOut setting not found", errors: errors));
                    }
                    return BadRequest(new Response(CustomCodes.InvalidRequest, "CheckIn CheckOut setting update failed", errors: errors));
                }
                return Ok(new Response(0, "CheckIn CheckOut setting updated successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new Response(-1, "An error occurred while processing your request", errors: new List<string> { ex.Message }));
            }
        }

        [HttpPut("UpdateBreakTime")]
        [Authorize(Policy = "CanUpdateSettings")]
        public async Task<ActionResult> UpdateBreakTime(BreakTimeDto model)
        {
            try
            {
                var result = await _checkInOutSettingRepository.UpdateBreakTimeSettingAsync(model);
                var errors = result.Errors.Select(e => e.Description).ToList();
                if (!result.Succeeded)
                {
                    if (errors.Any(e => e.Contains("Break time setting not found.")))
                    {
                        return NotFound(new Response(CustomCodes.NotFound, "Break time setting update failed:Break time setting not found", errors: errors));
                    }
                    return BadRequest(new Response(CustomCodes.InvalidRequest, "Break time setting update failed", errors: errors));
                }
                return Ok(new Response(0, "Break time setting updated successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new Response(-1, "An error occurred while processing your request", errors: new List<string> { ex.Message }));
            }
        }
    }
}
