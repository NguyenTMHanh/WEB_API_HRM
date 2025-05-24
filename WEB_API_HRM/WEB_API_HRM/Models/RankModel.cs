using System.ComponentModel.DataAnnotations;

namespace WEB_API_HRM.Models
{
    public class RankModel
    {
        [Required]
        [Key]
        public string Id { get; set; } = null!;
        [Required]
        public int PriorityLevel { get; set; }
        [Required]
        public string RankName { get; set; }

        public string Description { get; set; }
    }
}
