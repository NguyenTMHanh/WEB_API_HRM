using System.ComponentModel.DataAnnotations;

namespace WEB_API_HRM.Models
{
    public class HolidayModel
    {
        [Key]
        [Required]
        public string Id { get; set; }
        [Required]
        public string HolidayName { get; set; }
        [Required]
        public DateTime FromDate { get; set; }
        [Required]
        public DateTime ToDate { get; set; }
    }
}
