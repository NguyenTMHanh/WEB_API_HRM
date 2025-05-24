using Microsoft.AspNetCore.Mvc;
using WEB_API_HRM.Repositories;
using WEB_API_HRM.Models;
using WEB_API_HRM.Helpers;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using System.Collections.Generic;
using WEB_API_HRM.RSP;
using Microsoft.AspNetCore.Authorization;

namespace WEB_API_HRM.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RankController : ControllerBase
    {
        private readonly IRankRepository _rankRepository;

        public RankController(IRankRepository rankRepository)
        {
            _rankRepository = rankRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<RankModel>>> GetAllRanks()
        {
            try
            {
                var ranks = await _rankRepository.GetAllRanksAsync();
                return Ok(ranks);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response(-1, "An error occurred while processing your request.", errors: new List<string> { ex.Message }));
            }
        }

        [HttpGet("{id}")]
        [Authorize(Policy = "CanViewSettings")]
        public async Task<ActionResult<RankModel>> GetRank(string id)
        {
            try
            {
                var rank = await _rankRepository.GetRankAsync(id);
                if (rank == null)
                {
                    return NotFound(new { ErrorCode = CustomCodes.NotFound, Message = "Rank not found" });
                }
                return Ok(rank);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response(-1, "An error occurred while processing your request.", errors: new List<string> { ex.Message }));
            }
        }


        [HttpPost]
        [Authorize(Policy = "CanCreateSettings")]
        public async Task<IActionResult> CreateRank([FromBody] RankModel model)
        {
            try
            {
                var result = await _rankRepository.CreateRankAsync(model);
                var errors = result.Errors.Select(e => e.Description).ToList();
                if (errors.Any(e => e.Contains("Rank already exists in the system.")))
                {
                    return BadRequest(new Response(CustomCodes.Exists, "Role creation failed", errors: errors));
                }

                return Ok(new Response(0, "Rank created successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response(-1, "An error occurred while processing your request.", errors: new List<string> { ex.Message }));
            }
        }


        [HttpPut("{id}")]
        [Authorize(Policy = "CanUpdateSettings")]
        public async Task<ActionResult> UpdateRank(string id, [FromBody] RankModel model)
        {
            try
            {
                var result = await _rankRepository.UpdateRankAsync(id, model);
                var errors = result.Errors.Select(e => e.Description).ToList();
                if (errors.Any(e => e.Contains("Rank not found")))
                {
                    return BadRequest(new Response(CustomCodes.NotFound, "Rank update failed", errors: errors));
                }

                return Ok(new Response(0, "Rank updated successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response(-1, "An error occurred while processing your request.", errors: new List<string> { ex.Message }));
            }
        }


        [HttpDelete("{id}")]
        [Authorize(Policy = "CanDeleteSettings")]
        public async Task<ActionResult> DeleteRank(string id)
        {
            try
            {
                var result = await _rankRepository.DeleteRankAsync(id);
                var errors = result.Errors.Select(e => e.Description).ToList();
                if (errors.Any(e => e.Contains("Rank not found")))
                {
                    return BadRequest(new Response(CustomCodes.NotFound, "Rank deleted failed", errors: errors));
                }

                return Ok(new Response(0, "Rank deleted successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response(-1, "An error occurred while processing your request.", errors: new List<string> { ex.Message }));
            }
        }

        [HttpGet("GetCodeRank")]
        [Authorize(Policy = "CanCreateSettings")]
        public async Task<IActionResult> GetCodeRank()
        {
            try
            {
                var roleCode = await _rankRepository.GetCodeRank();
                return Ok(new Response(0, "Role code retrieved successfully", data: roleCode));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response(-1, "An error occurred while retrieving role code.", errors: new List<string> { ex.Message }));
            }
        }
    }
}