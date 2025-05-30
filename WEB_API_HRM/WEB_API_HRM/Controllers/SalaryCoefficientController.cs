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
    public class SalaryCoefficientController : Controller
    {
        private readonly ISalaryCoefficientRepository _salaryCoefficientRepository;
        public SalaryCoefficientController(ISalaryCoefficientRepository salaryCoefficientRepository)
        {
            _salaryCoefficientRepository = salaryCoefficientRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<SalaryCoefficientDto>>> GetSalaryCoefficients()
        {
            try
            {
                var salaryCoefficients = await _salaryCoefficientRepository.GetAllSalaryCoefficientAsync();
                return Ok(salaryCoefficients);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new Response(-1, "An error occurred while processing your request", errors: new List<string> { ex.Message }));
            }
        }


        [HttpPut()]
        [Authorize(Policy = "CanUpdateSettings")]
        public async Task<ActionResult> UpdateSalaryCoefficient([FromBody] List<SalaryCoefficientDto> model)
        {
            try
            {
                var result = await _salaryCoefficientRepository.UpdateSalaryCoefficientAsync(model);
                var errors = result.Errors.Select(e => e.Description).ToList();
                if (!result.Succeeded)
                {
                    if (errors.Any(e => e.Contains("Position not found.")))
                    {
                        return NotFound(new Response(CustomCodes.NotFound, "Salary coefficient update failed: Position not found", errors: errors));
                    }
                }

                return Ok(new Response(0, "Salary coefficient updated successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new Response(-1, "An error occurred while processing your request", errors: new List<string> { ex.Message }));
            }
        }
    }
}
