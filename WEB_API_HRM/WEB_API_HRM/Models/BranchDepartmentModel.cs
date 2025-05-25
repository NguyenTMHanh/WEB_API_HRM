using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WEB_API_HRM.Models
{
    public class BranchDepartmentModel
    {
        [Required]
        public string DepartmentId { get; set; } = null!;

        [Required]
        public string BranchId { get; set; } = null!;

        [ForeignKey(nameof(DepartmentId))]
        public DepartmentModel Department { get; set; } = null!;

        [ForeignKey(nameof(BranchId))]
        public BranchModel Branch { get; set; } = null!;
    }
}