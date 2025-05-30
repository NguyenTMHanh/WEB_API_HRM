using System.ComponentModel.DataAnnotations;

namespace WEB_API_HRM.Models
{
    public class AllowanceModel
    {
        [Key]
        [Required]
        public string Id { get; set; }
        [Required]
        public string NameAllowance{ get; set; }
        [Required]
        public double MoneyAllowance { get; set; }
        [Required]
        public string TypeAllowance { get; set; }
        [Required]
        public double MoneyAllowanceNoTax { get; set; }
    }
}
