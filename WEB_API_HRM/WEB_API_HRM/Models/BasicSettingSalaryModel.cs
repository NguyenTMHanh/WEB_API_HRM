using System.ComponentModel.DataAnnotations;

namespace WEB_API_HRM.Models
{
    public class BasicSettingSalaryModel
    {
        [Key]
        [Required] 
        public string Id { get; set; }
        [Required]
        public double HourlySalary { get; set; }
        [Required]
        public double HourWorkStandard { get; set; }
        [Required]
        public double DayWorkStandard { get; set; }
    }
}
