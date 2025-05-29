using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Validations;
using Microsoft.VisualBasic;
using WEB_API_HRM.Data;
using WEB_API_HRM.DTO;
using WEB_API_HRM.Models;

namespace WEB_API_HRM.Repositories
{
    public class CheckInOutRepository : ICheckInOutSettingRepository
    {
        private readonly HRMContext _context;
        public CheckInOutRepository(HRMContext context)
        {
            _context = context;
        }
        public async Task<BreakTimeDto> GetBreakTimeAsync()
        {
            var hasSetting = await _context.CheckInOutSettings.AnyAsync();
            if (!hasSetting)
            {
                var settingFirst = new CheckInOutSettingModel
                {
                    Id = Guid.NewGuid().ToString(),
                    BreakHour = 0,
                    BreakMinute = 0,
                    Checkin = TimeSpan.Parse("00:00"),
                    Checkout = TimeSpan.Parse("00:00")
                };
                _context.CheckInOutSettings.Add(settingFirst);
                await _context.SaveChangesAsync();
            }

            var setting = await _context.CheckInOutSettings.FirstOrDefaultAsync();
            return new BreakTimeDto
            {
                BreakHour = setting.BreakHour,
                BreakMinute = setting.BreakMinute,
            };
        }

        public async Task<CheckInOutSetitngDto> GetCheckInOutSettingAsync()
        {
            var hasSetting = await _context.CheckInOutSettings.AnyAsync();
            if (!hasSetting)
            {
                var settingFirst = new CheckInOutSettingModel
                {
                    Id = Guid.NewGuid().ToString(),
                    BreakHour = 0,
                    BreakMinute = 0,
                    Checkin = TimeSpan.Parse("00:00"),
                    Checkout = TimeSpan.Parse("00:00")
                };
                _context.CheckInOutSettings.Add(settingFirst);
                await _context.SaveChangesAsync();
            }

            var setting = await _context.CheckInOutSettings.FirstOrDefaultAsync();
            return new CheckInOutSetitngDto
            {
                Checkin = setting.Checkin,
                Checkout = setting.Checkout
            };
        }

        public async Task<IdentityResult> UpdateBreakTimeSettingAsync(BreakTimeDto model)
        {
            var setting = await _context.CheckInOutSettings.FirstOrDefaultAsync();
            if (setting == null)
            {
                return IdentityResult.Failed(new IdentityError { Description = "Break time setting not found." });
            }
            setting.BreakMinute = model.BreakMinute;
            setting.BreakHour = model.BreakHour;
            await _context.SaveChangesAsync();
            return IdentityResult.Success;
        }

        public async Task<IdentityResult> UpdateCheckInOutSettingAsync(CheckInOutSetitngDto model)
        {
            var setting = await _context.CheckInOutSettings.FirstOrDefaultAsync();
            if (setting == null)
            {
                return IdentityResult.Failed(new IdentityError { Description = "CheckIn CheckOut setting not found." });
            }
            setting.Checkin = model.Checkin;
            setting.Checkout = model.Checkout;
            await _context.SaveChangesAsync();
            return IdentityResult.Success;
        }
    }
}
