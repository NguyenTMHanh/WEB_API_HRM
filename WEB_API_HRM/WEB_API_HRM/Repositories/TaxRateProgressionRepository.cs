using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WEB_API_HRM.Data;
using WEB_API_HRM.Models;

namespace WEB_API_HRM.Repositories
{
    public class TaxRateProgressionRepository : ITaxRateProgressionRepository
    {
        private readonly HRMContext _context;
        public TaxRateProgressionRepository(HRMContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<TaxRateProgressionModel>> GetAllTaxRateProgression()
        {
            return await _context.TaxRateProgressions.ToListAsync();
        }

        public async Task<IdentityResult> UpdateTaxRateProgression(List<TaxRateProgressionModel> models)
        {
            var existingTaxRateProgressionList = await _context.TaxRateProgressions.ToListAsync(); 
            foreach(var taxRateProgression in existingTaxRateProgressionList)
            {
                _context.TaxRateProgressions.Remove(taxRateProgression);
            }
            foreach(var taxRateProgression in models)
            {
                if (taxRateProgression == null) continue;
                var newTaxRateProgression = new TaxRateProgressionModel();
                newTaxRateProgression.Id = Guid.NewGuid().ToString();
                newTaxRateProgression.TaxableIncome = taxRateProgression.TaxableIncome;
                newTaxRateProgression.TaxRate = taxRateProgression.TaxRate;
                newTaxRateProgression.Progression = taxRateProgression.Progression;

                _context.TaxRateProgressions.Add(newTaxRateProgression);
            }
            await _context.SaveChangesAsync();
            return IdentityResult.Success;
        }
    }
}
