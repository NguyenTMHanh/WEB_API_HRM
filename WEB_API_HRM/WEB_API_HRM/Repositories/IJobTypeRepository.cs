using Microsoft.AspNetCore.Identity;
using WEB_API_HRM.Models;

namespace WEB_API_HRM.Repositories
{
    public interface IJobTypeRepository
    {
        Task<IEnumerable<JobTypeModel>> GetAllJobType();
        Task<IdentityResult> UpdateJobType(List<JobTypeModel> jobtypes);
    }
}
