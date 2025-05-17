using System.ComponentModel.DataAnnotations;

namespace WEB_API_HRM.Models
{
    public class ModuleModel
    {
        [Required]
        [Key]
        public string Id { get; set; } = null!;
        [Required]
        public string ModuleName { get; set; }
    }
}
