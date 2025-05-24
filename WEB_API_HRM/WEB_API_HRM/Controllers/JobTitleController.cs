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
    public class JobTitleController : ControllerBase 
    {
        private readonly IJobTitleRepository _jobTitleRepository;

        public JobTitleController(IJobTitleRepository jobTitleRepository)
        {
            _jobTitleRepository = jobTitleRepository ?? throw new ArgumentNullException(nameof(jobTitleRepository));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<JobTitleDto>>> GetAllJobTiles()
        {
            try
            {
                var jobTitles = await _jobTitleRepository.GetAllJobsAsync();
                return Ok(jobTitles);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new Response(-1, "An error occurred while processing your request", errors: new List<string> { ex.Message }));
            }
        }

        [HttpGet("{id}")]
        [Authorize(Policy = "CanViewSettings")]
        public async Task<ActionResult<JobTitleDto>> GetJobTile(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest(new Response(CustomCodes.InvalidRequest, "Invalid job title ID"));
            }

            try
            {
                var jobTitle = await _jobTitleRepository.GetJobTitleAsync(id);
                if (jobTitle == null)
                {
                    return NotFound(new Response(CustomCodes.NotFound, "Job title not found"));
                }
                return Ok(new Response(0, "Job title retrieved successfully", data: jobTitle));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new Response(-1, "An error occurred while processing your request", errors: new List<string> { ex.Message }));
            }
        }

        [HttpPost]
        [Authorize(Policy = "CanCreateSettings")]
        public async Task<IActionResult> CreateJobTile([FromBody] JobTitleDto model)
        {
            if (model == null || !ModelState.IsValid)
            {
                return BadRequest(new Response(CustomCodes.InvalidRequest, "Invalid job title data"));
            }

            try
            {
                var result = await _jobTitleRepository.CreateJobTitleAsync(model);
                var errors = result.Errors.Select(e => e.Description).ToList();
                if (!result.Succeeded)
                {
                    if (errors.Any(e => e.Contains("JobTitle already exists in the system.")))
                    {
                        return BadRequest(new Response(CustomCodes.Exists, "Job title creation failed", errors: errors));
                    }
                    else if (errors.Any(e => e.Contains("Rank not found.")))
                    {
                        return BadRequest(new Response(CustomCodes.NotFound, "Job title creation failed: Rank not found", errors: errors));
                    }
                    else if (errors.Any(e => e.Contains("Role not found.")))
                    {
                        return BadRequest(new Response(CustomCodes.NotFound, "Job title creation failed: Role not found", errors: errors));
                    }
                    return BadRequest(new Response(CustomCodes.InvalidRequest, "Job title creation failed", errors: errors));
                }
                return Ok(new Response(0, "Job title created successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new Response(-1, "An error occurred while processing your request", errors: new List<string> { ex.Message }));
            }
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "CanUpdateSettings")]
        public async Task<ActionResult> UpdateJobTitle(string id, [FromBody] JobTitleDto model)
        {
            if (string.IsNullOrEmpty(id) || model == null || !ModelState.IsValid)
            {
                return BadRequest(new Response(CustomCodes.InvalidRequest, "Invalid job title data or ID"));
            }

            try
            {
                var result = await _jobTitleRepository.UpdateJobTitleAsync(id, model);
                var errors = result.Errors.Select(e => e.Description).ToList();
                if (!result.Succeeded)
                {
                    if (errors.Any(e => e.Contains("JobTitle not found")))
                    {
                        return NotFound(new Response(CustomCodes.NotFound, "Job title update failed: Job title not found", errors: errors));
                    }
                    else if (errors.Any(e => e.Contains("Rank not found.")))
                    {
                        return BadRequest(new Response(CustomCodes.NotFound, "Job title update failed: Rank not found", errors: errors));
                    }
                    else if (errors.Any(e => e.Contains("Role not found.")))
                    {
                        return BadRequest(new Response(CustomCodes.NotFound, "Job title update failed: Role not found", errors: errors));
                    }
                    return BadRequest(new Response(CustomCodes.InvalidRequest, "Job title update failed", errors: errors));
                }
                return Ok(new Response(0, "Job title updated successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new Response(-1, "An error occurred while processing your request", errors: new List<string> { ex.Message }));
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "CanDeleteSettings")]
        public async Task<ActionResult> DeleteJobTitle(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest(new Response(CustomCodes.InvalidRequest, "Invalid job title ID"));
            }

            try
            {
                var result = await _jobTitleRepository.DeleteJobTitleAsync(id);
                var errors = result.Errors.Select(e => e.Description).ToList();
                if (!result.Succeeded)
                {
                    if (errors.Any(e => e.Contains("Jobtitle not found")))
                    {
                        return NotFound(new Response(CustomCodes.NotFound, "Job title deletion failed: Job title not found", errors: errors));
                    }
                    return BadRequest(new Response(CustomCodes.InvalidRequest, "Job title deletion failed", errors: errors));
                }
                return Ok(new Response(0, "Job title deleted successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new Response(-1, "An error occurred while processing your request", errors: new List<string> { ex.Message }));
            }
        }

        [HttpGet("GetCodeJobTitle")]
        [Authorize(Policy = "CanCreateSettings")]
        public async Task<IActionResult> GetCodeJobTitle()
        {
            try
            {
                var jobTitleCode = await _jobTitleRepository.GetCodeJobTitle();
                return Ok(new Response(0, "Job title code retrieved successfully", data: jobTitleCode));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new Response(-1, "An error occurred while retrieving job title code", errors: new List<string> { ex.Message }));
            }
        }
    }
}