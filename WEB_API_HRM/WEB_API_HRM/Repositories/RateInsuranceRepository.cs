using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WEB_API_HRM.Data;
using WEB_API_HRM.Models;

namespace WEB_API_HRM.Repositories
{
    public class RateInsuranceRepository : IRateInsuranceRepository
    {
        private readonly HRMContext _context;
        public RateInsuranceRepository(HRMContext context)
        {
            _context = context;
        }
        public async Task<RateInsuranceModel> GetRateInsuranceAsync()
        {
            var hasSetting = await _context.RateInsurances.AnyAsync();
            if(!hasSetting)
            {
                var settingFirst = new RateInsuranceModel
                {
                    Id = Guid.NewGuid().ToString(),
                    bhytBusinessRate = 0,
                    bhytEmpRate = 0,
                    bhxhBusinessRate = 0,
                    bhxhEmpRate = 0,
                    bhtnBusinessRate = 0,
                    bhtnEmpRate = 0,
                };
                _context.RateInsurances.Add(settingFirst);
                await _context.SaveChangesAsync();
            }
            var setting = await _context.RateInsurances.FirstOrDefaultAsync();
            return setting;
        }

        public async Task<IdentityResult> UpdateBHTNRateAsync(RateInsuranceModel model)
        {
            var setting = await _context.RateInsurances.FirstOrDefaultAsync();
            if(setting == null)
            {
                return IdentityResult.Failed(new IdentityError { Description = "Rate insurance setting not found." });
            }
            setting.bhtnBusinessRate = model.bhtnBusinessRate;
            setting.bhtnEmpRate = model.bhtnEmpRate;
            await _context.SaveChangesAsync();
            return IdentityResult.Success;
        }

        public async Task<IdentityResult> UpdateBHXHRateAsync(RateInsuranceModel model)
        {
            var setting = await _context.RateInsurances.FirstOrDefaultAsync();
            if (setting == null)
            {
                return IdentityResult.Failed(new IdentityError { Description = "Rate insurance setting not found." });
            }
            setting.bhxhBusinessRate = model.bhxhBusinessRate;
            setting.bhxhEmpRate = model.bhxhEmpRate;
            await _context.SaveChangesAsync();
            return IdentityResult.Success;
        }

        public async Task<IdentityResult> UpdateBHYTRateAsync(RateInsuranceModel model)
        {
            var setting = await _context.RateInsurances.FirstOrDefaultAsync();
            if (setting == null)
            {
                return IdentityResult.Failed(new IdentityError { Description = "Rate insurance setting not found." });
            }
            setting.bhytBusinessRate = model.bhytBusinessRate;
            setting.bhytEmpRate = model.bhytEmpRate;
            await _context.SaveChangesAsync();
            return IdentityResult.Success;
        }
    }
}
