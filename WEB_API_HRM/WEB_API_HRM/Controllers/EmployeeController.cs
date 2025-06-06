using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography.X509Certificates;
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
        [Authorize(Policy = "CanCreatePersonalEmployees")]
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


        [HttpPost("CreatePersonel")]
        [Authorize(Policy = "CanCreatePersonelEmployees")]
        public async Task<IActionResult> CreatePersonelEmployee([FromBody] CreatePersonelEmployeeDto model)
        {
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

            var result = await _employeeRepository.CreatePersonelEmployeeAsync(model);

            // Kiểm tra kết quả từ IdentityResult
            if (result.Succeeded)
            {
                return Ok(new Response(
                    code: 0,
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

            var firstError = result.Errors.First();
            switch (firstError.Code)
            {
                case "EmployeeNotFound":
                    return BadRequest(new Response(
                        code: CustomCodes.EmployeeNotFound,
                        message: "Employee not found. Please create personal employee first",
                        data: null,
                        errors: errorList
                    ));
                case "DuplicatePersonel":
                    return BadRequest(new Response(
                        code: CustomCodes.DuplicatePersonel,
                        message: "personel employee exist in system.",
                        data: null,
                        errors: errorList
                    ));
                case "BranchNotFound":
                    return BadRequest(new Response(
                        code: CustomCodes.BranchNotFound,
                        message: "Branch not found.",
                        data: null,
                        errors: errorList
                    ));
                case "DepartmentNotFound":
                    return BadRequest(new Response(
                        code: CustomCodes.DepartmentNotFound,
                        message: "Department not found.",
                        data: null,
                        errors: errorList
                    ));
                case "JobtitleNotFound":
                    return BadRequest(new Response(
                        code: CustomCodes.JobtitleNotFound,
                        message: "jobtitle not found.",
                        data: null,
                        errors: errorList
                    ));

                case "RankNotFound":
                    return BadRequest(new Response(
                        code: CustomCodes.RankNotFound,
                        message: "rank not found.",
                        data: null,
                        errors: errorList
                    ));
                case "PositionNotFound":
                    return BadRequest(new Response(
                        code: CustomCodes.PositionNotFound,
                        message: "position not found.",
                        data: null,
                        errors: errorList
                    ));

                case "ManagerNotFound":
                    return BadRequest(new Response(
                        code: CustomCodes.ManagerNotFound,
                        message: "manager not found.",
                        data: null,
                        errors: errorList
                    ));

                case "JobTypeNotFound":
                    return BadRequest(new Response(
                        code: CustomCodes.JobTypeNotFound,
                        message: "job type not found.",
                        data: null,
                        errors: errorList
                    ));
                case "DuplicateUser":
                    return BadRequest(new Response(
                        code: CustomCodes.UsernameExists,
                        message: "Username already exists in the system.",
                        data: null,
                        errors: errorList
                    ));

                case "InvalidEmail":
                    return BadRequest(new Response(
                        code: CustomCodes.InvalidEmail,
                        message: "Email format is invalid.",
                        data: null,
                        errors: errorList
                    ));


                case "RoleNotFound":
                    return BadRequest(new Response(
                        code: CustomCodes.RoleNotFound,
                        message: "Role not found.",
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

        [HttpGet("CodeNameEmployee")]
        public async Task<ActionResult<IEnumerable<CodeNameEmployeeRes>>> GetAllCodeNameEmployee()
        {
            try
            {
                var codeNameEmployees = await _employeeRepository.GetCodeNameEmployeeAsync();
                return Ok(codeNameEmployees);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response(-1, "An error occurred while processing your request.", errors: new List<string> { ex.Message }));
            }
        }

        [HttpGet("CodeNameManager")]
        public async Task<ActionResult<IEnumerable<CodeNameEmployeeRes>>> GetAllCodeNameManager(string employeeCode, string rankName)
        {
            try
            {
                var codeNameManagers = await _employeeRepository.GetCodeNameManagerAsync(employeeCode, rankName);
                return Ok(codeNameManagers);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response(-1, "An error occurred while processing your request.", errors: new List<string> { ex.Message }));
            }
        }

        [HttpGet("GetGenderBirth")]
        public async Task<ActionResult<GenderDayOfBirthRes>> GetGenderBirth(string employeeCode)
        {
            try
            {
                var result = await _employeeRepository.GetGenderDayOfBirthAsync(employeeCode);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response(-1, "An error occurred while processing your request.", errors: new List<string> { ex.Message }));
            }
        }

        [HttpGet("GetAccountDefault")]
        public async Task<ActionResult<AccountDefaultRes>> GetAccountDefault(string employeeCode)
        {
            try
            {
                var result = await _employeeRepository.GetAccountDefaultAsync(employeeCode);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response(-1, "An error occurred while processing your request.", errors: new List<string> { ex.Message }));
            }
        }

        [HttpGet("GetRoleEmployee")]
        public async Task<ActionResult<string>> GetRoleEmployee(string jontitleName)
        {
            try
            {
                var result = await _employeeRepository.GetRoleEmployee(jontitleName);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response(-1, "An error occurred while processing your request.", errors: new List<string> { ex.Message }));
            }
        }

        [HttpGet("CodeNameEmployeeUnContract")]
        public async Task<ActionResult<IEnumerable<CodeNameEmployeeRes>>> GetAllCodeNameEmployeeUnContract()
        {
            try
            {
                var codeNameEmployees = await _employeeRepository.GetCodeNameEmployeeUnContractAsync();
                return Ok(codeNameEmployees);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response(-1, "An error occurred while processing your request.", errors: new List<string> { ex.Message }));
            }
        }
        [HttpGet("GetCodeContract")]
        public async Task<ActionResult<string>> GetCodeContract(string employeeCode)
        {
            try
            {
                var codeContract = await _employeeRepository.GetCodeContect(employeeCode);
                return Ok(codeContract);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response(-1, "An error occurred while processing your request.", errors: new List<string> { ex.Message }));
            }
        }

        [HttpGet("GetPositionCoefficientEmployee")]
        public async Task<ActionResult<PositionCoeficientRes>> GetPositionCofficientEmployee(string employeeCode)
        {
            try
            {
                var result = await _employeeRepository.GetPositionCoeficient(employeeCode);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response(-1, "An error occurred while processing your request.", errors: new List<string> { ex.Message }));
            }
        }


        [HttpPost("CreateContract")]
        [Authorize(Policy = "CanCreateContractEmployees")]
        public async Task<IActionResult> CreateContractEmployee([FromBody] CreateContractEmployeeDto model)
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
            var result = await _employeeRepository.CreateContractEmployeeAsync(model);

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
                case "EmployeeNotFound":
                    return BadRequest(new Response(
                        code: CustomCodes.EmployeeNotFound,
                        message: "Employee not found. Please create personal employee first",
                        data: null,
                        errors: errorList
                    ));
                case "DuplicateContract":
                    return BadRequest(new Response(
                        code: CustomCodes.DuplicateContract,
                        message: "contract employee exist in system.",
                        data: null,
                        errors: errorList
                    ));
                case "PositionNotFound":
                    return BadRequest(new Response(
                        code: CustomCodes.PositionNotFound,
                        message: "position not found.",
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


        [HttpGet("GetRateInsurance")]
        public async Task<ActionResult<RateInsuranceModel>> GetRateInsurance()
        {
            try
            {
                var result = await _employeeRepository.GetRateInsuranceAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response(-1, "An error occurred while processing your request.", errors: new List<string> { ex.Message }));
            }
        }

        [HttpGet("CodeNameEmployeeUnInsurance")]
        public async Task<ActionResult<IEnumerable<CodeNameEmployeeRes>>> GetAllCodeNameEmployeeUnInsurance()
        {
            try
            {
                var codeNameEmployees = await _employeeRepository.GetCodeNameEmployeeUnInsuranceAsync();
                return Ok(codeNameEmployees);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response(-1, "An error occurred while processing your request.", errors: new List<string> { ex.Message }));
            }
        }


        [HttpPost("CreateInsurance")]
        [Authorize(Policy = "CanCreateInsuranceEmployees")]
        public async Task<IActionResult> CreateInsuranceEmployee([FromBody] CreateInsuranceDto model)
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
            var result = await _employeeRepository.CreateInsuranceEmployeeAsync(model);

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
                case "EmployeeNotFound":
                    return BadRequest(new Response(
                        code: CustomCodes.EmployeeNotFound,
                        message: "Employee not found. Please create personal employee first",
                        data: null,
                        errors: errorList
                    ));
                case "DuplicateInsurance":
                    return BadRequest(new Response(
                        code: CustomCodes.DuplicateInsurance,
                        message: "insurance employee exist in system.",
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

        [HttpPost("CreateTax")]
        [Authorize(Policy = "CanCreateTaxEmployees")]
        public async Task<IActionResult> CreateTaxEmployee([FromBody] CreateTaxDto model)
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
            var result = await _employeeRepository.CreateTaxEmployeeAsync(model);

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
                case "EmployeeNotFound":
                    return BadRequest(new Response(
                        code: CustomCodes.EmployeeNotFound,
                        message: "Employee not found. Please create personal employee first",
                        data: null,
                        errors: errorList
                    ));
                case "DuplicateTax":
                    return BadRequest(new Response(
                        code: CustomCodes.DuplicateTax,
                        message: "tax employee exist in system.",
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

        [HttpGet("CodeNameEmployeeUnTax")]
        public async Task<ActionResult<IEnumerable<CodeNameEmployeeRes>>> GetAllCodeNameEmployeeUnTax()
        {
            try
            {
                var codeNameEmployees = await _employeeRepository.GetCodeNameEmployeeUnTaxAsync();
                return Ok(codeNameEmployees);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response(-1, "An error occurred while processing your request.", errors: new List<string> { ex.Message }));
            }
        }

        [HttpGet("GetPersonalInformation")]
        public async Task<IActionResult> GetPersonalInformation(string employeeCode)
        {
            var result = await _employeeRepository.GetPersonalInformationAsync(employeeCode);
            if(result == null)
            {
                return BadRequest(new Response(
                       code: CustomCodes.EmployeeNotFound,
                       message: "Employee not found. Please create personal employee first",
                       data: null,
                       errors: new List<string>()
                   ));
            }
            return Ok(new Response(
                    code: 0, // Thành công
                    message: "Employee geted successfully.",
                    data: result,
                    errors: new List<string>()
            ));
        }


        [HttpGet("GetEmployeeCodeToUserId")]
        public async Task<IActionResult> GetEmployeeCodeToUserId(string userId)
        {
            try
            {
                var result = await _employeeRepository.GetEmployeeCodeToUsername(userId); // Await the result
                if (string.IsNullOrEmpty(result))
                {
                    return NotFound(new Response(
                        code: CustomCodes.EmployeeNotFound,
                        message: "User not found.",
                        data: null,
                        errors: new List<string>()
                    ));
                }
                return Ok(new Response(
                    code: 0, // Success
                    message: "Employee code retrieved successfully.",
                    data: result, // Now result is a string
                    errors: new List<string>()
                ));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response(
                    code: -1,
                    message: "An error occurred while processing your request.",
                    data: null,
                    errors: new List<string> { ex.Message }
                ));
            }
        }
    }
}