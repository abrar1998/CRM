using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CRM.Middlewares
{
    // Middleware to determine the user's role and store it in HttpContext.Items
    public class RoleDeterminationMiddleware
    {
        private readonly RequestDelegate _next;

        public RoleDeterminationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            if (context.User.Identity!.IsAuthenticated)
            {
                // Determine the user's role and store it in HttpContext.Items
                if (context.User.IsInRole("Admin"))
                {
                    context.Items["UserRole"] = "Admin";
                }
                else if (context.User.IsInRole("Client"))
                {
                    context.Items["UserRole"] = "Client";
                }
                else if (context.User.IsInRole("Employee"))
                {
                    context.Items["UserRole"] = "Employee";
                }
            }

            // Call the next middleware in the pipeline
            await _next(context);
        }
    }

    // Extension method to add the RoleDeterminationMiddleware to the ASP.NET Core pipeline
    public static class RoleDeterminationMiddlewareExtensions
    {
        public static IApplicationBuilder UseRoleDeterminationMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<RoleDeterminationMiddleware>();
        }
    }
}

