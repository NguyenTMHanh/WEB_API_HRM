using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WEB_API_HRM.Models
{
    public class CheckInOutSettingModel
    {
        [Key]
        public string Id { get; set; } 
        [Required]
        public TimeSpan Checkin { get; set; }
        [Required]
        public TimeSpan Checkout { get; set; }
        [Required]
        public int BreakHour { get; set; }
        public int BreakMinute { get; set; }
    }
}
