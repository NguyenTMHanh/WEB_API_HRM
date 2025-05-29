using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WEB_API_HRM.Data;
using WEB_API_HRM.Models;

namespace WEB_API_HRM.Repositories
{
    public class JobTypeRepository : IJobTypeRepository
    {
        private readonly HRMContext _context;
        public JobTypeRepository(HRMContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<JobTypeModel>> GetAllJobType()
        {
            return await _context.JobTypes.ToListAsync();
        }

        public async Task<IdentityResult> UpdateJobType(List<JobTypeModel> jobtypes)
        {
            var existingJobTypeList = await _context.JobTypes.ToListAsync();
            foreach (var jobtype in existingJobTypeList)
            {
                _context.JobTypes.Remove(jobtype);
            }
            foreach (var jobtype in jobtypes)
            {
                if (jobtype == null) continue;
                var newJobtype = new JobTypeModel();
                newJobtype.Id = Guid.NewGuid().ToString();
                newJobtype.NameJobType = jobtype.NameJobType;
                newJobtype.WorkHourMinimum = jobtype.WorkHourMinimum;
                newJobtype.WorkMinuteMinimum = jobtype.WorkMinuteMinimum;

                _context.JobTypes.Add(newJobtype);
            }

            await _context.SaveChangesAsync();
            return IdentityResult.Success;          
        }
    }
}
