using Microsoft.AspNetCore.Identity;
using WEB_API_HRM.DTO;
using WEB_API_HRM.Models;
using WEB_API_HRM.RSP;

namespace WEB_API_HRM.Repositories
{
    public interface IEmployeeRepository
    {
        Task<IdentityResult> CreatePersonalEmployeeAsync(CreatePersonalEmployeeDto model);
        Task<IdentityResult> CreatePersonelEmployeeAsync(CreatePersonelEmployeeDto model);
        Task<List<CodeNameEmployeeRes>> GetCodeNameEmployeeAsync();
        Task<List<CodeNameEmployeeRes>> GetCodeNameManagerAsync(string employeeCode, string rankName);
        Task<GenderDayOfBirthRes> GetGenderDayOfBirthAsync(string employeeCode);
        Task<AccountDefaultRes> GetAccountDefaultAsync(string employeeCode);
        Task<string> GetRoleEmployee(string jobtitleName);
        Task<List<CodeNameEmployeeRes>> GetCodeNameEmployeeUnContractAsync();
        Task<string> GetCodeContect(string employeeCode);
        Task<PositionCoeficientRes> GetPositionCoeficient (string employeeCode);
        Task<IdentityResult> CreateContractEmployeeAsync(CreateContractEmployeeDto model);

        Task<RateInsuranceModel> GetRateInsuranceAsync();
        Task<List<CodeNameEmployeeRes>> GetCodeNameEmployeeUnInsuranceAsync();
        Task<IdentityResult> CreateInsuranceEmployeeAsync(CreateInsuranceDto model);
        Task<List<CodeNameEmployeeRes>> GetCodeNameEmployeeUnTaxAsync();
        Task<IdentityResult> CreateTaxEmployeeAsync(CreateTaxDto model);

        Task<CreatePersonalEmployeeDto> GetPersonalInformationAsync(string employeeCode);
        Task<string> GetEmployeeCodeToUsername(string userId);

        Task<PersonelInformationRes> GetPersonelInformationAsync(string employeeCode);

        Task<CreateContractEmployeeDto> GetContractInformationAsync(string employeeCode);

        Task<CreateInsuranceDto> GetInsuranceInformationAsync(string employeeCode);
    }
}
