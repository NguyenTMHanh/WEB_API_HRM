using Humanizer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WEB_API_HRM.Data;
using WEB_API_HRM.DTO;
using WEB_API_HRM.Models;

namespace WEB_API_HRM.Repositories
{
    public class BranchRepository : IBranchRepository
    {
        private readonly HRMContext _context;
        public BranchRepository(HRMContext context)
        {
            _context = context;
        }
        public async Task<IdentityResult> CreateBranchAsync(BranchDto model)
        {
            var existingBranch = await _context.Branchs.AnyAsync(b => b.BranchName == model.BranchName);
            if (existingBranch)
            {
                return IdentityResult.Failed(new IdentityError { Description = "Branch already exists in the system." });
            }

            var branch = new BranchModel
            {
                Id = model.Id,
                BranchName = model.BranchName,
                Address = model.Address,
                Status = model.Status,
            };

            await _context.Branchs.AddAsync(branch);


            var branchDepartments = new List<BranchDepartmentModel>();
            foreach (var departmentName in model.DepartmentName)
            {
                var department = await _context.Departments.FirstOrDefaultAsync(d => d.DepartmentName == departmentName);
                if (department == null)
                {
                    return IdentityResult.Failed(new IdentityError { Description = "Department not found." });
                }
                var branchDepartment = new BranchDepartmentModel
                {
                    BranchId = model.Id,
                    DepartmentId = department.Id,
                };
                branchDepartments.Add(branchDepartment);
            }

            _context.BranchDepartment.AddRange(branchDepartments);
            await _context.SaveChangesAsync();

            return IdentityResult.Success;
        }

        public async Task<IdentityResult> DeleteBranchAsync(string branchId)
        {
            var branch = await _context.Branchs.FirstOrDefaultAsync(b => b.Id == branchId);
            if (branch == null)
            {
                return IdentityResult.Failed(new IdentityError
                {
                    Description = "Branch not found"
                });
            }

            var branchDepartments = _context.BranchDepartment.Where(b => b.BranchId == branchId).ToList();
            _context.BranchDepartment.RemoveRange(branchDepartments);

            _context.Branchs.Remove(branch);
            await _context.SaveChangesAsync();

            return IdentityResult.Success;
        }

        public async Task<IEnumerable<BranchDto>> GetAllBranchesAsync()
        {
            var branches = await _context.Branchs.ToListAsync();

            var branchIds = branches.Select(b => b.Id).ToList();

            var relevantBranchDepartments = await _context.BranchDepartment
                .Where(bd => branchIds.Contains(bd.BranchId))
                .ToListAsync();

            var allDepartments = await _context.Departments.ToListAsync();

            var branchDtos = branches.Select(b =>
            {
                var departmentIds = relevantBranchDepartments
                    .Where(bd => bd.BranchId == b.Id)
                    .Select(bd => bd.DepartmentId)
                    .ToList();

                var departmentNames = new List<string>();
                foreach (var departmentId in departmentIds)
                {
                    var department = allDepartments.FirstOrDefault(d => d.Id == departmentId);
                    if (department != null)
                    {
                        departmentNames.Add(department.DepartmentName);
                    }
                }

                return new BranchDto
                {
                    Id = b.Id,
                    BranchName = b.BranchName,
                    Address = b.Address,
                    Status = b.Status,
                    DepartmentName = departmentNames
                };
            }).ToList();

            return branchDtos;
        }

        public async Task<BranchDto> GetBranchAsync(string branchId)
        {
            var branch = await _context.Branchs
                    .Include(b => b.BranchDepartment)
                    .ThenInclude(bd => bd.Department)
                    .FirstOrDefaultAsync(b => b.Id == branchId);

            if (branch == null)
            {
                return null;
            }
            var departmentIds = new List<string>();
            foreach(var bd in await _context.BranchDepartment.ToListAsync()) {
                if (bd.BranchId == branchId)
                    departmentIds.Add(bd.DepartmentId);
            }

            var departmentName = new List<string>();
            foreach (var departmentId in departmentIds)
            {
                var department = await _context.Departments.FirstOrDefaultAsync(d => d.Id == departmentId);
                departmentName.Add(department.DepartmentName);
            }
            var branchDto = new BranchDto
            {
                Id = branch.Id,
                BranchName = branch.BranchName,
                Address = branch.Address,
                Status = branch.Status,
                DepartmentName = departmentName
            };

            return branchDto;
        }

        public async Task<string> GetCodeBranch()
        {
            try
            {
                var branchFinal = await _context.Branchs
                    .OrderBy(d => d.Id)
                    .LastOrDefaultAsync();

                if (branchFinal == null || string.IsNullOrEmpty(branchFinal.Id))
                {
                    return "BRANCH001";
                }

                string numericPart = branchFinal.Id.Replace("BRANCH", "").Trim();
                if (int.TryParse(numericPart, out int currentNumber))
                {
                    var nextNumber = currentNumber + 1;
                    var rankCode = $"BRANCH{nextNumber:D3}";
                    return rankCode;
                }
                return "BRANCH001";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetCodeBranch: {ex.Message}");
                throw;
            }
        }

        public async Task<IdentityResult> UpdateBranchAsync(string branchId, BranchDto model)
        {
            try
            {
                var branch = await _context.Branchs
                    .Include(b => b.BranchDepartment)
                    .ThenInclude(bd => bd.Department)
                    .FirstOrDefaultAsync(b => b.Id == branchId);

                if (branch == null)
                {
                    return IdentityResult.Failed(new IdentityError { Description = "Branch not found." });
                }
                var branchCheckList = await _context.Branchs.ToListAsync();
                foreach (var branchCheck in branchCheckList)
                {
                    if (branchCheck.BranchName == model.BranchName && branchCheck.Id != branchId)
                    {
                        return IdentityResult.Failed(new IdentityError
                        {
                            Description = "Branch already exists in the system."
                        });
                    }
                }
                branch.BranchName = model.BranchName;
                branch.Address = model.Address;
                branch.Status = model.Status;

                var existingBranchDepartments = _context.BranchDepartment
                    .Where(bd => bd.BranchId == branchId)
                    .ToList();
                _context.BranchDepartment.RemoveRange(existingBranchDepartments);

                var newBranchDepartments = new List<BranchDepartmentModel>();
                foreach (var departmentName in model.DepartmentName)
                {
                    var department = await _context.Departments
                        .FirstOrDefaultAsync(d => d.DepartmentName == departmentName);
                    if (department == null)
                    {
                        return IdentityResult.Failed(new IdentityError { Description = $"Department '{departmentName}' not found." });
                    }
                    var branchDepartment = new BranchDepartmentModel
                    {
                        BranchId = branchId,
                        DepartmentId = department.Id
                    };
                    newBranchDepartments.Add(branchDepartment);
                }
                _context.BranchDepartment.AddRange(newBranchDepartments);

                await _context.SaveChangesAsync();

                return IdentityResult.Success;
            }
            catch (Exception ex)
            {
                return IdentityResult.Failed(new IdentityError { Description = $"Failed to update branch: {ex.Message}" });
            }
        }
    }
}