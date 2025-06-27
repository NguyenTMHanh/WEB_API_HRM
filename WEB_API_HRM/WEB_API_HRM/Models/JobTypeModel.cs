using System.ComponentModel.DataAnnotations;

namespace WEB_API_HRM.Models
{
    public class JobTypeModel
    {
        [Key]
        [Required]
        public string Id { get; set; }
        [Required] 
        public string NameJobType { get; set; }
        [Required]
        public int WorkHourMinimum { get; set; }
        [Required]
        public int WorkMinuteMinimum { get; set; }
    }
}
