using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WEB_API_HRM.Models;
using WEB_API_HRM.Repositories;
using WEB_API_HRM.RSP;

namespace WEB_API_HRM.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AllowanceController : Controller
    {
        private readonly IAllowanceRepository _allowanceRepository;

        public AllowanceController(IAllowanceRepository allowanceRepository)
        {
            _allowanceRepository = allowanceRepository;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<AllowanceModel>>> GetAllAllowance()
        {
            try
            {
                var allowances = await _allowanceRepository.GetAllAllowanceAsync();
                return Ok(allowances);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new Response(-1, "An error occurred while processing your request", errors: new List<string> { ex.Message }));
            }
        }

        [HttpPut()]
        [Authorize(Policy = "CanUpdateSettings")]
        public async Task<ActionResult> UpdateAllowance([FromBody] List<AllowanceModel> allowances)
        {
            try
            {
                var result = await _allowanceRepository.UpdateAllowance(allowances);
                return Ok(new Response(0, "Allowance updated successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new Response(-1, "An error occurred while processing your request", errors: new List<string> { ex.Message }));
            }
        }
    }
}
