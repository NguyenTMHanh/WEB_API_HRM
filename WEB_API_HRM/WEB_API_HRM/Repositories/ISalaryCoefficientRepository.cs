using Microsoft.AspNetCore.Identity;
using WEB_API_HRM.DTO;

namespace WEB_API_HRM.Repositories
{
    public interface ISalaryCoefficientRepository
    {
        Task<IEnumerable<SalaryCoefficientDto>> GetAllSalaryCoefficientAsync();
        Task<IdentityResult> UpdateSalaryCoefficientAsync(List<SalaryCoefficientDto> models);
    }
}
