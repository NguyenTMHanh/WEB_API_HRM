using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WEB_API_HRM.Data;
using WEB_API_HRM.DTO;
using WEB_API_HRM.Models;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WEB_API_HRM.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly HRMContext _context;

        public EmployeeRepository(HRMContext context)
        {
            _context = context;
        }

        public async Task<IdentityResult> CreatePersonalEmployeeAsync(CreatePersonalEmployeeDto model)
        {
            if (!IsValidEmail(model.Email))
            {
                return IdentityResult.Failed(new IdentityError
                {
                    Code = "InvalidEmail",
                    Description = "Email format is invalid."
                });
            }

            // Kiểm tra email đã tồn tại
            if (await _context.PersonalEmployees.AnyAsync(e => e.Email == model.Email))
            {
                return IdentityResult.Failed(new IdentityError
                {
                    Code = "DuplicateEmail",
                    Description = "Email is already in use."
                });
            }

            // Kiểm tra số điện thoại
            if (!IsValidPhoneNumber(model.PhoneNumber))
            {
                return IdentityResult.Failed(new IdentityError
                {
                    Code = "InvalidPhoneNumber",
                    Description = "Phone number must be exactly 10 digits and start with a valid Vietnam mobile prefix."
                });
            }

            // Kiểm tra số điện thoại đã tồn tại
            if (await _context.PersonalEmployees.AnyAsync(e => e.PhoneNumber == model.PhoneNumber))
            {
                return IdentityResult.Failed(new IdentityError
                {
                    Code = "DuplicatePhoneNumber",
                    Description = "Phone number is already in use."
                });
            }

            // Kiểm tra số CCCD
            if (!IsValidCCCD(model.NumberIdentification))
            {
                return IdentityResult.Failed(new IdentityError
                {
                    Code = "InvalidCCCD",
                    Description = "Citizen Identification Number (CCCD) must be exactly 12 digits."
                });
            }

            // Kiểm tra CCCD đã tồn tại
            if (await _context.PersonalEmployees.AnyAsync(e => e.NumberIdentification == model.NumberIdentification))
            {
                return IdentityResult.Failed(new IdentityError
                {
                    Code = "DuplicateCCCD",
                    Description = "Citizen Identification Number (CCCD) is already in use."
                });
            }

            var numberEmployee = _context.Employees.Count();
            var employeeCode = $"{(numberEmployee + 1):D4}";
            var employee = new EmployeeModel();
            employee.EmployeeCode = employeeCode;
            _context.Employees.Add(employee);

            // Tạo mới EmployeeModel
            var personalEmployee = new PersonalEmployeeModel
            {
                Id = Guid.NewGuid().ToString(),
                NameEmployee = model.NameEmployee,
                Gender = model.Gender,
                DateOfBirth = model.DateOfBirth,
                Nationality = model.Nationality,
                Ethnicity = model.Ethnicity,
                NumberIdentification = model.NumberIdentification,
                DateIssueIdentification = model.DateIssueIdentification,
                PlaceIssueIdentification = model.PlaceIssueIdentification,
                FrontIdentificationPath = model.FrontIdentificationPath,
                BackIdentificationPath = model.BackIdentificationPath,
                ProvinceResidence = model.ProvinceResidence,
                DistrictResidence = model.DistrictResidence,
                WardResidence = model.WardResidence,
                HouseNumberResidence = model.HouseNumberResidence,
                ProvinceContact = model.ProvinceContact,
                DistrictContact = model.DistrictContact,
                WardContact = model.WardContact,
                HouseNumberContact = model.HouseNumberContact,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                BankNumber = model.BankNumber,
                NameBank = model.NameBank,
                BranchBank = model.BranchBank,
                EmployeeCode = employeeCode,
                Employee = employee
            };

            _context.PersonalEmployees.Add(personalEmployee);
            await _context.SaveChangesAsync();
            return IdentityResult.Success;
        }

        private bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                var regex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");
                return regex.IsMatch(email);
            }
            catch
            {
                return false;
            }
        }

        private bool IsValidPhoneNumber(string phoneNumber)
        {
            if (string.IsNullOrWhiteSpace(phoneNumber))
                return false;

            var regex = new Regex(@"^\d{10}$");
            if (!regex.IsMatch(phoneNumber))
                return false;

            string[] validPrefixes = { "03", "05", "07", "08", "09" };
            return validPrefixes.Any(prefix => phoneNumber.StartsWith(prefix));
        }

        private bool IsValidCCCD(string cccd)
        {
            if (string.IsNullOrWhiteSpace(cccd))
                return false;

            var regex = new Regex(@"^(\d{9}|\d{12})$");
            return regex.IsMatch(cccd);
        }
    }
}