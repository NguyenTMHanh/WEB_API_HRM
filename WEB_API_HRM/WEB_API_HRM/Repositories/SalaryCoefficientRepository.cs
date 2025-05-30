using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WEB_API_HRM.Data;
using WEB_API_HRM.DTO;
using WEB_API_HRM.Models;

namespace WEB_API_HRM.Repositories
{
    public class SalaryCoefficientRepository : ISalaryCoefficientRepository
    {
        private readonly HRMContext _context;
        public SalaryCoefficientRepository(HRMContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<SalaryCoefficientDto>> GetAllSalaryCoefficientAsync()
        {
            return await _context.SalaryCoefficients
                .Include(s => s.Position)
                .Select(s => new SalaryCoefficientDto
                {
                    Id = s.Id,
                    SalaryCoefficient = s.SalaryCoefficient,
                    PositionName = s.Position.PositionName
                })
                .ToListAsync();
        }

        public async Task<IdentityResult> UpdateSalaryCoefficientAsync(List<SalaryCoefficientDto> models)
        {
            try
            {
                var existingCoefficients = await _context.SalaryCoefficients.ToListAsync();
                foreach (var salaryCoefficient in existingCoefficients)
                {
                    _context.SalaryCoefficients.Remove(salaryCoefficient);
                }
                await _context.SaveChangesAsync(); 

                foreach (var model in models)
                {
                    if (model == null) continue;
                    var position = await _context.Positions
                        .FirstOrDefaultAsync(s => s.PositionName == model.PositionName);
                    if (position == null)
                    {
                        return IdentityResult.Failed(new IdentityError
                        {
                            Description = "Position not found."
                        });
                    }

                    var newSalaryCoefficient = new SalaryCoefficientModel
                    {
                        Id = model.Id,
                        SalaryCoefficient = model.SalaryCoefficient,
                        PositionId = position.Id 
                    };

                    _context.SalaryCoefficients.Add(newSalaryCoefficient); 
                }

                await _context.SaveChangesAsync();
                return IdentityResult.Success;
            }
            catch (Exception ex)
            {
                return IdentityResult.Failed(new IdentityError
                {
                    Description = $"An error occurred while updating salary coefficients: {ex.Message}"
                });
            }
        }
    }
}
