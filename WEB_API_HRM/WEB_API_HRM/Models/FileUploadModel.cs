using System.ComponentModel.DataAnnotations;

namespace WEB_API_HRM.Models
{
    public class FileUpload
    {
        [Key]
        public string Id { get; set; }

        [Required]
        [MaxLength(255)]
        public string FileName { get; set; }

        [Required]
        [MaxLength(255)]
        public string UniqueFileName { get; set; }

        [Required]
        [MaxLength(500)]
        public string FilePath { get; set; }

        [Required]
        [MaxLength(100)]
        public string ContentType { get; set; }

        [Required]
        public long FileSize { get; set; }

        [Required]
        public DateTime UploadDate { get; set; }
    }
}