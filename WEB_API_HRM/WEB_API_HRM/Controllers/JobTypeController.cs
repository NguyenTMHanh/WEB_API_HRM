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
    public class JobTypeController : Controller
    {
        private readonly IJobTypeRepository _jobTypeRepository;
        public JobTypeController (IJobTypeRepository jobTypeRepository)
        {
            _jobTypeRepository = jobTypeRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<JobTypeModel>>> GetAllJobTypes()
        {
            try
            {
                var jobtypes = await _jobTypeRepository.GetAllJobType();
                return Ok(jobtypes);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new Response(-1, "An error occurred while processing your request", errors: new List<string> { ex.Message }));
            }
        }


        [HttpPut()]
        [Authorize(Policy = "CanUpdateSettings")]
        public async Task<ActionResult> UpdateJobType([FromBody] List<JobTypeModel> jobtypes)
        {
            try
            {
                var result = await _jobTypeRepository.UpdateJobType(jobtypes);
                return Ok(new Response(0, "JobType updated successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new Response(-1, "An error occurred while processing your request", errors: new List<string> { ex.Message }));
            }
        }
    }
}
