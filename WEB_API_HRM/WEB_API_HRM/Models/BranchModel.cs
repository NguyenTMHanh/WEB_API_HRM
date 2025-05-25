using Microsoft.Extensions.Logging.Abstractions;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WEB_API_HRM.Models
{
    public class BranchModel
    {
        [Required]
        [Key]
        public string Id { get; set; } = null!;
        [Required]
        public string BranchName { get; set; } = null!;
        [Required]
        public string Address { get; set; } = null!;
        [Required]
        public string Status { get; set; } = null!;

        public ICollection<BranchDepartmentModel> BranchDepartment { get; set; }
    }
}
