using Auth_System_Project.Model;

namespace Auth_System_Project.DataAccess
{
    public interface IAuthDL
    {
        public Task<SignUpResponse> SignUp(SignUpRequest request);
        public Task<SignInResponse> SignIn(SignInRequest request);
    }
}
