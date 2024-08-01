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
//add services for dbcontext
builder.Services.AddDbContext<DataContext>(opt=>opt.UseSqlServer(builder.Configuration.GetConnectionString("dbcs")));
//add servies for identity frame work
builder.Services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<DataContext>();
builder.Services.ConfigureApplicationCookie(opt =>
{
    opt.LoginPath = "/Account/LoginUser";
    opt.AccessDeniedPath = "/Account/AccessDenied";
});


//add services for repositories
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
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

//use custom middlewares
app.UseAdminMiddleware();
app.UseEmployeeMiddleware();
app.UseClientMiddleware();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
