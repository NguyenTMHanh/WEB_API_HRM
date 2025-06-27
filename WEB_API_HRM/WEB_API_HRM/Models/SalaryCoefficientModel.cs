using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WEB_API_HRM.Models
{
    public class SalaryCoefficientModel
    {
        [Key]
        [Required]
        public string Id { get; set; }
        [Required]
        public double SalaryCoefficient {  get; set; }
        [Required]
        public string PositionId { get; set; }
        [ForeignKey("PositionId")]
        public PositionModel Position { get; set; }
    }
}
