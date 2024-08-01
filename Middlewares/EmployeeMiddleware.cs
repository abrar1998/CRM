using CRM.Repositories.AdminRepository;
using CRM.Repositories.EmployeeRepository;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CRM.Middlewares
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class EmployeeMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IServiceScopeFactory serviceScopeFactory;

        public EmployeeMiddleware(RequestDelegate next, IServiceScopeFactory serviceScopeFactory)
        {
            _next = next;
            this.serviceScopeFactory = serviceScopeFactory;
        }

        public async Task Invoke(HttpContext httpContext)
        {

            if (httpContext.User.Identity!.IsAuthenticated)
            {
                var userId = httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

                using (var scope = serviceScopeFactory.CreateScope())
                {
                    var employeeRepo = scope.ServiceProvider.GetRequiredService<IEmployeeRepo>();
                    var employeeDetails = await employeeRepo.GetEmployeeDetails(userId!);

                    if (employeeDetails != null)
                    {
                        httpContext.Items["userName"] = employeeDetails.Name;
                        httpContext.Items["userPhoto"] = employeeDetails.PhotoPath;
                    }
                    else
                    {
                        // Handle cases where the user is not an admin, if necessary.
                        // For example, you might want to set default values or handle other roles.
                        httpContext.Items["userName"] = "Unknown";
                        httpContext.Items["userPhoto"] = "/path/to/default/photo.png"; // Default photo path
                    }
                }
            }

            await _next(httpContext);
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class EmployeeMiddlewareExtensions
    {
        public static IApplicationBuilder UseEmployeeMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<EmployeeMiddleware>();
        }
    }
}
