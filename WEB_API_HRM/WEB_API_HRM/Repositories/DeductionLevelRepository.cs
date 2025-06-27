using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Razor.Language.Extensions;
using Microsoft.EntityFrameworkCore;
using WEB_API_HRM.Data;
using WEB_API_HRM.Models;

namespace WEB_API_HRM.Repositories
{
    public class DeductionLevelRepository : IDeductionLevelRepository
    {
        private readonly HRMContext _context;
        public DeductionLevelRepository(HRMContext context)
        {
            _context = context;
        }
        public async Task<DeductionLevelModel> GetDeductionLevelAsync()
        {
            var hasSetting = await _context.DeductionLevel.AnyAsync();
            if(!hasSetting)
            {
                var settingFirst = new DeductionLevelModel
                {
                    Id = Guid.NewGuid().ToString(),
                    IndividualDeduction = 0,
                    DependentDeduction = 0,
                };
                _context.DeductionLevel.Add(settingFirst);
                await _context.SaveChangesAsync();
            }

            var setting = await _context.DeductionLevel.FirstOrDefaultAsync();
            return new DeductionLevelModel
            {
                Id = setting.Id,
                IndividualDeduction = setting.IndividualDeduction,
                DependentDeduction = setting.DependentDeduction,
            };
        }

        public async Task<IdentityResult> UpdateDeductionLevelAsync(DeductionLevelModel model)
        {
            var setting = await _context.DeductionLevel.FirstOrDefaultAsync();
            if (setting == null)
            {
                return IdentityResult.Failed(new IdentityError { Description = "Deduction level setting not found." });
            }
            setting.IndividualDeduction = model.IndividualDeduction;
            setting.DependentDeduction = model.DependentDeduction;
            await _context.SaveChangesAsync();
            return IdentityResult.Success;
        }
    }
}
