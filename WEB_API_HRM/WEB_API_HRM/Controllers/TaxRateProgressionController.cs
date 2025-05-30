using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WEB_API_HRM.Models;
using WEB_API_HRM.Repositories;
using WEB_API_HRM.RSP;

namespace WEB_API_HRM.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaxRateProgressionController : Controller
    {
        private readonly ITaxRateProgressionRepository _taxRateProgressionRepository;
        public TaxRateProgressionController(ITaxRateProgressionRepository taxRateProgressionRepository)
        {
            _taxRateProgressionRepository = taxRateProgressionRepository;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<TaxRateProgressionModel>>> GetAllTaxRateProgression()
        {
            try
            {
                var taxRateProgressions = await _taxRateProgressionRepository.GetAllTaxRateProgression();
                return Ok(taxRateProgressions);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new Response(-1, "An error occurred while processing your request", errors: new List<string> { ex.Message }));
            }
        }


        [HttpPut()]
        [Authorize(Policy = "CanUpdateSettings")]
        public async Task<ActionResult> UpdateHoliday([FromBody] List<TaxRateProgressionModel> taxRateProgressionModels)
        {
            try
            {
                var result = await _taxRateProgressionRepository.UpdateTaxRateProgression(taxRateProgressionModels);
                return Ok(new Response(0, "Tax rate progression updated successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new Response(-1, "An error occurred while processing your request", errors: new List<string> { ex.Message }));
            }
        }

    }
}
