using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WEB_API_HRM.Data;

namespace WEB_API_HRM.Models
{
    public class JobTitleModel
    {
        [Required]
        [Key]
        public string Id { get; set; } = null!;

        [Required]
        public string JobTitleName { get; set; } = null!;

        public string Description { get; set; } = null!;

        [Required]
        public string RankId { get; set; } = null!;

        [ForeignKey("RankId")]
        public RankModel Rank { get; set; } = null!;
        [Required]
        public string RoleId { get; set; } = null!;

        [ForeignKey("RoleId")]
        public ApplicationRole Role { get; set; } = null!;
    }
}