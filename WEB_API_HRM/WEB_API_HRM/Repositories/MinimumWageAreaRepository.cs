using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WEB_API_HRM.Data;
using WEB_API_HRM.Models;

namespace WEB_API_HRM.Repositories
{
    public class MinimumWageAreaRepository : IMinimumWageAreaRepository
    {
        private readonly HRMContext _context;
        public MinimumWageAreaRepository (HRMContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<MinimumWageAreaModel>> GetAllMinimumWageAreaAsync()
        {
            return await _context.MinimumWageAreas.ToListAsync();
        }

        public async Task<IdentityResult> UpdateMinimumWageArea(List<MinimumWageAreaModel> models)
        {
            var existingMinimumWageAreaList = await _context.MinimumWageAreas.ToListAsync();
            foreach (var minimumWageArea in existingMinimumWageAreaList)
            {
                _context.MinimumWageAreas.Remove(minimumWageArea);
            }
            foreach (var model in models)
            {
                if (model == null) continue;
                var newMinimumWageArea= new MinimumWageAreaModel();
                newMinimumWageArea.Id = Guid.NewGuid().ToString();
                newMinimumWageArea.NameArea = model.NameArea;
                newMinimumWageArea.MoneyMinimumWageArea = model.MoneyMinimumWageArea;

                _context.MinimumWageAreas.Add(newMinimumWageArea);
            }

            await _context.SaveChangesAsync();
            return IdentityResult.Success;
        }
    }
}
