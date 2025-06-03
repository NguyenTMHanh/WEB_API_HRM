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
    public class EmployeeController : Controller
    {
        private readonly IEmployeeRepository _employeeRepository;

        public EmployeeController(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        [HttpPost("CreatePersonal")]
        [Authorize(Policy = "CanCreateEmployees")]
        public async Task<IActionResult> CreatePersonalEmployee([FromBody] CreatePersonalEmployeeDto model)
        {
            // Kiểm tra model có hợp lệ không (dựa trên các annotation trong DTO)
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();
                return BadRequest(new Response(
                    code: CustomCodes.InvalidRequest,
                    message: "Invalid request data.",
                    data: null,
                    errors: errors
                ));
            }

            // Gọi repository để tạo nhân viên
            var result = await _employeeRepository.CreatePersonalEmployeeAsync(model);

            // Kiểm tra kết quả từ IdentityResult
            if (result.Succeeded)
            {
                return Ok(new Response(
                    code: 0, // Thành công
                    message: "Employee created successfully.",
                    data: model,
                    errors: new List<string>()
                ));
            }

            // Thu thập tất cả lỗi từ IdentityResult
            var errorList = result.Errors.Select(e => e.Description).ToList();
            if (!errorList.Any())
            {
                return BadRequest(new Response(
                    code: CustomCodes.InvalidRequest,
                    message: "An unknown error occurred.",
                    data: null,
                    errors: new List<string> { "An unknown error occurred." }
                ));
            }

            // Ánh xạ các lỗi từ repository sang CustomCodes
            var firstError = result.Errors.First();
            switch (firstError.Code)
            {
                case "InvalidEmail":
                    return BadRequest(new Response(
                        code: CustomCodes.InvalidEmail,
                        message: "Email validation failed.",
                        data: null,
                        errors: errorList
                    ));
                case "DuplicateEmail":
                    return Conflict(new Response(
                        code: CustomCodes.DuplicateEmail,
                        message: "Email already exists.",
                        data: null,
                        errors: errorList
                    ));
                case "InvalidPhoneNumber":
                    return BadRequest(new Response(
                        code: CustomCodes.InvalidPhoneNumber,
                        message: "Phone number validation failed.",
                        data: null,
                        errors: errorList
                    ));
                case "DuplicatePhoneNumber":
                    return Conflict(new Response(
                        code: CustomCodes.DuplicatePhoneNumber,
                        message: "Phone number already exists.",
                        data: null,
                        errors: errorList
                    ));
                case "InvalidCCCD":
                    return BadRequest(new Response(
                        code: CustomCodes.InvalidIdentification,
                        message: "Citizen Identification Number validation failed.",
                        data: null,
                        errors: errorList
                    ));
                case "DuplicateCCCD":
                    return Conflict(new Response(
                        code: CustomCodes.DuplicateIdentification,
                        message: "Citizen Identification Number already exists.",
                        data: null,
                        errors: errorList
                    ));
                default:
                    return BadRequest(new Response(
                        code: CustomCodes.InvalidRequest,
                        message: "An error occurred while creating the employee.",
                        data: null,
                        errors: errorList
                    ));
            }
        }
    }
}