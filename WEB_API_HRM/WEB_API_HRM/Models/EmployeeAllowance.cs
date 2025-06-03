using System.ComponentModel.DataAnnotations;

namespace WEB_API_HRM.Models
{
    public class EmployeeAllowance
    {
        [Required]
        public string AllowanceId { get; set; }
        [Required]
        public string EmployeeCode { get; set; }
        public AllowanceModel Allowance { get; set; }
        public EmployeeModel Employee { get; set; }
    }
}
