using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WEB_API_HRM.Data;
using WEB_API_HRM.Models;

namespace WEB_API_HRM.Repositories
{
    public class HolidayRepository : IHolidayRepository
    {
        private readonly HRMContext _context;
        public HolidayRepository(HRMContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<HolidayModel>> GetAllHoliday()
        {
            return await _context.Holidays.ToListAsync();
        }

        public async Task<IdentityResult> UpdateHoliday(List<HolidayModel> holidays)
        {
            var existingHolidayList = await _context.Holidays.ToListAsync();
            foreach(var holiday in existingHolidayList)
            {
                _context.Holidays.Remove(holiday);
            }
            foreach(var holiday in holidays)
            {
                if (holiday == null) continue;
                var newHoliday = new HolidayModel();
                newHoliday.Id = Guid.NewGuid().ToString();
                newHoliday.HolidayName = holiday.HolidayName;
                newHoliday.FromDate = holiday.FromDate;
                newHoliday.ToDate = holiday.ToDate;

                _context.Holidays.Add(newHoliday);
            }

            await _context.SaveChangesAsync();
            return IdentityResult.Success;
        }
    }
}
