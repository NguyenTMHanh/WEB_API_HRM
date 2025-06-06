using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WEB_API_HRM.Models
{
    public class InsuranceEmployeeModel
    {
        [Key]
        [Required]
        public string Id { get; set; }
        public string CodeBHYT { get; set; }
        [Required]
        public string RegisterMedical { get; set; }
        [Required]
        public DateTime DateStartParticipateBHYT { get; set; }

        public bool HasBHXH { get; set; }
        [Required]
        public string CodeBHXH { get; set; }
        [Required]
        public DateTime DateStartParticipateBHXH { get; set; }
        [Required]
        public DateTime DateStartParticipateBHTN { get; set; }
        [Required]
        public string InsuranceStatus { get; set; }
        [Required]
        public DateTime DateEndParticipateInsurance { get; set; }
        [Required]
        public string EmployeeCode { get; set; }
        [ForeignKey("EmployeeCode")]
        public EmployeeModel Employee { get; set; }
    }
}
