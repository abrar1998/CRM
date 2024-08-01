using CRM.DB_Context;
using CRM.Models.Registration;
using Microsoft.AspNetCore.Identity;

namespace CRM.Repositories.RegistrationRepo
{
    public class AccountRepo: IAccountRepo
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly DataContext dbcontext;

        public AccountRepo(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager,
            RoleManager<IdentityRole> roleManager, DataContext dbcontext)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.roleManager = roleManager;
            this.dbcontext = dbcontext;
        }

        //user registration 
        public async Task<IdentityResult> UserRegisteration(UserSignUpModel umodel)
        {
            var user = new ApplicationUser
            {
                UserName = umodel.Email,
                Email = umodel.Email,
                Name = umodel.Name,
                
            };

            var result = await userManager.CreateAsync(user, umodel.Password);
            if (!string.IsNullOrEmpty(umodel.Roles.ToString()))
            {
                await userManager.AddToRoleAsync(user, umodel.Roles.ToString()!);
            }
            else
            {
                await userManager.AddToRoleAsync(user, RolesClass.Employee);
            }

            return result;
        }


        //login
        public async Task<SignInResult> UserLogin(UserSigninModel lmodel)
        {
            var result = await signInManager.PasswordSignInAsync(lmodel.Email, lmodel.Password, lmodel.RememberMe, false);
            return result;
        }

        public async Task LogOut()
        {
            await signInManager.SignOutAsync();
        }





    }
}
