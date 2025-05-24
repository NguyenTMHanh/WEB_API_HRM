using Microsoft.AspNetCore.Identity;
using WEB_API_HRM.DTO;

namespace WEB_API_HRM.Repositories
{
    public interface IJobTitleRepository
    {
        Task<IdentityResult> CreateJobTitleAsync(JobTitleDto dto);
        Task<IdentityResult> UpdateJobTitleAsync(string jobtitleId, JobTitleDto dto);
        Task<IdentityResult> DeleteJobTitleAsync(string jobtitleId);
        Task<IEnumerable<JobTitleDto>> GetAllJobsAsync();
        Task<JobTitleDto> GetJobTitleAsync(string jobtitleId);
        Task<string> GetCodeJobTitle();
    }
}