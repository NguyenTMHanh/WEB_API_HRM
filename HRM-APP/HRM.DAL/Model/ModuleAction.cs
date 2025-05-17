using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HRM.DAL.Model
{
    public class ModuleAction
    {
        [Key]
        [Column(Order = 1)]
        public int ModuleId { get; set; }

        [Key]
        [Column(Order = 2)]
        public int ActionId { get; set; }

        [ForeignKey("ModuleId")]
        public Module Module { get; set; }

        [ForeignKey("ActionId")]
        public Action Action { get; set; }
    }
}