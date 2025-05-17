using System.ComponentModel.DataAnnotations;
using WEB_API_HRM.Data;

namespace WEB_API_HRM.Models
{
    public class RoleModuleActionModel
    {
        [Required]
        public string ModuleId { get; set; } = null!;
        [Required]
        public string ActionId { get; set; } = null!;
        [Required]
        public string RoleId { get; set; } = null!;

        public ModuleModel Module { get; set; }
        public ActionModel Action { get; set; }
        public ApplicationRole Role { get; set; }
    }
}
