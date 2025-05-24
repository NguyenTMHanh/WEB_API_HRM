using Microsoft.AspNetCore.Identity;
using WEB_API_HRM.Models;

namespace WEB_API_HRM.Repositories
{
    public interface IRankRepository
    {
        Task<IdentityResult> CreateRankAsync(RankModel model);
        Task<IdentityResult> DeleteRankAsync(string rankId);
        Task<IdentityResult> UpdateRankAsync(string rankId, RankModel model);
        Task<IEnumerable<RankModel>> GetAllRanksAsync();
        Task<RankModel> GetRankAsync(string rankId);
        Task<string> GetCodeRank();
    }
}
