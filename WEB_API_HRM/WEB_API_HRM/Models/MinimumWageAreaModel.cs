using System.ComponentModel.DataAnnotations;

namespace WEB_API_HRM.Models
{
    public class MinimumWageAreaModel
    {
        [Key]
        [Required]
        public string Id { get; set; }
        [Required]
        public string NameArea { get; set; }
        [Required]
        public double MoneyMinimumWageArea { get; set; }
    }
}
