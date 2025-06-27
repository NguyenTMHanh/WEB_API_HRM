using System.ComponentModel.DataAnnotations;

namespace WEB_API_HRM.Models
{
    public class DeductionLevelModel
    {
        [Key]
        [Required]
        public string Id { get; set; }
        [Required]
        public double IndividualDeduction { get; set; }
        [Required]
        public double DependentDeduction { get; set; }
    }
}
