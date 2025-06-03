using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WEB_API_HRM.Data;

namespace WEB_API_HRM.Models
{
    public class EmployeeModel
    {
        [Key]
        [Required]
        public string EmployeeCode { get; set; }
     
    }
}
