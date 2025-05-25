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
    public class PositionController : Controller
    {
        private readonly IPositionRepository _positionRepository;
        public PositionController(IPositionRepository positionRepository)
        {
            _positionRepository = positionRepository;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<PositionDto>>> GetAllPositions()
        {
            try
            {
                var positions = await _positionRepository.GetAllPositionsAsync();
                return Ok(positions);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new Response(-1, "An error occurred while processing your request", errors: new List<string> { ex.Message }));
            }
        }


        [HttpGet("{id}")]
        [Authorize(Policy = "CanViewSettings")]
        public async Task<ActionResult<PositionDto>> GetPosition(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest(new Response(CustomCodes.InvalidRequest, "Invalid position ID"));
            }

            try
            {
                var position = await _positionRepository.GetPositionAsync(id);
                if (position == null)
                {
                    return NotFound(new Response(CustomCodes.NotFound, "Position not found"));
                }
                return Ok(new Response(0, "Position retrieved successfully", data: position));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new Response(-1, "An error occurred while processing your request", errors: new List<string> { ex.Message }));
            }
        }

        [HttpPost]
        [Authorize(Policy = "CanCreateSettings")]
        public async Task<IActionResult> CreatePosition([FromBody] PositionDto model)
        {
            try
            {
                var result = await _positionRepository.CreatePositionAsync(model);
                var errors = result.Errors.Select(e => e.Description).ToList();
                if (!result.Succeeded)
                {
                    if (errors.Any(e => e.Contains("Position already exists in the system.")))
                    {
                        return BadRequest(new Response(CustomCodes.Exists, "Position creation failed", errors: errors));
                    }
                    else if (errors.Any(e => e.Contains("Department not found.")))
                    {
                        return BadRequest(new Response(CustomCodes.NotFound, "Position creation failed: Position not found", errors: errors));
                    }                   
                    return BadRequest(new Response(CustomCodes.InvalidRequest, "Position creation failed", errors: errors));
                }
                return Ok(new Response(0, "Position created successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new Response(-1, "An error occurred while processing your request", errors: new List<string> { ex.Message }));
            }
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "CanUpdateSettings")]
        public async Task<ActionResult> UpdatePosition(string id, [FromBody] PositionDto model)
        {
            try
            {
                var result = await _positionRepository.UpdatePositionAsync(id, model);
                var errors = result.Errors.Select(e => e.Description).ToList();
                if (!result.Succeeded)
                {
                    if (errors.Any(e => e.Contains("Position not found")))
                    {
                        return NotFound(new Response(CustomCodes.NotFound, "Position update failed: Position not found", errors: errors));
                    }
                    else if (errors.Any(e => e.Contains("Department not found.")))
                    {
                        return BadRequest(new Response(CustomCodes.NotFound, "Position update failed: Department not found", errors: errors));
                    } 
                    else if(errors.Any(e => e.Contains("Position already exists in the system.")))
                    {
                        return BadRequest(new Response(CustomCodes.Exists, "Position update failed: Position already exists in the system.", errors: errors));
                    }
                    return BadRequest(new Response(CustomCodes.InvalidRequest, "Position update failed", errors: errors));
                }
                return Ok(new Response(0, "Position updated successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new Response(-1, "An error occurred while processing your request", errors: new List<string> { ex.Message }));
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "CanDeleteSettings")]
        public async Task<ActionResult> DeletePosition(string id)
        {
            try
            {
                var result = await _positionRepository.DeletePositionAsync(id);
                var errors = result.Errors.Select(e => e.Description).ToList();
                if (!result.Succeeded)
                {
                    if (errors.Any(e => e.Contains("Position not found")))
                    {
                        return NotFound(new Response(CustomCodes.NotFound, "position deletion failed: position not found", errors: errors));
                    }
                    return BadRequest(new Response(CustomCodes.InvalidRequest, "position deletion failed", errors: errors));
                }
                return Ok(new Response(0, "position deleted successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new Response(-1, "An error occurred while processing your request", errors: new List<string> { ex.Message }));
            }
        }


        [HttpGet("GetCodePosition")]
        [Authorize(Policy = "CanCreateSettings")]
        public async Task<IActionResult> GetCodePosition()
        {
            try
            {
                var positionCode = await _positionRepository.GetCodePosition();
                return Ok(new Response(0, "position code retrieved successfully", data: positionCode));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new Response(-1, "An error occurred while retrieving position code", errors: new List<string> { ex.Message }));
            }
        }
    }
}
