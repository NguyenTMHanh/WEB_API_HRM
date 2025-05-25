using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WEB_API_HRM.Models
{
    public class PositionModel
    {

        [Required]
        [Key]
        public string Id { get; set; } = null!;
        [Required]
        public string PositionName { get; set; }

        public string Description { get; set; }
        [Required]
        public string DepartmentId { get; set; }
        [ForeignKey("DepartmentId")]
        public DepartmentModel Department { get; set; }
    }
}
