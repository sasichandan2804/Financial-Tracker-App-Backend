using System.ComponentModel.DataAnnotations;

namespace Auth_System_Project.Model
{
    public class SignUpRequest
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string ConfirmPassword { get; set; }
        

    }
    public class SignUpResponse
    {
        public bool IsSuccess { get; set; }
        public string Message {  get; set; }
    }
}
