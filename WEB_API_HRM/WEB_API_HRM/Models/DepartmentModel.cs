using System.ComponentModel.DataAnnotations;

namespace WEB_API_HRM.Models
{
    public class DepartmentModel
    {
        [Required]
        [Key]
        public string Id { get; set; } = null!;
        [Required]
        public string DepartmentName { get; set; }

        public string Description { get; set; }
    }
}
