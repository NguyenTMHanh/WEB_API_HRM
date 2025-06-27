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
    public class BasicSettingSalaryController : Controller
    {
        private readonly IBasicSettingSalaryRepository _basicSettingSalaryRepository;
        public BasicSettingSalaryController(IBasicSettingSalaryRepository basicSettingSalaryRepository)
        {
            _basicSettingSalaryRepository = basicSettingSalaryRepository;
        }

        [HttpGet]
        public async Task<ActionResult<BasicSettingSalaryModel>> GetBasicSettingSalary()
        {
            try
            {
                var setting = await _basicSettingSalaryRepository.GetBasicSettingSalaryAsync();
                if (setting == null)
                {
                    return NotFound(new Response(CustomCodes.NotFound, "basic setting salary not found"));
                }
                return Ok(new Response(0, "basic setting salary retrieved successfully", data: setting));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new Response(-1, "An error occurred while processing your request", errors: new List<string> { ex.Message }));
            }
        }

        [HttpPut]
        [Authorize(Policy = "CanUpdateSettings")]
        public async Task<ActionResult> UpdateBasicSettingSalary(BasicSettingSalaryModel model)
        {
            try
            {
                var result = await _basicSettingSalaryRepository.UpdateBasicSettingSalary(model);
                var errors = result.Errors.Select(e => e.Description).ToList();
                if (!result.Succeeded)
                {
                    if (errors.Any(e => e.Contains("Basic setting salary not found.")))
                    {
                        return NotFound(new Response(CustomCodes.NotFound, "Basic setting salary update failed: Basic setting salary not found", errors: errors));
                    }
                    return BadRequest(new Response(CustomCodes.InvalidRequest, "Basic setting salary update failed", errors: errors));
                }
                return Ok(new Response(0, "Basic setting salary updated successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new Response(-1, "An error occurred while processing your request", errors: new List<string> { ex.Message }));
            }
        }
    }
}
