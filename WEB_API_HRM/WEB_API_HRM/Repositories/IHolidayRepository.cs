using Microsoft.AspNetCore.Identity;
using WEB_API_HRM.Models;

namespace WEB_API_HRM.Repositories
{
    public interface IHolidayRepository
    {
        Task<IEnumerable<HolidayModel>> GetAllHoliday();
        Task<IdentityResult> UpdateHoliday(List<HolidayModel> holidays);
    }
}
