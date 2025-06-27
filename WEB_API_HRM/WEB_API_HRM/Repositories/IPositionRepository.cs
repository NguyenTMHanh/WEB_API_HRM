using Microsoft.AspNetCore.Identity;
using WEB_API_HRM.DTO;

namespace WEB_API_HRM.Repositories
{
    public interface IPositionRepository
    {
        Task<IdentityResult> CreatePositionAsync(PositionDto dto);
        Task<IdentityResult> UpdatePositionAsync(string positionId, PositionDto position);
        Task<IdentityResult> DeletePositionAsync(string positionId);
        Task<IEnumerable<PositionDto>> GetAllPositionsAsync();
        Task<PositionDto> GetPositionAsync(string positionId);
        Task<string> GetCodePosition();
    }
}
