using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WEB_API_HRM.Data;
using WEB_API_HRM.Models;

namespace WEB_API_HRM.Repositories
{
    public class AllowanceRepository : IAllowanceRepository
    {
        private readonly HRMContext _context;
        public AllowanceRepository(HRMContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<AllowanceModel>> GetAllAllowanceAsync()
        {
            return await _context.Allowances.ToListAsync();
        }

        public async Task<IdentityResult> UpdateAllowance(List<AllowanceModel> models)
        {
            var existingAllowanceList = await _context.Allowances.ToListAsync();
            foreach (var allowance in existingAllowanceList)
            {
                _context.Allowances.Remove(allowance);
            }
            foreach (var model in models)
            {
                if (model == null) continue;
                var newAllowance = new AllowanceModel();
                newAllowance.Id = Guid.NewGuid().ToString();
                newAllowance.NameAllowance = model.NameAllowance;
                newAllowance.MoneyAllowance = model.MoneyAllowance;
                newAllowance.TypeAllowance = model.TypeAllowance;
                newAllowance.MoneyAllowanceNoTax = model.MoneyAllowanceNoTax;

                _context.Allowances.Add(newAllowance);
            }

            await _context.SaveChangesAsync();
            return IdentityResult.Success;
        }
    }
}
