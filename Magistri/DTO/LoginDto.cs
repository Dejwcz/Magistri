using System.ComponentModel.DataAnnotations;

namespace Magistri.DTO {
    public class LoginDto {
        [Display(Name = "User name")]
        public string UserName { get; set; }
        public string Password { get; set; }
        public string ReturnUrl { get; set; }
    }
}
