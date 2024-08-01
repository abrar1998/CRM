using CRM.DB_Context;
using CRM.Middlewares;
using CRM.Models.Registration;
using CRM.Repositories.AdminRepository;
using CRM.Repositories.ClientRepository;
using CRM.Repositories.EmployeeRepository;
using CRM.Repositories.RegistrationRepo;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Add services for DbContext
builder.Services.AddDbContext<DataContext>(opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("dbcs")));

// Add services for Identity Framework
builder.Services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<DataContext>();
builder.Services.ConfigureApplicationCookie(opt =>
{
    opt.LoginPath = "/Account/LoginUser";
    opt.AccessDeniedPath = "/Account/AccessDenied";
});

// Add services for repositories
builder.Services.AddTransient<IAccountRepo, AccountRepo>();
builder.Services.AddScoped<IAdminRepo, AdminRepo>();
builder.Services.AddTransient<IEmployeeRepo, EmployeeRepo>();
builder.Services.AddTransient<IClientRepo, ClientRepo>();

// Register IHttpContextAccessor if needed
builder.Services.AddHttpContextAccessor();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

// Use custom middleware to determine user role
app.UseRoleDeterminationMiddleware();

// Apply role-specific middleware based on the user role
app.MapWhen(context => context.Items.ContainsKey("UserRole"), appBuilder =>
{
    appBuilder.MapWhen(ctx => ctx.Items["UserRole"]?.ToString() == "Admin", appBuilder =>
    {
        appBuilder.UseAdminMiddleware();
        appBuilder.UseEndpoints(endpoints => endpoints.MapControllers());
    });

    appBuilder.MapWhen(ctx => ctx.Items["UserRole"]?.ToString() == "Client", appBuilder =>
    {
        appBuilder.UseClientMiddleware();
        appBuilder.UseEndpoints(endpoints => endpoints.MapControllers());
    });

    appBuilder.MapWhen(ctx => ctx.Items["UserRole"]?.ToString() == "Employee", appBuilder =>
    {
        appBuilder.UseEmployeeMiddleware();
        appBuilder.UseEndpoints(endpoints => endpoints.MapControllers());
    });
});

// Default route mapping
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
