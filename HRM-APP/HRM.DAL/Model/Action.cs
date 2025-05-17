using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM.DAL.Model
{
    public class Action
    {
        [Required]
        [Key]
        public int Id { get; set; }
        public string NameRole { get; set; }
    }
}
