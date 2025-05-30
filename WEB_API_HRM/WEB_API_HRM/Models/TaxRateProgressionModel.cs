using System.ComponentModel.DataAnnotations;

namespace WEB_API_HRM.Models
{
    public class TaxRateProgressionModel
    {
        [Key]
        [Required]
        public string Id { get; set; }
        [Required]
        public double TaxableIncome { get; set; }

        [Required]
        public double TaxRate { get; set; }
        [Required]
        public double Progression { get; set; }
    }
}
