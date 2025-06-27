using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WEB_API_HRM.Helpers;
using WEB_API_HRM.Models;
using WEB_API_HRM.Repositories;
using WEB_API_HRM.RSP;

namespace WEB_API_HRM.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : Controller
    {
        private readonly IDepartmentRepository _departmentRepository;
        public DepartmentController(IDepartmentRepository departmentRepository)
        {
            _departmentRepository = departmentRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DepartmentModel>>> GetAllDepartments()
        {
            try
            {
                var departments = await _departmentRepository.GetAllDepartmentsAsync();
                return Ok(departments);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response(-1, "An error occurred while processing your request.", errors: new List<string> { ex.Message }));
            }
        }

        [HttpGet("{id}")]
        [Authorize(Policy = "CanViewSettings")]
        public async Task<ActionResult<DepartmentModel>> GetDepartment(string id)
        {
            try
            {
                var department = await _departmentRepository.GetDepartmentAsync(id);
                if (department == null)
                {
                    return NotFound(new { ErrorCode = CustomCodes.NotFound, Message = "Department not found" });
                }
                return Ok(department);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response(-1, "An error occurred while processing your request.", errors: new List<string> { ex.Message }));
            }
        }


        [HttpPost]
        [Authorize(Policy = "CanCreateSettings")]
        public async Task<IActionResult> CreateDepartment([FromBody] DepartmentModel model)
        {
            try
            {
                var result = await _departmentRepository.CreateDepartmentAsync(model);
                var errors = result.Errors.Select(e => e.Description).ToList();
                if (errors.Any(e => e.Contains("Department already exists in the system.")))
                {
                    return BadRequest(new Response(CustomCodes.Exists, "Department creation failed", errors: errors));
                }

                return Ok(new Response(0, "Department created successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response(-1, "An error occurred while processing your request.", errors: new List<string> { ex.Message }));
            }
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "CanUpdateSettings")]
        public async Task<ActionResult> UpdateDepartment(string id, [FromBody] DepartmentModel model)
        {
            try
            {
                var result = await _departmentRepository.UpdateDepartmentAsync(id, model);
                var errors = result.Errors.Select(e => e.Description).ToList();
                if (errors.Any(e => e.Contains("Department not found")))
                {
                    return BadRequest(new Response(CustomCodes.NotFound, "Department update failed", errors: errors));
                }
                else if (errors.Any(e => e.Contains("Department already exists in the system.")))
                {
                    return BadRequest(new Response(CustomCodes.Exists, "Department update failed", errors: errors));
                }
                return Ok(new Response(0, "Department updated successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response(-1, "An error occurred while processing your request.", errors: new List<string> { ex.Message }));
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "CanDeleteSettings")]
        public async Task<ActionResult> DeleteDepartment(string id)
        {
            try
            {
                var result = await _departmentRepository.DeleteDepartmentAsync(id);
                var errors = result.Errors.Select(e => e.Description).ToList();
                if (errors.Any(e => e.Contains("Department not found")))
                {
                    return BadRequest(new Response(CustomCodes.NotFound, "Department deleted failed", errors: errors));
                }

                return Ok(new Response(0, "Department deleted successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response(-1, "An error occurred while processing your request.", errors: new List<string> { ex.Message }));
            }
        }
        [HttpGet("GetCodeDepartment")]
        [Authorize(Policy = "CanCreateSettings")]
        public async Task<IActionResult> GetCodeDepartment()
        {
            try
            {
                var departmentCode = await _departmentRepository.GetCodeDepartment();
                return Ok(new Response(0, "Role code retrieved successfully", data: departmentCode));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response(-1, "An error occurred while retrieving role code.", errors: new List<string> { ex.Message }));
            }
        }
    }
}
