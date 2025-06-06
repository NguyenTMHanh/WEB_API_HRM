using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WEB_API_HRM.Models
{
    public class TaxEmployeeModel
    {
        [Key]
        [Required]
        public string Id { get; set; }
        public bool HasTaxCode { get; set; }
        [Required]
        public string TaxCode { get; set; }
        [Required]
        public string EmployeeCode { get; set; }
        [ForeignKey("EmployeeCode")]
        public EmployeeModel Employee { get; set; }
    }
}
