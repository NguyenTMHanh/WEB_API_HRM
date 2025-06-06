using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using WEB_API_HRM.Data;

namespace WEB_API_HRM.Models
{
    public class PersonelEmployeeModel
    {
        [Key]
        [Required]
        public string Id { get; set; }
        [Required]
        public DateTime DateJoinCompany { get; set; }
        [Required]
        public string BranchId { get; set; }
        [ForeignKey("BranchId")]
        public BranchModel Branch { get; set; }
        [Required]
        public string DepartmentId { get; set; }
        [ForeignKey("DepartmentId")]
        public DepartmentModel Department { get; set; }
        [Required]
        public string JobTitleId { get; set; }
        [ForeignKey("JobTitleId")]
        public JobTitleModel JobTitle { get; set; }
        [Required]
        public string RankId { get; set; }
        [ForeignKey("RankId")]
        public RankModel Rank { get; set; }
        [Required]
        public string PositionId { get; set; }
        [ForeignKey("PositionId")]
        public PositionModel Position { get; set; }
        [Required]
        public string ManagerId { get; set; }
        [Required]
        public string JobTypeId { get; set; }
        public string AvatarPath { get; set; }
        [Required]
        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }
        [Required]
        public string RoleId { get; set; }
        [ForeignKey("RoleId")]
        public ApplicationRole Role { get; set; }
        [Required]
        public string EmployeeCode { get; set; }
        [ForeignKey("EmployeeCode")]
        public EmployeeModel Employee { get; set; }
    }
}
