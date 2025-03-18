using System.ComponentModel.DataAnnotations;

namespace Auth_System_Project.Model
{
    public class SignInRequest
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
    }

    public class SignInResponse
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
    }
}
