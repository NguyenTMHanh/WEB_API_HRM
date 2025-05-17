using System.ComponentModel.DataAnnotations;

namespace WEB_API_HRM.Models
{
    public class ActionModel
    {
        [Required]
        [Key]
        public string Id { get; set; } = null!;
        [Required]
        public string ActionName { get; set; }
    }
}
