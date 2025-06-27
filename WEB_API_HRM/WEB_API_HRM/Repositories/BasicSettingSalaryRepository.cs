using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WEB_API_HRM.Data;
using WEB_API_HRM.DTO;
using WEB_API_HRM.Models;

namespace WEB_API_HRM.Repositories
{
    public class BasicSettingSalaryRepository : IBasicSettingSalaryRepository
    {
        private readonly HRMContext _context;
        public BasicSettingSalaryRepository(HRMContext context)
        {
            _context = context;
        }

        public async Task<BasicSettingSalaryModel> GetBasicSettingSalaryAsync()
        {
            var hasSetting = await _context.BasicSettingSalary.AnyAsync();
            if (!hasSetting)
            {
                var settingFirst = new BasicSettingSalaryModel
                {
                    Id = Guid.NewGuid().ToString(),
                    HourlySalary = 0,
                    DayWorkStandard = 0, 
                    HourWorkStandard = 0,
                };
                _context.BasicSettingSalary.Add(settingFirst);
                await _context.SaveChangesAsync();
            }

            var setting = await _context.BasicSettingSalary.FirstOrDefaultAsync();
            return new BasicSettingSalaryModel
            {
                HourlySalary = setting.HourlySalary,
                HourWorkStandard = setting.HourWorkStandard,
                DayWorkStandard = setting.DayWorkStandard,
            };
        }

        public async Task<IdentityResult> UpdateBasicSettingSalary(BasicSettingSalaryModel model)
        {
            var setting = await _context.BasicSettingSalary.FirstOrDefaultAsync();
            if (setting == null)
            {
                return IdentityResult.Failed(new IdentityError { Description = "Basic setting salary not found." });
            }
            setting.HourlySalary = model.HourlySalary;
            setting.HourWorkStandard = model.HourWorkStandard;
            setting.DayWorkStandard = model.DayWorkStandard;
            await _context.SaveChangesAsync();
            return IdentityResult.Success;
        }
    }
}
