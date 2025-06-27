using Microsoft.AspNetCore.Identity;
using Microsoft.CodeAnalysis.CSharp.Formatting;
using Microsoft.EntityFrameworkCore;
using WEB_API_HRM.Data;
using WEB_API_HRM.DTO;
using WEB_API_HRM.Models;

namespace WEB_API_HRM.Repositories
{
    public class PositionRepository : IPositionRepository
    {
        private readonly HRMContext _context;
        public PositionRepository(HRMContext context)
        {
            _context = context;
        }

        public async Task<IdentityResult> CreatePositionAsync(PositionDto dto)
        {
            var existingPosition = await _context.Positions.AnyAsync(p => p.PositionName == dto.PositionName);
            if (existingPosition)
            {
                return IdentityResult.Failed(new IdentityError { Description = "Position already exists in the system." });
            }

            var department = await _context.Departments.FirstOrDefaultAsync(d => d.DepartmentName == dto.DepartmentName);
            if (department == null)
            {
                return IdentityResult.Failed(new IdentityError { Description = "Department not found." });
            }

            var position = new PositionModel
            {
                Id = dto.Id,
                PositionName = dto.PositionName,
                Description = dto.Description,
                DepartmentId = department.Id,
            };

            await _context.Positions.AddAsync(position);
            await _context.SaveChangesAsync();

            return IdentityResult.Success;
        }

        public async Task<IdentityResult> DeletePositionAsync(string positionId)
        {
            var position = await _context.Positions.FirstOrDefaultAsync(p => p.Id == positionId);
            if(position == null)
            {
                return IdentityResult.Failed(new IdentityError
                {
                    Description = "Position not found"
                });
            }

            _context.Positions.Remove(position);
            await _context.SaveChangesAsync();

            return IdentityResult.Success;
        }

        public async Task<IEnumerable<PositionDto>> GetAllPositionsAsync()
        {
            return await _context.Positions
                .Include(p => p.Department)
                .Select(p => new  PositionDto
                {
                    Id = p.Id,
                    PositionName = p.PositionName,
                    Description = p.Description,
                    DepartmentName = p.Department.DepartmentName
                })
                .ToListAsync();
        }

        public async Task<string> GetCodePosition()
        {
            try
            {
                var positionFinal = await _context.Positions
                    .OrderBy(p => p.Id)
                    .LastOrDefaultAsync();

                if (positionFinal == null || string.IsNullOrEmpty(positionFinal.Id))
                {
                    return "POSITION001";
                }

                string numericPart = positionFinal.Id.Replace("POSITION", "").Trim();
                if (int.TryParse(numericPart, out int currentNumber))
                {
                    var nextNumber = currentNumber + 1;
                    var jobTitleCode = $"POSITION{nextNumber:D3}";
                    return jobTitleCode;
                }
                return "POSITION001";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetCodePosition: {ex.Message}");
                throw;
            }
        }

        public async Task<PositionDto> GetPositionAsync(string positionId)
        {
            var position = await _context.Positions
                .Include(p => p.Department)
                .Where(p => p.Id == positionId)
                .Select(p => new PositionDto
                {
                    Id = p.Id,
                    PositionName = p.PositionName,
                    Description = p.Description,
                    DepartmentName = p.Department.DepartmentName
                })
                .FirstOrDefaultAsync();

            return position;
        }

        public async Task<IdentityResult> UpdatePositionAsync(string positionId, PositionDto dto)
        {
            var position = await _context.Positions.FirstOrDefaultAsync(p => p.Id == positionId);
            if(position == null)
            {
                return IdentityResult.Failed(new IdentityError
                {
                    Description = "Position not found"
                });
            }
            var positionCheckList = await _context.Positions.ToListAsync();
            foreach (var positionCheck in positionCheckList)
            {
                if (positionCheck.PositionName == dto.PositionName && positionCheck.Id != positionId)
                {
                    return IdentityResult.Failed(new IdentityError
                    {
                        Description = "Position already exists in the system."
                    });
                }
            }
            var department = await _context.Departments.FirstOrDefaultAsync(d => d.DepartmentName == dto.DepartmentName);
            if(department == null)
            {
                return IdentityResult.Failed(new IdentityError { Description = "Department not found." });
            }

            position.Description = dto.Description;
            position.PositionName = dto.PositionName;
            position.DepartmentId = department.Id;

            await _context.SaveChangesAsync();

            return IdentityResult.Success;
        }
    }
}
