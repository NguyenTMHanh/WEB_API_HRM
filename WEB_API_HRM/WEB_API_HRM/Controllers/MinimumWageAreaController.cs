using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WEB_API_HRM.Models;
using WEB_API_HRM.Repositories;
using WEB_API_HRM.RSP;

namespace WEB_API_HRM.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MinimumWageAreaController : Controller
    {
        private IMinimumWageAreaRepository _minimumWageAreaRepository;
        public MinimumWageAreaController(IMinimumWageAreaRepository minimumWageAreaRepository)
        {
            _minimumWageAreaRepository = minimumWageAreaRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MinimumWageAreaModel>>> GetAllMinimumWageArea()
        {
            try
            {
                var minimumWageAreas = await _minimumWageAreaRepository.GetAllMinimumWageAreaAsync();
                return Ok(minimumWageAreas);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new Response(-1, "An error occurred while processing your request", errors: new List<string> { ex.Message }));
            }
        }


        [HttpPut()]
        [Authorize(Policy = "CanUpdateSettings")]
        public async Task<ActionResult> UpdateMinimumWageArea([FromBody] List<MinimumWageAreaModel> models)
        {
            try
            {
                var result = await _minimumWageAreaRepository.UpdateMinimumWageArea(models);
                return Ok(new Response(0, "minimum wage area updated successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new Response(-1, "An error occurred while processing your request", errors: new List<string> { ex.Message }));
            }
        }
    }
}
