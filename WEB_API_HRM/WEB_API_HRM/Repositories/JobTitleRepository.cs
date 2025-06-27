using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WEB_API_HRM.Data;
using WEB_API_HRM.Models;
using WEB_API_HRM.DTO;

namespace WEB_API_HRM.Repositories
{
    public class JobTitleRepository : IJobTitleRepository
    {
        private readonly HRMContext _context;
        private readonly RoleManager<ApplicationRole> _roleManager;

        public JobTitleRepository(RoleManager<ApplicationRole> roleManager, HRMContext context)
        {
            _roleManager = roleManager;
            _context = context;
        }

        public async Task<IdentityResult> CreateJobTitleAsync(JobTitleDto dto)
        {
            var existingJobTitle = await _context.JobTitles.AnyAsync(j => j.JobTitleName == dto.JobtitleName);
            if (existingJobTitle)
            {
                return IdentityResult.Failed(new IdentityError { Description = "JobTitle already exists in the system." });
            }

            var rank = await _context.Ranks.FirstOrDefaultAsync(r => r.RankName == dto.RankName);
            if (rank == null)
            {
                return IdentityResult.Failed(new IdentityError { Description = "Rank not found." });
            }

            var role = await _roleManager.FindByNameAsync(dto.RoleName);
            if (role == null)
            {
                return IdentityResult.Failed(new IdentityError { Description = "Role not found." });
            }

            var jobtitle = new JobTitleModel
            {
                Id = dto.Id,
                JobTitleName = dto.JobtitleName,
                Description = dto.Description,
                RankId = rank.Id, 
                RoleId = role.Id  
            };

            await _context.JobTitles.AddAsync(jobtitle);
            await _context.SaveChangesAsync();

            return IdentityResult.Success;
        }

        public async Task<IdentityResult> DeleteJobTitleAsync(string jobtitleId)
        {
            var jobtitle = await _context.JobTitles.FirstOrDefaultAsync(j => j.Id == jobtitleId);
            if (jobtitle == null)
            {
                return IdentityResult.Failed(new IdentityError
                {
                    Description = "Jobtitle not found"
                });
            }

            _context.JobTitles.Remove(jobtitle);
            await _context.SaveChangesAsync();

            return IdentityResult.Success;
        }

        public async Task<IEnumerable<JobTitleDto>> GetAllJobsAsync()
        {
            return await _context.JobTitles
                .Include(j => j.Rank) 
                .Include(j => j.Role) 
                .Select(j => new JobTitleDto
                {
                    Id = j.Id,
                    JobtitleName = j.JobTitleName,
                    Description = j.Description,
                    RankName = j.Rank.RankName,
                    RoleName = j.Role.Name      
                })
                .ToListAsync();
        }

        public async Task<string> GetCodeJobTitle()
        {
            try
            {
                var jobtitleFinal = await _context.JobTitles
                    .OrderBy(j => j.Id)
                    .LastOrDefaultAsync();

                if (jobtitleFinal == null || string.IsNullOrEmpty(jobtitleFinal.Id))
                {
                    return "JOBTITLE001";
                }

                string numericPart = jobtitleFinal.Id.Replace("JOBTITLE", "").Trim();
                if (int.TryParse(numericPart, out int currentNumber))
                {
                    var nextNumber = currentNumber + 1;
                    var jobTitleCode = $"JOBTITLE{nextNumber:D3}";
                    return jobTitleCode;
                }
                return "JOBTITLE001";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetCodeJobtitle: {ex.Message}");
                throw;
            }
        }

        public async Task<JobTitleDto> GetJobTitleAsync(string jobtitleId)
        {
            var jobtitle = await _context.JobTitles
                .Include(j => j.Rank)
                .Include(j => j.Role)
                .Where(j => j.Id == jobtitleId)
                .Select(j => new JobTitleDto
                {
                    Id = j.Id,
                    JobtitleName = j.JobTitleName,
                    Description = j.Description,
                    RankName = j.Rank.RankName,
                    RoleName = j.Role.Name
                })
                .FirstOrDefaultAsync();

            return jobtitle;
        }

        public async Task<IdentityResult> UpdateJobTitleAsync(string jobtitleId, JobTitleDto dto)
        {
            var jobtitle = await _context.JobTitles.FirstOrDefaultAsync(j => j.Id == jobtitleId);
            if (jobtitle == null)
            {
                return IdentityResult.Failed(new IdentityError
                {
                    Description = "JobTitle not found"
                });
            }
            var jobtitleCheckList = await _context.JobTitles.ToListAsync();
            foreach (var jobtitleCheck in jobtitleCheckList)
            {
                if (jobtitleCheck.JobTitleName == dto.JobtitleName && jobtitleCheck.Id != jobtitleId)
                {
                    return IdentityResult.Failed(new IdentityError
                    {
                        Description = "JobTitle already exists in the system."
                    });
                }
            }
            var rank = await _context.Ranks.FirstOrDefaultAsync(r => r.RankName == dto.RankName);
            if (rank == null)
            {
                return IdentityResult.Failed(new IdentityError { Description = "Rank not found." });
            }

            var role = await _roleManager.FindByNameAsync(dto.RoleName);
            if (role == null)
            {
                return IdentityResult.Failed(new IdentityError { Description = "Role not found." });
            }

            jobtitle.JobTitleName = dto.JobtitleName;
            jobtitle.Description = dto.Description;
            jobtitle.RankId = rank.Id;
            jobtitle.RoleId = role.Id;

            await _context.SaveChangesAsync();

            return IdentityResult.Success;
        }
    }
}