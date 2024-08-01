using CRM.Models.Registration;
using Microsoft.AspNetCore.Identity;

namespace CRM.Repositories.RegistrationRepo
{
    public interface IAccountRepo
    {
        Task<IdentityResult> UserRegisteration(UserSignUpModel umodel);

        Task<SignInResult> UserLogin(UserSigninModel lmodel);
        Task LogOut();
    }
}
