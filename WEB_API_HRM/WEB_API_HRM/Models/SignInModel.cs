using System.ComponentModel.DataAnnotations;

namespace WEB_API_HRM.Models
{
    public class SignInModel
    {
        [Required]
        public string username { get; set; } = null!;
        [Required]
        public string password { get; set; } = null!;
    }
}
