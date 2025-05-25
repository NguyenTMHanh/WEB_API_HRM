using Microsoft.AspNetCore.Identity;
using WEB_API_HRM.DTO;

namespace WEB_API_HRM.Repositories
{
    public interface IBranchRepository
    {
        Task<IdentityResult> CreateBranchAsync(BranchDto model);
        Task<IdentityResult> UpdateBranchAsync(string branchId, BranchDto model);
        Task<IdentityResult> DeleteBranchAsync(string branchId);
        Task<IEnumerable<BranchDto>> GetAllBranchesAsync();
        Task<BranchDto> GetBranchAsync(string branchId);
        Task<string> GetCodeBranch();
    }
}
