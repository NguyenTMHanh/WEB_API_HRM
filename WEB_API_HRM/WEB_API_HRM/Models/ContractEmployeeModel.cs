using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WEB_API_HRM.Models
{
    public class ContractEmployeeModel
    {
        [Key]
        [Required]
        public string Id { get; set; }
        [Required]
        public string ContractCode { get; set; }

        public string TypeContract { get; set; }

        public DateTime DateStartContract { get; set; }

        public DateTime DateEndContract { get; set; }

        public string ContractStatus { get; set; }
        public double HourlySalary { get; set; }
        public double HourWorkStandard { get; set; }
        public double DayWorkStandard { get; set; }
        [Required]
        public double MoneyBasicSalary { get; set; }
        [Required]
        public string SalaryCoefficientId { get; set; }
        [ForeignKey("SalaryCoefficientId")]
        public SalaryCoefficientModel SalaryCoefficient { get; set; }

        public List<EmployeeAllowance> EmployeeAllowances { get; set; }
        [Required]
        public string EmployeeCode { get; set; }
        [ForeignKey("EmployeeCode")]
        public EmployeeModel Employee { get; set; }
    }
}
