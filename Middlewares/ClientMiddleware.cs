using CRM.Repositories.ClientRepository;
using CRM.Repositories.EmployeeRepository;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CRM.Middlewares
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class ClientMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IServiceScopeFactory serviceScopeFactory;

        public ClientMiddleware(RequestDelegate next, IServiceScopeFactory serviceScopeFactory)
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
                    var clientRepo = scope.ServiceProvider.GetRequiredService<IClientRepo>();
                    var clientDetails = await clientRepo.GetCurrentClientDetails(userId!);

                    if (clientDetails != null)
                    {
                        httpContext.Items["userName"] = clientDetails.Name;
                        httpContext.Items["userPhoto"] = clientDetails.PhotoPath;
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
    public static class ClientMiddlewareExtensions
    {
        public static IApplicationBuilder UseClientMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ClientMiddleware>();
        }
    }
}
