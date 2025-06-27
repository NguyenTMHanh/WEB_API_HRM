using Humanizer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WEB_API_HRM.Data;
using WEB_API_HRM.Models;

namespace WEB_API_HRM.Repositories
{
    public class DepartmentRepository : IDepartmentRepository
    {
        private readonly HRMContext _context;
        public DepartmentRepository(HRMContext context)
        {
            _context = context; 
        }
        public async Task<IdentityResult> CreateDepartmentAsync(DepartmentModel model)
        {
            var existingDepartment = await _context.Departments.AnyAsync(d => d.DepartmentName == model.DepartmentName);
            if(existingDepartment)
            {
                return IdentityResult.Failed(new IdentityError { Description = "Department already exists in the system." });
            }

            var department = new DepartmentModel
            {
                Id = model.Id,
                DepartmentName = model.DepartmentName,
                Description = model.Description,
            };

            await _context.Departments.AddAsync(department);
            await _context.SaveChangesAsync();

            return IdentityResult.Success;
        }

        public async Task<IdentityResult> DeleteDepartmentAsync(string departmentId)
        {
            var department = await _context.Departments.FirstOrDefaultAsync(d => d.Id == departmentId);
            if(department == null)
            {
                return IdentityResult.Failed(new IdentityError
                {
                    Description = "Department not found"
                });
            }
            _context.Departments.Remove(department);
            await _context.SaveChangesAsync();
            return IdentityResult.Success;
        }

        public async Task<IEnumerable<DepartmentModel>> GetAllDepartmentsAsync()
        {
            return await _context.Departments.ToListAsync();
        }

        public async Task<string> GetCodeDepartment()
        {
            try
            {
                var departmentFinal = await _context.Departments
                    .OrderBy(d => d.Id)
                    .LastOrDefaultAsync();

                if (departmentFinal == null || string.IsNullOrEmpty(departmentFinal.Id))
                {
                    return "DEP001";
                }

                string numericPart = departmentFinal.Id.Replace("DEP", "").Trim();
                if (int.TryParse(numericPart, out int currentNumber))
                {
                    var nextNumber = currentNumber + 1;
                    var rankCode = $"DEP{nextNumber:D3}";
                    return rankCode;
                }
                return "DEP001";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetCodeDepartment: {ex.Message}");
                throw;
            }
        }

        public async Task<DepartmentModel> GetDepartmentAsync(string departmentId)
        {
            return await _context.Departments.FirstOrDefaultAsync(d => d.Id == departmentId);
        }

        public async Task<IdentityResult> UpdateDepartmentAsync(string departmentId, DepartmentModel model)
        {
            var department = await _context.Departments.FirstOrDefaultAsync(d => d.Id == departmentId);
            if(department == null)
            {
                return IdentityResult.Failed(new IdentityError
                {
                    Description = "Department not found"
                });
            }
            var departmentCheckList = await _context.Departments.ToListAsync();
            foreach (var departmentCheck in departmentCheckList)
            {
                if (departmentCheck.DepartmentName == model.DepartmentName && departmentCheck.Id != departmentId)
                {
                    return IdentityResult.Failed(new IdentityError
                    {
                        Description = "Department already exists in the system."
                    });
                }
            }
            department.DepartmentName = model.DepartmentName;
            department.Description = model.Description;

            _context.Departments.Update(department);
            await _context.SaveChangesAsync();
            return IdentityResult.Success;
        }
    }
}
