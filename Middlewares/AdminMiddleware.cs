using CRM.Models.Registration;
using CRM.Repositories.AdminRepository;
using CRM.Repositories.ClientRepository;
using CRM.Repositories.EmployeeRepository;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CRM.Middlewares
{
    public class AdminMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public AdminMiddleware(RequestDelegate next, IServiceScopeFactory serviceScopeFactory)
        {
            _next = next;
            _serviceScopeFactory = serviceScopeFactory;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            if (httpContext.User.Identity!.IsAuthenticated)
            {
                var userId = httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var adminRepo = scope.ServiceProvider.GetRequiredService<IAdminRepo>();
                    var adminDetails = await adminRepo.GetAdminRegistrationDetails(userId!);

                    if (adminDetails != null)
                    {
                        httpContext.Items["adminName"] = adminDetails.Name;
                        httpContext.Items["adminphoto"] = adminDetails.PhotoPath;
                    }
                    else
                    {
                        // Handle cases where the user is not an admin, if necessary.
                        // For example, you might want to set default values or handle other roles.
                        httpContext.Items["adminName"] = "Unknown";
                        httpContext.Items["adminphoto"] = "/path/to/default/photo.png"; // Default photo path
                    }
                }
            }

            await _next(httpContext);


        }


        //public async Task Invoke(HttpContext httpContext)
        //{
        //    if (httpContext.User.Identity!.IsAuthenticated)
        //    {
        //        var userId = httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

        //        using (var scope = _serviceScopeFactory.CreateScope())
        //        {
        //            if (httpContext.User.IsInRole(RolesClass.Admin))
        //            {
        //                var adminRepo = scope.ServiceProvider.GetRequiredService<IAdminRepo>();
        //                var adminDetails = await adminRepo.GetAdminRegistrationDetails(userId!);

        //                if (adminDetails != null)
        //                {
        //                    httpContext.Items["userName"] = adminDetails.Name;
        //                    httpContext.Items["userPhoto"] = adminDetails.PhotoPath;
        //                }
        //            }
        //            else if (httpContext.User.IsInRole("Employee"))
        //            {
        //                var employeeRepo = scope.ServiceProvider.GetRequiredService<IEmployeeRepo>();
        //                var employeeDetails = await employeeRepo.GetEmployeeDetails(userId!);

        //                if (employeeDetails != null)
        //                {
        //                    httpContext.Items["userName"] = employeeDetails.Name;
        //                    httpContext.Items["userPhoto"] = employeeDetails.PhotoPath;
        //                }
        //            }
        //            else if (httpContext.User.IsInRole("Client"))
        //            {
        //                var clientRepo = scope.ServiceProvider.GetRequiredService<IClientRepo>();
        //                //var clientDetails = await clientRepo.GetClientDetails(userId!);

        //                //if (clientDetails != null)
        //                //{
        //                //    httpContext.Items["userName"] = clientDetails.Name;
        //                //    httpContext.Items["userPhoto"] = clientDetails.PhotoPath;
        //                //}
        //            }
        //            else
        //            {
        //                // Handle cases where the user's role is not recognized
        //                httpContext.Items["userName"] = "Unknown User";
        //                httpContext.Items["userPhoto"] = "/path/to/default/photo.png"; // Default photo path
        //            }
        //        }
        //    }

        //    await _next(httpContext);
        //}




    }

    public static class AdminMiddlewareExtensions
    {
        public static IApplicationBuilder UseAdminMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<AdminMiddleware>();
        }
    }
}
