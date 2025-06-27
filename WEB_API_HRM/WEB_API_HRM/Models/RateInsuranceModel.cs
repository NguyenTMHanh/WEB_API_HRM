using System.ComponentModel.DataAnnotations;
namespace WEB_API_HRM.Models
{
    public class RateInsuranceModel
    {
        [Key]
        [Required]
        public string Id { get; set; }
        [Required]
        public double bhytBusinessRate { get; set; }
        [Required]
        public double bhytEmpRate { get;set; }
        [Required]
        public double bhxhBusinessRate { get; set; }
        [Required]
        public double bhxhEmpRate { get; set; }
        [Required]
        public double bhtnBusinessRate { get; set; }
        [Required]
        public double bhtnEmpRate { get; set; }
    }
}
