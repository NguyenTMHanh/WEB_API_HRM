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

            if(!(await _context.JobTitles.AnyAsync(j => j.JobTitleName == model.JobtitleName)))
            {
                return IdentityResult.Failed(new IdentityError
                {
                    Code = "JobtitleNotFound",
                    Description = "Jobtitle not found."
                });
            }
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
            var jobtitle = await _context.JobTitles.FirstOrDefaultAsync(j => j.JobTitleName == model.JobtitleName);
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
            personelEmployee.JobTitleId = jobtitle.Id;
            personelEmployee.JobTitle = jobtitle;
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

        public async Task<CreatePersonalEmployeeDto> GetPersonalInformationAsync(string employeeCode)
        {
            var personal = await _context.PersonalEmployees
                .FirstOrDefaultAsync(e => e.EmployeeCode == employeeCode);

            if (personal == null)
            {
                return null;
            }

            var personalRes = new CreatePersonalEmployeeDto
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
                BranchBank = personal.BranchBank
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
            var jobtitle = await _context.JobTitles.FirstOrDefaultAsync(j => j.Id == personel.JobTitleId);
            var rank = await _context.Ranks.FirstOrDefaultAsync(r => r.Id == personel.RankId);
            var position = await _context.Positions.FirstOrDefaultAsync(p => p.Id == personel.PositionId);
            var manager = await _context.PersonalEmployees.FirstOrDefaultAsync(m => m.EmployeeCode == personel.ManagerId);
            var jobtype = await _context.JobTypes.FirstOrDefaultAsync(j => j.Id == personel.JobTypeId);
            var breakLunch = await _context.CheckInOutSettings.FirstOrDefaultAsync();
            double breakLunchRes = breakLunch.BreakHour + ((double)breakLunch.BreakMinute / (double)60);
            var personelRes = new PersonelInformationRes();
            personelRes.EmployeeCode = personel.EmployeeCode ?? string.Empty;
            personelRes.NameEmployee = personal.NameEmployee ?? string.Empty;
            personelRes.Gender = personal.Gender ?? string.Empty;
            personelRes.DateOfBirth = personal.DateOfBirth;
            personelRes.DateJoinCompany = personel.DateJoinCompany;
            personelRes.BranchName = branch.BranchName ?? string.Empty;
            personelRes.DepartmentName = department.DepartmentName ?? string.Empty;
            personelRes.JobtitleName = jobtitle.JobTitleName ?? string.Empty;
            personelRes.RankName = rank.RankName ?? string.Empty;
            personelRes.PositionName = position.PositionName ?? string.Empty;
            personelRes.ManagerName = manager.NameEmployee ?? string.Empty;
            personelRes.JobTypeName = jobtype.NameJobType ?? string.Empty;
            personelRes.BreakLunch = breakLunchRes;
            personelRes.AvatarPath = personel.AvatarPath ?? string.Empty;

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
    }
}