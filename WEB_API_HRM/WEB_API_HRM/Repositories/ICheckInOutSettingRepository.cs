using Microsoft.AspNetCore.Identity;
using WEB_API_HRM.Models;
using WEB_API_HRM.DTO;
namespace WEB_API_HRM.Repositories
{
    public interface ICheckInOutSettingRepository
    {
        Task<CheckInOutSetitngDto> GetCheckInOutSettingAsync();
        Task<BreakTimeDto> GetBreakTimeAsync();
        Task<IdentityResult> UpdateCheckInOutSettingAsync(CheckInOutSetitngDto model);
        Task<IdentityResult> UpdateBreakTimeSettingAsync(BreakTimeDto model);
    }
}
