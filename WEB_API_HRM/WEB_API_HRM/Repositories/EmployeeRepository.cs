using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WEB_API_HRM.Data;
using WEB_API_HRM.DTO;
using WEB_API_HRM.Models;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Swashbuckle.AspNetCore.SwaggerUI;
using Microsoft.VisualBasic;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using WEB_API_HRM.Helpers;
using WEB_API_HRM.RSP;
using System.ComponentModel;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Diagnostics.Contracts;
using Microsoft.IdentityModel.Logging;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.CodeAnalysis.Operations;

namespace WEB_API_HRM.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly HRMContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IAccountRepository _accountRepository;
        public EmployeeRepository(HRMContext context, UserManager<ApplicationUser> userManager, IAccountRepository accountRepository)
        {
            _context = context;
            _userManager = userManager;
            _accountRepository = accountRepository;
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
                Nationality = model.Nationality ?? string.Empty,
                Ethnicity = model.Ethnicity ?? string.Empty,
                NumberIdentification = model.NumberIdentification,
                DateIssueIdentification = model.DateIssueIdentification,
                PlaceIssueIdentification = model.PlaceIssueIdentification ?? string.Empty,
                FrontIdentificationPath = model.FrontIdentificationPath,
                BackIdentificationPath = model.BackIdentificationPath,
                ProvinceResidence = model.ProvinceResidence ?? string.Empty,
                DistrictResidence = model.DistrictResidence ?? string.Empty,
                WardResidence = model.WardResidence ?? string.Empty,
                HouseNumberResidence = model.HouseNumberResidence ?? string.Empty,
                ProvinceContact = model.ProvinceContact ?? string.Empty,
                DistrictContact = model.DistrictContact ?? string.Empty,
                WardContact = model.WardContact ?? string.Empty,
                HouseNumberContact = model.HouseNumberContact ?? string.Empty,
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
        private (string LastName, string FirstName) SplitName(string nameEmployee)
        {
            if (string.IsNullOrWhiteSpace(nameEmployee))
            {
                return (string.Empty, string.Empty);
            }

            nameEmployee = nameEmployee.Trim();
            var parts = nameEmployee.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            if (parts.Length == 0)
            {
                return (string.Empty, string.Empty);
            }
            else if (parts.Length == 1)
            {
                return (string.Empty, parts[0]); 
            }
            else
            {
                string firstName = parts[parts.Length - 1];
                string lastName = string.Join(" ", parts.Take(parts.Length - 1));
                return (lastName, firstName);
            }
        }
        public async Task<IdentityResult> CreatePersonelEmployeeAsync(CreatePersonelEmployeeDto model)
        {
            if(!(await _context.Employees.AnyAsync(e => e.EmployeeCode == model.EmployeeCode)))
            {
                return IdentityResult.Failed(new IdentityError
                {
                    Code = "EmployeeNotFound",
                    Description = "Employee not found."
                });
            }
            if ((await _context.PersonelEmployees.AnyAsync(e => e.EmployeeCode == model.EmployeeCode)))
            {
                return IdentityResult.Failed(new IdentityError
                {
                    Code = "DuplicatePersonel",
                    Description = "personel employee exist in system."
                });
            }
            if (!(await _context.Branchs.AnyAsync(b => b.BranchName == model.BranchName)))
            {
                return IdentityResult.Failed(new IdentityError
                {
                    Code = "BranchNotFound",
                    Description = "Branch not found."
                });
            }

            if(!(await _context.Departments.AnyAsync(d => d.DepartmentName == model.DepartmentName)))
            {
                return IdentityResult.Failed(new IdentityError
                {
                    Code = "DepartmentNotFound",
                    Description = "Department not found."
                });
            }

            //if(!(await _context.JobTitles.AnyAsync(j => j.JobTitleName == model.JobtitleName)))
            //{
            //    return IdentityResult.Failed(new IdentityError
            //    {
            //        Code = "JobtitleNotFound",
            //        Description = "Jobtitle not found."
            //    });
            //}
            if(!(await _context.Ranks.AnyAsync(r => r.RankName == model.RankName)))
            {
                return IdentityResult.Failed(new IdentityError
                {
                    Code = "RankNotFound",
                    Description = "Rank not found."
                });
            }
            if(!(await _context.Positions.AnyAsync(p => p.PositionName == model.PositionName)))
            {
                return IdentityResult.Failed(new IdentityError
                {
                    Code = "PositionNotFound",
                    Description = "Position not found."
                });
            }
            if(!(await _context.Employees.AnyAsync(e => e.EmployeeCode == model.ManagerId)))
            {
                return IdentityResult.Failed(new IdentityError
                {
                    Code = "ManagerNotFound",
                    Description = "Manager not found."
                });
            }
            if(!(await _context.JobTypes.AnyAsync(j=>j.NameJobType == model.JobTypeName)))
            {
                return IdentityResult.Failed(new IdentityError
                {
                    Code = "JobTypeNotFound",
                    Description = "Job type not found."
                });
            }
            if (!(await _context.ApplicationRoles.AnyAsync(r => r.Name == model.RoleName)))
            {
                return IdentityResult.Failed(new IdentityError
                {
                    Code = "RoleNotFound",
                    Description = "Role not found."
                });
            }

            var branch = await _context.Branchs.FirstOrDefaultAsync(b => b.BranchName == model.BranchName);
            var department = await _context.Departments.FirstOrDefaultAsync(d => d.DepartmentName == model.DepartmentName);
            //var jobtitle = await _context.JobTitles.FirstOrDefaultAsync(j => j.JobTitleName == model.JobtitleName);
            var rank = await _context.Ranks.FirstOrDefaultAsync(r => r.RankName == model.RankName);
            var position = await _context.Positions.FirstOrDefaultAsync(p => p.PositionName == model.PositionName);
            var jobtype = await _context.JobTypes.FirstOrDefaultAsync(j => j.NameJobType == model.JobTypeName);
            var role = await _context.ApplicationRoles.FirstOrDefaultAsync(r => r.Name == model.RoleName);
            
            

            var personalEmployee = await _context.PersonalEmployees.FirstOrDefaultAsync(p => p.EmployeeCode == model.EmployeeCode);


            var (lastName, firstName) = SplitName(model.NameEmployee);
            var signUpModel = new SignUpModel();
            signUpModel.FirstName = firstName;
            signUpModel.LastName = lastName;
            signUpModel.Username = model.Username;
            signUpModel.Password = model.Password;
            signUpModel.Email = personalEmployee.Email;
            signUpModel.RoleCode = role.Id;
            var resultSignUp = await _accountRepository.SignUpAsync(signUpModel);
            var errorList = resultSignUp.Errors.Select(e => e.Description).ToList();

            if (errorList.Any(e => e.Contains("Username already exists in the system.")))
            {
                return IdentityResult.Failed(new IdentityError
                {
                    Code = "DuplicateUser",
                    Description = "Username already exists in the system."
                });
            }
            else if (errorList.Any(e => e.Contains("Email format is invalid.")))
            {
                return IdentityResult.Failed(new IdentityError
                {
                    Code = "InvalidEmail",
                    Description = "Email format is invalid."
                });
            }
            else if (errorList.Any(e => e.Contains("Role not found.")))
            {
                return IdentityResult.Failed(new IdentityError
                {
                    Code = "RoleNotFound",
                    Description = "Role not found."
                });
            }
            var user = await _userManager.FindByNameAsync(model.Username);

            var employee = await _context.Employees.FirstOrDefaultAsync(e => e.EmployeeCode == model.EmployeeCode);

            var personelEmployee = new PersonelEmployeeModel();
            personelEmployee.Id = Guid.NewGuid().ToString();
            personelEmployee.DateJoinCompany = model.DateJoinCompany;
            personelEmployee.BranchId = branch.Id;
            personelEmployee.Branch = branch;
            personelEmployee.DepartmentId = department.Id;
            personelEmployee.Department = department;
            //personelEmployee.JobTitleId = jobtitle.Id;
            //personelEmployee.JobTitle = jobtitle;
            personelEmployee.RankId = rank.Id;
            personelEmployee.Rank = rank;
            personelEmployee.PositionId = position.Id;
            personelEmployee.Position = position;
            personelEmployee.ManagerId = model.ManagerId;
            personelEmployee.JobTypeId = jobtype.Id;
            personelEmployee.AvatarPath = model.AvatarPath ?? string.Empty;
            personelEmployee.UserId = model.Username;
            personelEmployee.User = user;
            personelEmployee.RoleId = role.Id;
            personelEmployee.Role = role;
            personelEmployee.EmployeeCode = employee.EmployeeCode;
            personelEmployee.Employee = employee;


            _context.PersonelEmployees.Add(personelEmployee);
            await _context.SaveChangesAsync();
            return IdentityResult.Success;
        }

        public async Task<List<CodeNameEmployeeRes>> GetCodeNameEmployeeAsync()
        {
            var personalEmpts = await _context.PersonalEmployees.ToListAsync();
            var personelEmpts = await _context.PersonelEmployees.ToListAsync();

            var codeNameEmployeeList = new List<CodeNameEmployeeRes>();
            foreach(var employee in personalEmpts)
            {
                if (await _context.PersonelEmployees.AnyAsync(e => e.EmployeeCode == employee.EmployeeCode))
                    continue;
                var codeName = new CodeNameEmployeeRes();
                codeName.EmployeeCode = employee.EmployeeCode;
                codeName.EmployeeName = employee.NameEmployee;
                codeNameEmployeeList.Add(codeName);
            }
            return codeNameEmployeeList;
        }

        public async Task<List<CodeNameEmployeeRes>> GetCodeNameManagerAsync(string employeeCode, string rankName)
        {
            var employee = await _context.PersonalEmployees.FirstOrDefaultAsync(e => e.EmployeeCode == employeeCode);
            var employees = await _context.PersonelEmployees.ToListAsync();
            var rankEmployee = await _context.Ranks.FirstOrDefaultAsync(r => r.RankName == rankName);
            var codeNameManagerList = new List<CodeNameEmployeeRes>();
            if (employee != null && rankName!= null)
            {
                if(rankEmployee.PriorityLevel == 1)
                {
                    var firstManager = new CodeNameEmployeeRes();
                    firstManager.EmployeeName = employee.NameEmployee;
                    firstManager.EmployeeCode = employee.EmployeeCode;
                    codeNameManagerList.Add(firstManager);
                }
                foreach(var empt in employees)
                {
                    var rankManager = await _context.Ranks.FirstOrDefaultAsync(r => r.Id == empt.RankId);
                    if(rankManager.PriorityLevel < rankEmployee.PriorityLevel)
                    {
                        var codeName = new CodeNameEmployeeRes();
                        codeName.EmployeeCode = empt.EmployeeCode;
                        var personalManager = await _context.PersonalEmployees.FirstOrDefaultAsync(e => e.EmployeeCode == empt.EmployeeCode);
                        if (personalManager != null)
                            codeName.EmployeeName = personalManager.NameEmployee; 
                        codeNameManagerList.Add(codeName);
                    }
                }
            }
            return codeNameManagerList;
        }

        public async Task<GenderDayOfBirthRes> GetGenderDayOfBirthAsync(string employeeCode)
        {
            var employee = await _context.PersonalEmployees.FirstOrDefaultAsync(e => e.EmployeeCode == employeeCode);
            var genderBirth = new GenderDayOfBirthRes();
            genderBirth.Gender = employee.Gender;
            genderBirth.DateOfBirth = employee.DateOfBirth;

            return genderBirth;
        }

        public async Task<AccountDefaultRes> GetAccountDefaultAsync(string employeeCode)
        {
            var accounDefault = new AccountDefaultRes();
            accounDefault.Username = employeeCode;
            accounDefault.Password = "Abc&123";
            return accounDefault;
        }

        public async Task<string> GetRoleEmployee(string jobtitleName)
        {
            var jobtile = await _context.JobTitles.FirstOrDefaultAsync(j => j.JobTitleName == jobtitleName);
            if(jobtile != null)
            {
                var role = await _context.ApplicationRoles.FirstOrDefaultAsync(r => r.Id == jobtile.RoleId);
                return role.Name;
            }
            return string.Empty;
        }

        public async Task<List<CodeNameEmployeeRes>> GetCodeNameEmployeeUnContractAsync()
        {
            var personalEmpts = await _context.PersonalEmployees.ToListAsync();
            var contractEmpts = await _context.ContractEmployees.ToListAsync();

            var codeNameList = new List<CodeNameEmployeeRes>();
            foreach (var employee in personalEmpts)
            {
                if (await _context.ContractEmployees.AnyAsync(e => e.EmployeeCode == employee.EmployeeCode))
                    continue;
                var codeName = new CodeNameEmployeeRes();
                codeName.EmployeeCode = employee.EmployeeCode;
                codeName.EmployeeName = employee.NameEmployee;
                codeNameList.Add(codeName);
            }
            return codeNameList;
        }

        public Task<string> GetCodeContect(string employeeCode)
        {
            string datePart = DateTime.Now.Date.ToString("ddMMyyyy");
            string contractCode = $"{datePart}/HĐLĐ-{employeeCode}";
            return Task.FromResult(contractCode);
        }

        public async Task<PositionCoeficientRes> GetPositionCoeficient(string employeeCode)
        {
            var employee = await _context.PersonelEmployees.FirstOrDefaultAsync(e => e.EmployeeCode == employeeCode);
            var positionCoefficient = new PositionCoeficientRes();
            if (employee != null)
            {
                var position = await _context.Positions.FirstOrDefaultAsync(p => p.Id == employee.PositionId);
                var coefficient = await _context.SalaryCoefficients.FirstOrDefaultAsync(c => c.PositionId == position.Id);
                positionCoefficient.PositionName = (position != null) ? position.PositionName : "";
                positionCoefficient.Coefficient = (coefficient != null) ? coefficient.SalaryCoefficient : 0;
                var basicSalary = await _context.BasicSettingSalary.FirstOrDefaultAsync();
                positionCoefficient.HourWorkStandard = basicSalary.HourWorkStandard;
                positionCoefficient.DayWorkStandard = basicSalary.DayWorkStandard;
                positionCoefficient.HourlySalary = basicSalary.HourlySalary;
                positionCoefficient.BasicSalary = (basicSalary.HourlySalary * basicSalary.HourWorkStandard * basicSalary.DayWorkStandard);
            }
            return positionCoefficient;
        }

        public async Task<IdentityResult> CreateContractEmployeeAsync(CreateContractEmployeeDto model)
        {
            if (!(await _context.Employees.AnyAsync(e => e.EmployeeCode == model.EmployeeCode)))
            {
                return IdentityResult.Failed(new IdentityError
                {
                    Code = "EmployeeNotFound",
                    Description = "Employee not found."
                });
            }
            if ((await _context.ContractEmployees.AnyAsync(e => e.EmployeeCode == model.EmployeeCode)))
            {
                return IdentityResult.Failed(new IdentityError
                {
                    Code = "DuplicateContract",
                    Description = "contract employee exist in system."
                });
            }
            if (!(await _context.Positions.AnyAsync(p => p.PositionName == model.NamePosition)))
            {
                return IdentityResult.Failed(new IdentityError
                {
                    Code = "PositionNotFound",
                    Description = "Position not found."
                });
            }

            var position = await _context.Positions.FirstOrDefaultAsync(p => p.PositionName == model.NamePosition);
            var coefficient = await _context.SalaryCoefficients.FirstOrDefaultAsync(c => c.PositionId == position.Id);
            var employee = await _context.Employees.FirstOrDefaultAsync(e => e.EmployeeCode == model.EmployeeCode);
            var employeeAllowances = new List<EmployeeAllowance>();
            foreach(var allowanceRes in model.Allowances)
            {
                var allowance = await _context.Allowances.FirstOrDefaultAsync(e => e.NameAllowance == allowanceRes.NameAllowance);
                var employeeAllowance = new EmployeeAllowance();
                employeeAllowance.AllowanceId = allowance.Id;
                employeeAllowance.EmployeeCode = model.EmployeeCode;

                employeeAllowances.Add(employeeAllowance);

            }

            _context.EmployeeAllowances.AddRange(employeeAllowances);


            var contract = new ContractEmployeeModel();
            contract.Id = Guid.NewGuid().ToString();
            contract.ContractCode = model.CodeContract;
            contract.TypeContract = model.TypeContract ?? string.Empty;
            contract.DateStartContract = model.StartContract;
            contract.DateEndContract = model.EndContract;
            contract.ContractStatus = model.StatusContract ?? string.Empty;
            contract.SalaryCoefficientId = coefficient.Id;
            contract.SalaryCoefficient = coefficient;
            contract.EmployeeCode = model.EmployeeCode;
            contract.Employee = employee;


            _context.ContractEmployees.Add(contract);
            await _context.SaveChangesAsync();
            return IdentityResult.Success;
        }

        public async Task<RateInsuranceModel> GetRateInsuranceAsync()
        {
            return await _context.RateInsurances.FirstOrDefaultAsync();            
        }

        public async Task<List<CodeNameEmployeeRes>> GetCodeNameEmployeeUnInsuranceAsync()
        {
            var personalEmpts = await _context.PersonalEmployees.ToListAsync();
            var insuranceEmpts = await _context.InsuranceEmployees.ToListAsync();

            var codeNameList = new List<CodeNameEmployeeRes>();
            foreach (var employee in personalEmpts)
            {
                if (await _context.InsuranceEmployees.AnyAsync(e => e.EmployeeCode == employee.EmployeeCode))
                    continue;
                var codeName = new CodeNameEmployeeRes();
                codeName.EmployeeCode = employee.EmployeeCode;
                codeName.EmployeeName = employee.NameEmployee;
                codeNameList.Add(codeName);
            }
            return codeNameList;
        }

        public async Task<IdentityResult> CreateInsuranceEmployeeAsync(CreateInsuranceDto model)
        {
            if (!(await _context.Employees.AnyAsync(e => e.EmployeeCode == model.EmployeeCode)))
            {
                return IdentityResult.Failed(new IdentityError
                {
                    Code = "EmployeeNotFound",
                    Description = "Employee not found."
                });
            }
            if ((await _context.InsuranceEmployees.AnyAsync(e => e.EmployeeCode == model.EmployeeCode)))
            {
                return IdentityResult.Failed(new IdentityError
                {
                    Code = "DuplicateInsurance",
                    Description = "insurance employee exist in system."
                });
            }

            var rateInsurance = await _context.RateInsurances.FirstOrDefaultAsync();
            var employee = await _context.Employees.FirstOrDefaultAsync(e => e.EmployeeCode == model.EmployeeCode);

            var insurance = new InsuranceEmployeeModel();
            insurance.Id = Guid.NewGuid().ToString();
            insurance.CodeBHYT = model.CodeBHYT;
            insurance.CodeBHXH = model.CodeBHXH;
            insurance.RegisterMedical = model.RegisterMedical;
            insurance.DateStartParticipateBHYT = model.DateStartParticipateBHYT;
            insurance.HasBHXH = model.HasBHXH;
            insurance.DateStartParticipateBHXH = model.DateStartParticipateBHXH;
            insurance.DateStartParticipateBHTN = model.DateStartParticipateBHTN;
            insurance.InsuranceStatus = model.InsuranceStatus;
            insurance.DateEndParticipateInsurance = model.DateEndParticipateInsurance;
            insurance.Employee = employee;
            insurance.EmployeeCode = model.EmployeeCode;


            _context.InsuranceEmployees.Add(insurance);
            await _context.SaveChangesAsync();
            return IdentityResult.Success;
        }       
        public async Task<IdentityResult> CreateTaxEmployeeAsync(CreateTaxDto model)
        {
            if (!(await _context.Employees.AnyAsync(e => e.EmployeeCode == model.EmployeeCode)))
            {
                return IdentityResult.Failed(new IdentityError
                {
                    Code = "EmployeeNotFound",
                    Description = "Employee not found."
                });
            }
            if ((await _context.TaxEmployees.AnyAsync(e => e.EmployeeCode == model.EmployeeCode)))
            {
                return IdentityResult.Failed(new IdentityError
                {
                    Code = "DuplicateTax",
                    Description = "tax employee exist in system."
                });
            }


            var employee = await _context.Employees.FirstOrDefaultAsync(e => e.EmployeeCode == model.EmployeeCode);
            var tax = new TaxEmployeeModel();
            tax.Id = Guid.NewGuid().ToString();
            tax.HasTaxCode = model.HasTaxCode;
            tax.TaxCode = model.TaxCode;
            tax.Employee = employee;
            tax.EmployeeCode = employee.EmployeeCode;

            _context.TaxEmployees.Add(tax);

            var dependents = new List<DependentModel>();
            foreach(var dependentModel in model.Dependents)
            {
                var dependent = new DependentModel();
                dependent.Id = Guid.NewGuid().ToString(); ;
                dependent.RegisterDependentStatus = dependentModel.RegisterDependentStatus;
                dependent.TaxCode = dependentModel.TaxCode;
                dependent.NameDependent = dependentModel.NameDependent;
                dependent.DayOfBirthDependent = dependentModel.DayOfBirthDependent;
                dependent.Relationship = dependentModel.Relationship;
                dependent.EvidencePath = dependentModel.EvidencePath;
                dependent.Employee = employee;
                dependent.EmployeeCode = employee.EmployeeCode;

                dependents.Add(dependent);
            }

            _context.Dependents.AddRange(dependents);

            await _context.SaveChangesAsync();
            return IdentityResult.Success;
        }

        public async Task<List<CodeNameEmployeeRes>> GetCodeNameEmployeeUnTaxAsync()
        {
            var personalEmpts = await _context.PersonalEmployees.ToListAsync();
            var taxEmpts = await _context.TaxEmployees.ToListAsync();

            var codeNameList = new List<CodeNameEmployeeRes>();
            foreach (var employee in personalEmpts)
            {
                if (await _context.TaxEmployees.AnyAsync(e => e.EmployeeCode == employee.EmployeeCode))
                    continue;
                var codeName = new CodeNameEmployeeRes();
                codeName.EmployeeCode = employee.EmployeeCode;
                codeName.EmployeeName = employee.NameEmployee;
                codeNameList.Add(codeName);
            }
            return codeNameList;
        }

        public async Task<PersonalInfoRes> GetPersonalInformationAsync(string employeeCode)
        {
            var personal = await _context.PersonalEmployees
                .FirstOrDefaultAsync(e => e.EmployeeCode == employeeCode);

            if (personal == null)
            {
                return null;
            }

            var personalRes = new PersonalInfoRes
            {
                NameEmployee = personal.NameEmployee,
                Gender = personal.Gender,
                DateOfBirth = personal.DateOfBirth,
                Nationality = personal.Nationality,
                Ethnicity = personal.Ethnicity,
                NumberIdentification = personal.NumberIdentification,
                DateIssueIdentification = personal.DateIssueIdentification,
                PlaceIssueIdentification = personal.PlaceIssueIdentification,
                FrontIdentificationPath = personal.FrontIdentificationPath,
                BackIdentificationPath = personal.BackIdentificationPath,
                ProvinceResidence = personal.ProvinceResidence,
                DistrictResidence = personal.DistrictResidence,
                WardResidence = personal.WardResidence,
                HouseNumberResidence = personal.HouseNumberResidence,
                ProvinceContact = personal.ProvinceContact,
                DistrictContact = personal.DistrictContact, // Fixed: was incorrectly set to ProvinceContact
                WardContact = personal.WardContact,
                HouseNumberContact = personal.HouseNumberContact,
                Email = personal.Email,
                PhoneNumber = personal.PhoneNumber,
                BankNumber = personal.BankNumber,
                NameBank = personal.NameBank,
                BranchBank = personal.BranchBank,
                EmployeeCode = employeeCode
            };

            return personalRes;
        }

        public async Task<string> GetEmployeeCodeToUsername(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            return user?.UserName; // Added null check for safety
        }

        public async Task<PersonelInformationRes> GetPersonelInformationAsync(string employeeCode)
        {
            var personel = await _context.PersonelEmployees.FirstOrDefaultAsync(e => e.EmployeeCode == employeeCode);
           
            if(personel == null)
            {
                return null;
            }

            var personal = await _context.PersonalEmployees.FirstOrDefaultAsync(e => e.EmployeeCode == employeeCode);

            var branch = await _context.Branchs.FirstOrDefaultAsync(b => b.Id == personel.BranchId);
            var department = await _context.Departments.FirstOrDefaultAsync(d => d.Id == personel.DepartmentId);
            //var jobtitle = await _context.JobTitles.FirstOrDefaultAsync(j => j.Id == personel.JobTitleId);
            var rank = await _context.Ranks.FirstOrDefaultAsync(r => r.Id == personel.RankId);
            var position = await _context.Positions.FirstOrDefaultAsync(p => p.Id == personel.PositionId);
            var manager = await _context.PersonalEmployees.FirstOrDefaultAsync(m => m.EmployeeCode == personel.ManagerId);
            var jobtype = await _context.JobTypes.FirstOrDefaultAsync(j => j.Id == personel.JobTypeId);
            var breakLunch = await _context.CheckInOutSettings.FirstOrDefaultAsync();
            double breakLunchRes = breakLunch.BreakHour + ((double)breakLunch.BreakMinute / (double)60);

            var user = await _userManager.FindByNameAsync(employeeCode);

            var roleUser = await _context.UserRoles.FirstOrDefaultAsync(u => u.UserId == user.Id);

            var role = await _context.ApplicationRoles.FirstOrDefaultAsync(r => r.Id == roleUser.RoleId);

            var personelRes = new PersonelInformationRes();
            personelRes.EmployeeCode = personel.EmployeeCode ?? string.Empty;          
            personelRes.NameEmployee = personal.NameEmployee ?? string.Empty;
            personelRes.Gender = personal.Gender ?? string.Empty;
            personelRes.DateOfBirth = personal.DateOfBirth;
            personelRes.DateJoinCompany = personel.DateJoinCompany;
            personelRes.BranchName = branch.BranchName ?? string.Empty;
            personelRes.DepartmentName = department.DepartmentName ?? string.Empty;
            //personelRes.JobtitleName = jobtitle.JobTitleName ?? string.Empty;
            personelRes.RankName = rank.RankName ?? string.Empty;
            personelRes.PositionName = position.PositionName ?? string.Empty;
            personelRes.ManagerName = $"{employeeCode} - {manager.NameEmployee} " ?? string.Empty;
            personelRes.JobTypeName = jobtype.NameJobType ?? string.Empty;
            personelRes.BreakLunch = breakLunchRes;
            personelRes.AvatarPath = personel.AvatarPath ?? string.Empty;
            personelRes.RoleName = role.Name;
            return personelRes;
        }

        public async Task<CreateContractEmployeeDto> GetContractInformationAsync(string employeeCode)
        {
            var contract = await _context.ContractEmployees.FirstOrDefaultAsync(e => e.EmployeeCode == employeeCode);

            if(contract == null)
            {
                return null;
            }

            var personal = await _context.PersonalEmployees.FirstOrDefaultAsync(e => e.EmployeeCode == employeeCode);

            var basicSalarySetting = await _context.BasicSettingSalary.FirstOrDefaultAsync();

            var salaryCoefficient = await _context.SalaryCoefficients.FirstOrDefaultAsync(s => s.Id == contract.SalaryCoefficientId);

            var position = await _context.Positions.FirstOrDefaultAsync(p => p.Id == salaryCoefficient.PositionId);

            var allowances = await _context.EmployeeAllowances.Where(a => a.EmployeeCode == employeeCode).ToListAsync();
            var allowancesRes = new List<AllowanceRes>();

            foreach(var allowance in allowances)
            {
                var allowanceRes = new AllowanceRes();

                var allowanceModel = await _context.Allowances.FirstOrDefaultAsync(a => a.Id == allowance.AllowanceId);
                allowanceRes.NameAllowance = allowanceModel.NameAllowance;
                allowanceRes.MoneyAllowance = allowanceModel.MoneyAllowance;

                allowancesRes.Add(allowanceRes);
            }

            var contractRes = new CreateContractEmployeeDto();
            contractRes.EmployeeCode = contract.EmployeeCode;
            contractRes.NameEmployee = personal.NameEmployee;
            contractRes.Gender = personal.Gender;
            contractRes.DateOfBirth = personal.DateOfBirth;
            contractRes.CodeContract = contract.ContractCode;
            contractRes.TypeContract = contract.TypeContract;
            contractRes.StartContract = contract.DateStartContract;
            contractRes.EndContract = contract.DateEndContract;
            contractRes.StatusContract = contract.ContractStatus;
            contractRes.HourlySalary = basicSalarySetting.HourlySalary;
            contractRes.HourWorkStandard = basicSalarySetting.HourWorkStandard;
            contractRes.NamePosition = position.PositionName;
            contractRes.CoefficientSalary = salaryCoefficient.SalaryCoefficient;
            contractRes.DayWorkStandard = basicSalarySetting.DayWorkStandard;
            contractRes.BasicSalary = basicSalarySetting.HourlySalary * basicSalarySetting.HourWorkStandard * basicSalarySetting.DayWorkStandard;
            contractRes.Allowances = allowancesRes;

            return contractRes;
        }

        public async Task<CreateInsuranceDto> GetInsuranceInformationAsync(string employeeCode)
        {
            var insurance = await _context.InsuranceEmployees.FirstOrDefaultAsync(i => i.EmployeeCode == employeeCode);

            if (insurance == null)
                return null;

            var personal = await _context.PersonalEmployees.FirstOrDefaultAsync(p => p.EmployeeCode == employeeCode);

            var rateInsurance = await _context.RateInsurances.FirstOrDefaultAsync();

            var insuranceRes = new CreateInsuranceDto();
            insuranceRes.EmployeeCode = employeeCode;
            insuranceRes.NameEmployee = personal.NameEmployee;
            insuranceRes.Gender = personal.Gender;
            insuranceRes.DateOfBirth = personal.DateOfBirth;
            insuranceRes.CodeBHYT = insurance.CodeBHYT;
            insuranceRes.RateBHYTBussiness = rateInsurance.bhytBusinessRate;
            insuranceRes.RateBHYTEmpt = rateInsurance.bhytEmpRate;
            insuranceRes.RegisterMedical = insurance.RegisterMedical;
            insuranceRes.DateStartParticipateBHYT = insurance.DateStartParticipateBHYT;
            insuranceRes.HasBHXH = insurance.HasBHXH;
            insuranceRes.CodeBHXH = insurance.CodeBHXH;
            insuranceRes.RateBHXHEmpt = rateInsurance.bhxhEmpRate;
            insuranceRes.RateBHXHBussiness = rateInsurance.bhxhBusinessRate;
            insuranceRes.DateStartParticipateBHXH = insurance.DateStartParticipateBHXH;
            insuranceRes.RateBHTNBussiness = rateInsurance.bhtnBusinessRate;
            insuranceRes.RateBHTNEmpt = rateInsurance.bhtnEmpRate;
            insuranceRes.DateStartParticipateBHTN = insurance.DateStartParticipateBHTN;
            insuranceRes.InsuranceStatus = insurance.InsuranceStatus;
            insuranceRes.DateEndParticipateInsurance = insurance.DateEndParticipateInsurance;

            return insuranceRes;
        }

        public async Task<CreateTaxDto> GetTaxInformationAsync(string employeeCode)
        {
            var tax = await _context.TaxEmployees.FirstOrDefaultAsync(t => t.EmployeeCode == employeeCode);

            if (tax == null)
                return null;

            var personal = await _context.PersonalEmployees.FirstOrDefaultAsync(p => p.EmployeeCode == employeeCode);

            var dependents = await _context.Dependents.Where(d => d.EmployeeCode == employeeCode).ToListAsync();

            var dependentsRes = new List<DependentRes>();

            foreach(var dependent in dependents)
            {
                var dependentRes = new DependentRes();
                dependentRes.RegisterDependentStatus = dependent.RegisterDependentStatus;
                dependentRes.TaxCode = dependent.TaxCode;
                dependentRes.NameDependent = dependent.NameDependent;
                dependentRes.DayOfBirthDependent = dependent.DayOfBirthDependent;
                dependentRes.Relationship = dependent.Relationship;
                dependentRes.EvidencePath = dependent.EvidencePath;

                dependentsRes.Add(dependentRes);
            }

            var taxRes = new CreateTaxDto();
            taxRes.EmployeeCode = employeeCode;
            taxRes.NameEmployee = personal.NameEmployee;
            taxRes.Gender = personal.Gender;
            taxRes.DateOfBirth = personal.DateOfBirth;
            taxRes.HasTaxCode = tax.HasTaxCode;
            taxRes.TaxCode = tax.TaxCode;
            taxRes.Dependents = dependentsRes;

            return taxRes;
        }

        public async Task<IdentityResult> UpdatePersonalEmployeeAsync(CreatePersonalEmployeeDto model, string employeeCode)
        {
            var personal = await _context.PersonalEmployees.FirstOrDefaultAsync(e => e.EmployeeCode == employeeCode);

            if (!(await _context.Employees.AnyAsync(e => e.EmployeeCode == employeeCode)))
            {
                return IdentityResult.Failed(new IdentityError
                {
                    Code = "EmployeeNotFound",
                    Description = "Employee not found."
                });
            }

            if (!IsValidEmail(model.Email))
            {
                return IdentityResult.Failed(new IdentityError
                {
                    Code = "InvalidEmail",
                    Description = "Email format is invalid."
                });
            }

            // Kiểm tra email đã tồn tại
            if (model.Email != personal.Email && await _context.PersonalEmployees.AnyAsync(e => e.Email == model.Email))
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
            if (model.PhoneNumber != personal.PhoneNumber && await _context.PersonalEmployees.AnyAsync(e => e.PhoneNumber == model.PhoneNumber))
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
            if (model.NumberIdentification != personal.NumberIdentification && await _context.PersonalEmployees.AnyAsync(e => e.NumberIdentification == model.NumberIdentification))
            {
                return IdentityResult.Failed(new IdentityError
                {
                    Code = "DuplicateCCCD",
                    Description = "Citizen Identification Number (CCCD) is already in use."
                });
            }
            personal.NameEmployee = model.NameEmployee;
            personal.Gender = model.Gender;
            personal.DateOfBirth = model.DateOfBirth;
            personal.Nationality = model.Nationality ?? string.Empty;
            personal.Ethnicity = model.Ethnicity ?? string.Empty;
            personal.NumberIdentification = model.NumberIdentification;
            personal.DateIssueIdentification = model.DateIssueIdentification;
            personal.PlaceIssueIdentification = model.PlaceIssueIdentification ?? string.Empty;
            personal.FrontIdentificationPath = model.FrontIdentificationPath;
            personal.BackIdentificationPath = model.BackIdentificationPath;
            personal.ProvinceResidence = model.ProvinceResidence ?? string.Empty;
            personal.DistrictResidence = model.DistrictResidence ?? string.Empty;
            personal.WardResidence = model.WardResidence ?? string.Empty;
            personal.HouseNumberResidence = model.HouseNumberResidence ?? string.Empty;
            personal.ProvinceContact = model.ProvinceContact ?? string.Empty;
            personal.DistrictContact = model.DistrictContact ?? string.Empty;
            personal.WardContact = model.WardContact ?? string.Empty;
            personal.HouseNumberContact = model.HouseNumberContact ?? string.Empty;
            personal.Email = model.Email;
            personal.PhoneNumber = model.PhoneNumber;
            personal.BankNumber = model.BankNumber;
            personal.NameBank = model.NameBank;
            personal.BranchBank = model.BranchBank;
            personal.EmployeeCode = employeeCode;

            _context.PersonalEmployees.Update(personal);
            await _context.SaveChangesAsync();
            return IdentityResult.Success;
        }

        public async Task<IdentityResult> UpdateContractEmployeeAsync(CreateContractEmployeeDto model)
        {
            var contract = await _context.ContractEmployees.FirstOrDefaultAsync(e => e.EmployeeCode == model.EmployeeCode);

            if (contract == null)
                return null;

            var position = await _context.Positions.FirstOrDefaultAsync(p => p.PositionName == model.NamePosition);

            var salaryCoefficient = await _context.SalaryCoefficients.FirstOrDefaultAsync(s => s.PositionId == position.Id);

            var employeeAllowanceOld = await _context.EmployeeAllowances.Where(e => e.EmployeeCode == model.EmployeeCode).ToListAsync();
            foreach(var allowance in employeeAllowanceOld)
            {
                _context.EmployeeAllowances.Remove(allowance);
            }

            var employeeAllowances = new List<EmployeeAllowance>();
            foreach(var allowance in model.Allowances)
            {
                var allowanceModel = await _context.Allowances.FirstOrDefaultAsync(a => a.NameAllowance == allowance.NameAllowance);
                var employeeAllowance = new EmployeeAllowance();
                employeeAllowance.AllowanceId = allowanceModel.Id;
                employeeAllowance.EmployeeCode = model.EmployeeCode;
                employeeAllowances.Add(employeeAllowance);
            }

            contract.ContractCode = model.CodeContract;
            contract.ContractStatus = model.StatusContract;
            contract.TypeContract = model.TypeContract;
            contract.DateStartContract = model.StartContract;
            contract.DateEndContract = model.EndContract;
            contract.SalaryCoefficientId = salaryCoefficient.Id;
            contract.EmployeeAllowances = employeeAllowances;
            contract.EmployeeCode = model.EmployeeCode;

            _context.ContractEmployees.Update(contract);
            await _context.SaveChangesAsync();
            return IdentityResult.Success;
        }

        public async Task<IdentityResult> UpdatePersonelEmployeeAsync(CreatePersonelEmployeeDto model)
        {
            var personel = await _context.PersonelEmployees.FirstOrDefaultAsync(e => e.EmployeeCode == model.EmployeeCode);

            if (personel == null)
                return null;

            var branch = await _context.Branchs.FirstOrDefaultAsync(b => b.BranchName == model.BranchName);
            var department = await _context.Departments.FirstOrDefaultAsync(d => d.DepartmentName == model.DepartmentName);
            //var jobtitle = await _context.JobTitles.FirstOrDefaultAsync(j => j.JobTitleName == model.JobtitleName);
            var rank = await _context.Ranks.FirstOrDefaultAsync(r => r.RankName == model.RankName);
            var jobtype = await _context.JobTypes.FirstOrDefaultAsync(j => j.NameJobType == model.JobTypeName);
            var position = await _context.Positions.FirstOrDefaultAsync(p => p.PositionName == model.PositionName);

            var role = await _context.ApplicationRoles.FirstOrDefaultAsync(r => r.Name == model.RoleName);

            var user = await _userManager.FindByNameAsync(model.EmployeeCode);

            var roleUser = await _context.UserRoles.FirstOrDefaultAsync(u => u.UserId == user.Id);

            _context.UserRoles.Remove(roleUser);

            var newRoleUser = new IdentityUserRole<string>();
            newRoleUser.UserId = user.Id;
            newRoleUser.RoleId = role.Id;

            _context.UserRoles.Add(newRoleUser);


            personel.DateJoinCompany = model.DateJoinCompany;
            personel.BranchId = branch.Id;
            personel.DepartmentId = department.Id;
           // personel.JobTitleId = jobtitle.Id;
            personel.RankId = rank.Id;
            personel.PositionId = position.Id;
            personel.ManagerId = model.ManagerId;
            personel.JobTypeId = jobtype.Id;
            personel.AvatarPath = model.AvatarPath;
            personel.EmployeeCode = model.EmployeeCode;
            personel.RoleId = role.Id;

            _context.PersonelEmployees.Update(personel);
            await _context.SaveChangesAsync();
            return IdentityResult.Success;
        }

        public async Task<IdentityResult> UpdateInsuranceEmployeeAsync(CreateInsuranceDto model)
        {
            var insurance = await _context.InsuranceEmployees.FirstOrDefaultAsync(i => i.EmployeeCode == model.EmployeeCode);

            if (insurance == null)
                return null;

            insurance.CodeBHYT = model.CodeBHYT;
            insurance.RegisterMedical = model.RegisterMedical;
            insurance.DateStartParticipateBHYT = model.DateStartParticipateBHYT;
            insurance.HasBHXH = model.HasBHXH;
            insurance.CodeBHXH = model.CodeBHXH;
            insurance.DateStartParticipateBHXH = model.DateStartParticipateBHXH;
            insurance.DateStartParticipateBHTN = model.DateStartParticipateBHTN;
            insurance.InsuranceStatus = model.InsuranceStatus;
            insurance.DateEndParticipateInsurance = model.DateEndParticipateInsurance;
            insurance.EmployeeCode = model.EmployeeCode;

            _context.InsuranceEmployees.Update(insurance);
            await _context.SaveChangesAsync();
            return IdentityResult.Success;
        }

        public async Task<IdentityResult> UpdateTaxEmployeeAsync(CreateTaxDto model)
        {
            var tax = await _context.TaxEmployees.FirstOrDefaultAsync(t => t.EmployeeCode == model.EmployeeCode);

            if (tax == null)
                return null;

            tax.HasTaxCode = model.HasTaxCode;
            tax.TaxCode = model.TaxCode;
            tax.EmployeeCode = model.EmployeeCode;


            var dependentOlds = await _context.Dependents.Where(d => d.EmployeeCode == model.EmployeeCode).ToListAsync();
            foreach (var dependent in dependentOlds)
            {
                _context.Dependents.Remove(dependent);
            }

            var dependentNews = new List<DependentModel>();
            foreach (var dependentRes in model.Dependents)
            {
                var dependentNew = new DependentModel();
                dependentNew.Id = Guid.NewGuid().ToString();
                dependentNew.RegisterDependentStatus = dependentRes.RegisterDependentStatus;
                dependentNew.TaxCode = dependentRes.TaxCode;
                dependentNew.NameDependent = dependentRes.NameDependent;
                dependentNew.DayOfBirthDependent = dependentRes.DayOfBirthDependent;
                dependentNew.Relationship = dependentRes.Relationship;
                dependentNew.EvidencePath = dependentRes.EvidencePath;
                dependentNew.EmployeeCode = model.EmployeeCode;

                dependentNews.Add(dependentNew);
            }
            _context.Dependents.AddRange(dependentNews);

            _context.TaxEmployees.Update(tax);
            await _context.SaveChangesAsync();
            return IdentityResult.Success;
        }

        public async Task<List<AllPersonalRes>> GetAllPersonal()
        {
            var personals = await _context.PersonalEmployees.ToListAsync();

            var allPersonal = new List<AllPersonalRes>();
            foreach(var personal in personals)
            {
                var personalRes = new AllPersonalRes();
                var personel = await _context.PersonelEmployees.FirstOrDefaultAsync(e => e.EmployeeCode == personal.EmployeeCode);
                if(personel != null)
                    personalRes.AvatarPath = personel.AvatarPath ?? string.Empty;
                else
                    personalRes.AvatarPath = string.Empty;
                personalRes.EmployeeCode = personal.EmployeeCode;
                personalRes.NameEmployee = personal.NameEmployee;
                personalRes.Gender = personal.Gender;
                personalRes.DayOfBirth = personal.DateOfBirth;
                personalRes.Nationality = personal.Nationality ?? string.Empty;
                personalRes.Ethnicity = personal.Ethnicity ?? string.Empty;
                personalRes.ProvinceResidence = personal.ProvinceResidence ?? string.Empty;
                personalRes.DistrictResidence = personal.DistrictResidence ?? string.Empty;
                personalRes.WardResidence = personal.WardResidence ?? string.Empty;
                personalRes.HouseNumberResidence = personal.HouseNumberResidence ?? string.Empty;
                personalRes.ProvinceContact = personal.ProvinceContact ?? string.Empty;
                personalRes.DistrictContact = personal.DistrictContact ?? string.Empty;
                personalRes.WardContact = personal.WardContact ?? string.Empty;
                personalRes.HouseNumberContact = personal.HouseNumberContact ?? string.Empty;
                personalRes.Email = personal.Email;
                personalRes.PhoneNumber = personal.PhoneNumber;
                personalRes.BankNumber = personal.BankNumber;
                personalRes.NameBank = personal.NameBank;

                allPersonal.Add(personalRes);
            }
            return allPersonal;
        }

        public async Task<List<AllPersonelRes>> GetAllPersonel()
        {
            var personels = await _context.PersonelEmployees.ToListAsync();

            var allPersonel = new List<AllPersonelRes>();

            foreach(var personel in personels)
            {
                var personelRes = new AllPersonelRes();
                personelRes.AvatarPath = personel.AvatarPath ?? string.Empty;
                personelRes.EmployeeCode = personel.EmployeeCode;

                var personal = await _context.PersonalEmployees.FirstOrDefaultAsync(e => e.EmployeeCode == personel.EmployeeCode);

                if (personal != null)
                {
                    personelRes.NameEmployee = personal.NameEmployee ?? string.Empty;
                    personelRes.Email = personal.Email ?? string.Empty;
                    personelRes.PhoneNumber = personal.PhoneNumber ?? string.Empty;
                }                   
                else
                {
                    personelRes.NameEmployee = string.Empty;
                    personelRes.Email = string.Empty;
                    personelRes.PhoneNumber = string.Empty;
                }
                    

                var branch = await _context.Branchs.FirstOrDefaultAsync(b => b.Id == personel.BranchId);

                personelRes.BranchName = branch.BranchName;

                var department = await _context.Departments.FirstOrDefaultAsync(d => d.Id == personel.DepartmentId);

                personelRes.DepartmentName = department.DepartmentName;

                //var jobtitle = await _context.JobTitles.FirstOrDefaultAsync(j => j.Id == personel.JobTitleId);

                //personelRes.JobtitleName = jobtitle.JobTitleName;

                var rank = await _context.Ranks.FirstOrDefaultAsync(r => r.Id == personel.RankId);
                personelRes.RankName = rank.RankName;

                var position = await _context.Positions.FirstOrDefaultAsync(p => p.Id == personel.PositionId);
                personelRes.PositionName = position.PositionName;

                personelRes.DateJoinCompany = personel.DateJoinCompany;
                var jobType = await _context.JobTypes.FirstOrDefaultAsync(j => j.Id == personel.JobTypeId);

                var manager = await _context.PersonalEmployees.FirstOrDefaultAsync(e=>e.EmployeeCode ==  personel.ManagerId);
                if (manager != null)
                    personelRes.NameManager = manager.NameEmployee;
                else
                    personelRes.NameManager = string.Empty;


                personelRes.JobtypeName = jobType.NameJobType;

                var breakLunch = await _context.CheckInOutSettings.FirstOrDefaultAsync();

                personelRes.BreakLunch = breakLunch.BreakHour + ((double)breakLunch.BreakMinute / (double)60);

                allPersonel.Add(personelRes);
            }

            return allPersonel;
        }

        public async Task<List<AllContractRes>> GetAllContract()
        {
            var contracts = await _context.ContractEmployees.ToListAsync();

            var allContract = new List<AllContractRes>();
            foreach(var contract in contracts)
            {
                var contractRes = new AllContractRes();
                contractRes.EmployeeCode = contract.EmployeeCode;
                var personal = await _context.PersonalEmployees.FirstOrDefaultAsync(e => e.EmployeeCode == contract.EmployeeCode);
                if (personal != null)
                    contractRes.NameEmployee = personal.NameEmployee;
                else
                    contractRes.NameEmployee = string.Empty;

                var personel = await _context.PersonelEmployees.FirstOrDefaultAsync(e => e.EmployeeCode == contract.EmployeeCode);
                if(personel != null)
                {
                    contractRes.AvatarPath = personel.AvatarPath ?? string.Empty;
                    var branch = await _context.Branchs.FirstOrDefaultAsync(b => b.Id == personel.BranchId);

                    contractRes.BranchName = branch.BranchName;

                    var department = await _context.Departments.FirstOrDefaultAsync(d => d.Id == personel.DepartmentId);

                    contractRes.DepartmentName = department.DepartmentName;

                    var position = await _context.Positions.FirstOrDefaultAsync(p => p.Id == personel.PositionId);
                    contractRes.PositionName = position.PositionName;

                    var coeficient = await _context.SalaryCoefficients.FirstOrDefaultAsync(c => c.PositionId == personel.PositionId);
                    contractRes.CoefficientSalary = coeficient.SalaryCoefficient;

                }
                else
                {
                    contractRes.AvatarPath = string.Empty;
                    contractRes.BranchName = string.Empty;
                    contractRes.DepartmentName = string.Empty;
                    contractRes.PositionName = string.Empty;
                }

                contractRes.ContractCode = contract.ContractCode ?? string.Empty;
                contractRes.TypeContract = contract.TypeContract ?? string.Empty;
                contractRes.StatusContract = contract.ContractStatus ?? string.Empty;

                var basicSalary = await _context.BasicSettingSalary.FirstOrDefaultAsync();
                contractRes.HourlySalary = basicSalary.HourlySalary;
                contractRes.HourWorkStandard = basicSalary.HourWorkStandard;

                contractRes.StartContract = contract.DateStartContract;
                contractRes.EndContract = contract.DateEndContract;

                allContract.Add(contractRes);
            }
            return allContract;
        }

        public async Task<List<AllInsuranceRes>> GetAllInsurance()
        {
            var insurances = await _context.InsuranceEmployees.ToListAsync();

            var allInsurance = new List<AllInsuranceRes>();
            foreach(var insurance in insurances)
            {
                var insuranceRes = new AllInsuranceRes();
                insuranceRes.EmployeeCode = insurance.EmployeeCode;

                var personal = await _context.PersonalEmployees.FirstOrDefaultAsync(e => e.EmployeeCode == insurance.EmployeeCode);
                if (personal != null)
                    insuranceRes.NameEmployee = personal.NameEmployee;
                else
                    insuranceRes.NameEmployee = string.Empty;

                var personel = await _context.PersonelEmployees.FirstOrDefaultAsync(e => e.EmployeeCode == insurance.EmployeeCode);
                if (personel != null)
                {
                    insuranceRes.AvatarPath = personel.AvatarPath ?? string.Empty;
                    var branch = await _context.Branchs.FirstOrDefaultAsync(b => b.Id == personel.BranchId);

                    insuranceRes.BranchName = branch.BranchName;

                    var department = await _context.Departments.FirstOrDefaultAsync(d => d.Id == personel.DepartmentId);

                    insuranceRes.DepartmentName = department.DepartmentName;

                    var position = await _context.Positions.FirstOrDefaultAsync(p => p.Id == personel.PositionId);
                    insuranceRes.PositionName = position.PositionName;                   
                }
                else
                {
                    insuranceRes.AvatarPath = string.Empty;
                    insuranceRes.BranchName = string.Empty;
                    insuranceRes.DepartmentName = string.Empty;
                    insuranceRes.PositionName = string.Empty;
                }

                insuranceRes.CodeBHYT = insurance.CodeBHYT;
                insuranceRes.CodeBHXH = insurance.CodeBHXH;
                insuranceRes.HasBHXH = insurance.HasBHXH;
                insuranceRes.StatusInsurance = insurance.InsuranceStatus ?? string.Empty;
                insuranceRes.EndInsurance = insurance.DateEndParticipateInsurance;

                var rateInsurance = await _context.RateInsurances.FirstOrDefaultAsync();
                insuranceRes.BusinessRateBHYT = rateInsurance.bhytBusinessRate;
                insuranceRes.EmptRateBHYT = rateInsurance.bhytEmpRate;
                insuranceRes.BussinessRateBHXH = rateInsurance.bhxhBusinessRate;
                insuranceRes.EmptRateBHXH = rateInsurance.bhxhEmpRate;
                insuranceRes.BusinessRateBHTN = rateInsurance.bhtnBusinessRate;
                insuranceRes.EmptRateBHTN = rateInsurance.bhtnEmpRate;

                allInsurance.Add(insuranceRes);
            }

            return allInsurance;
        }

        public async Task<List<AllTaxRes>> GetAllTax()
        {
            var taxs = await _context.TaxEmployees.ToListAsync();

            var allTax = new List<AllTaxRes>();

            foreach(var tax in taxs)
            {
                var taxRes = new AllTaxRes();
                taxRes.EmployeeCode = tax.EmployeeCode;
                var personal = await _context.PersonalEmployees.FirstOrDefaultAsync(e => e.EmployeeCode == tax.EmployeeCode);
                if (personal != null)
                    taxRes.NameEmployee = personal.NameEmployee;
                else
                    taxRes.NameEmployee = string.Empty;

                var personel = await _context.PersonelEmployees.FirstOrDefaultAsync(e => e.EmployeeCode == tax.EmployeeCode);
                if (personel != null)
                {
                    taxRes.AvatarPath = personel.AvatarPath ?? string.Empty;
                    var branch = await _context.Branchs.FirstOrDefaultAsync(b => b.Id == personel.BranchId);

                    taxRes.BranchName = branch.BranchName;

                    var department = await _context.Departments.FirstOrDefaultAsync(d => d.Id == personel.DepartmentId);

                    taxRes.DepartmentName = department.DepartmentName;

                    var position = await _context.Positions.FirstOrDefaultAsync(p => p.Id == personel.PositionId);
                    taxRes.PositionName = position.PositionName;
                }
                else
                {
                    taxRes.AvatarPath = string.Empty;
                    taxRes.BranchName = string.Empty;
                    taxRes.DepartmentName = string.Empty;
                    taxRes.PositionName = string.Empty;
                }

                taxRes.HasTax = tax.HasTaxCode;
                taxRes.CodeTax = tax.TaxCode;

                int countDependent = await _context.Dependents.Where(d => d.EmployeeCode == tax.EmployeeCode).CountAsync();
                taxRes.CountDependent = countDependent;
                allTax.Add(taxRes);
            }

            return allTax;
        }

        public async Task<IdentityResult> DeletePersonal(string employeeCode)
        {
            var personal = await _context.PersonalEmployees.FirstOrDefaultAsync(e => e.EmployeeCode == employeeCode);
            if(personal == null)
            {
                return IdentityResult.Failed(new IdentityError
                {
                    Code = "PersonalNotFound",
                    Description = "Personal not found."
                });
            }
            _context.PersonalEmployees.Remove(personal);           

            await _context.SaveChangesAsync();

            return IdentityResult.Success;
        }

        public async Task<IdentityResult> DeletePersonel(string employeeCode)
        {
            var personel = await _context.PersonelEmployees.FirstOrDefaultAsync(e => e.EmployeeCode == employeeCode);
            if(personel == null)
            {
                return IdentityResult.Failed(new IdentityError
                {
                    Code = "PersonelNotFound",
                    Description = "Personel not found."
                });
            }

            _context.PersonelEmployees.Remove(personel);

            var user = await _userManager.FindByNameAsync(employeeCode);
            if (user != null)
            {
                await _userManager.DeleteAsync(user);
            }
              
            await _context.SaveChangesAsync();

            return IdentityResult.Success;
        }

        public async Task<IdentityResult> DeleteContract(string employeeeCode)
        {
            var contract = await _context.ContractEmployees.FirstOrDefaultAsync(e => e.EmployeeCode == employeeeCode);
            if(contract == null)
            {
                return IdentityResult.Failed(new IdentityError
                {
                    Code = "ContractNotFound",
                    Description = "Contract not found."
                });
            }

            _context.ContractEmployees.Remove(contract);

            var allowanceEmpts = await _context.EmployeeAllowances.Where(a => a.EmployeeCode == employeeeCode).ToListAsync();
            
            _context.EmployeeAllowances.RemoveRange(allowanceEmpts);

            await _context.SaveChangesAsync();

            return IdentityResult.Success;
        }

        public async Task<IdentityResult> DeleteInsurance(string employeeCode)
        {
            var insurance = await _context.InsuranceEmployees.FirstOrDefaultAsync(e => e.EmployeeCode == employeeCode);
            if(insurance == null)
            {
                return IdentityResult.Failed(new IdentityError
                {
                    Code = "InsuranceNotFound",
                    Description = "Insurance not found."
                });
            }

            _context.InsuranceEmployees.Remove(insurance);
            await _context.SaveChangesAsync();

            return IdentityResult.Success;
        }

        public async Task<IdentityResult> DeleteTax(string employeeCode)
        {
            var tax = await _context.TaxEmployees.FirstOrDefaultAsync(e => e.EmployeeCode == employeeCode);
            if (tax == null)
            {
                return IdentityResult.Failed(new IdentityError
                {
                    Code = "TaxNotFound",
                    Description = "Tax not found."
                });
            }

            _context.TaxEmployees.Remove(tax);

            var dependents = await _context.Dependents.Where(d => d.EmployeeCode == employeeCode).ToListAsync();

            _context.Dependents.RemoveRange(dependents);

            await _context.SaveChangesAsync();

            return IdentityResult.Success;
        }
    }
}