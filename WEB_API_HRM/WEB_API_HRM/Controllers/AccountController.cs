using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using WEB_API_HRM.Helpers;
using WEB_API_HRM.Models;
using WEB_API_HRM.Repositories;
using WEB_API_HRM.RSP;

namespace WEB_API_HRM.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountRepository accountRepo;

        public AccountController(IAccountRepository repo)
        {
            accountRepo = repo;
        }

        [HttpPost("signup")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> SignUp([FromBody] SignUpModel model)
        {
            //try
            //{
            //    var result = await accountRepo.SignUpAsync(model);
            //    if (result.Succeeded)
            //    {
            //        return Ok(result);
            //    }

            //    var errors = result.Errors.Select(e => e.Description).ToList();
            //    if (errors.Any(e => e.Contains("Username already exists in the system.")))
            //    {
            //        return BadRequest(new Response(CustomCodes.UsernameExists, "Registration failed", errors: errors));
            //    }
            //    else if (errors.Any(e => e.Contains("Email format is invalid")))
            //    {
            //        return BadRequest(new Response(CustomCodes.InvalidEmail, "Registration failed", errors: errors));
            //    }
            //    else if (errors.Any(e => e.Contains("Role not found")))
            //    {
            //        return BadRequest(new Response(CustomCodes.RoleNotFound, "Registration failed", errors: errors));
            //    }
            //    else
            //    {
            //        return BadRequest(new { Errors = result.Errors.Select(e => e.Description) });
            //    }
            //}
            //catch (Exception ex)
            //{
            //    return StatusCode(StatusCodes.Status500InternalServerError, new { Message = "An error occurred while processing your request.", Details = ex.Message });
            //}

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

            var result = await accountRepo.SignUpAsync(model);
            if (result.Succeeded)
            {
                return Ok(new Response(
                    code: 0, // Thành công
                    message: "sign up successfully.",
                    data: model,
                    errors: new List<string>()
                ));
            }

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

        [HttpPost("SignIn")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> SignIn([FromBody] SignInModel signInModel)
        {
            try
            {
                var result = await accountRepo.SignInAsync(signInModel);
                if (result == null)
                {
                    return Unauthorized(new Response(CustomCodes.InvalidCredentials, "Login failed", errors: new List<string> { "Invalid username or password." }));
                }
                return Ok(new Response(0, "Login successful", data: result));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response(-1, "An error occurred while processing your request.", errors: new List<string> { ex.Message }));
            }
        }

        [HttpPost("RenewToken")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> RenewToken([FromBody] LoginResponse model)
        {
            try
            {
                if (string.IsNullOrEmpty(model.AccessToken) || string.IsNullOrEmpty(model.RefreshToken))
                {
                    return BadRequest(new Response(CustomCodes.InvalidRequest, "Invalid request", errors: new List<string> { "Access token and refresh token are required" }));
                }

                var result = await accountRepo.RenewTokenAsync(model.AccessToken, model.RefreshToken);
                return Ok(new Response(0, "Token refreshed successfully", data: result));
            }
            catch (SecurityTokenException ex)
            {
                return Unauthorized(new Response(CustomCodes.InvalidToken, "Token refresh failed", errors: new List<string> { ex.Message }));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response(-1, "An error occurred while processing your request", errors: new List<string> { ex.Message }));
            }
        }

    }
}