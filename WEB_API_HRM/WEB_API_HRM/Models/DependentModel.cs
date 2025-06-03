using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WEB_API_HRM.Models
{
    public class DependentModel
    {
        [Key]
        [Required]
        public string Id { get; set; }
        [Required]
        public string RegisterDependentStatus { get; set; }
  
        public string TaxCode { get; set; }
        [Required]
        public string NameDependent { get; set; }
        [Required]
        public DateTime DayOfBirthDependent { get; set; }
        [Required]
        public string Relationship { get; set; }
        [Required]
        public string EvidencePath { get; set; }
        [Required]
        public string EmployeeId { get; set; }
        [ForeignKey("EmployeeId")]
        public EmployeeModel Employee { get; set; }
    }
}
