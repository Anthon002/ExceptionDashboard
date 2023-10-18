using ExceptionDashboard.Application.Services;
using ExceptionDashboard.Application.Services.IRepositories;
using ExceptionDashboard.Core.Models;
using ExceptionDashboard.Infrastructure.Data;
using ExceptionDashboard.Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.Cookies;
using Hangfire;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"), b => b.MigrationsAssembly("ExceptionDashboard.WebApi"));

});
builder.Services.AddDbContext<AuthenticationDbContext>(options => {
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"), b => b.MigrationsAssembly("ExceptionDashboard.WebApi"));
});

builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.Password.RequireUppercase = false;
}).AddEntityFrameworkStores<AuthenticationDbContext>().AddDefaultTokenProviders().AddRoles<IdentityRole>();

builder.Services.AddScoped<IApplicationService, ApplicationService>();
builder.Services.AddScoped<IExceptionHeaderService, ExceptionHeaderService>();
builder.Services.AddScoped<IExceptionService, ExceptionService>();
builder.Services.AddScoped<IUserAuthenticationService, UserAuthenticationService>();
builder.Services.AddTransient<IApplicationRepository, ApplicationRepository>();
builder.Services.AddTransient<IExceptionHeaderRepository, ExceptionHeaderRepository>();
builder.Services.AddTransient<IExceptionRepository, ExceptionRepository>();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options => options.LoginPath ="/ApplicationView/ViewApplications");
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Admin", policy => policy.RequireRole("Admin"));
});
builder.Services.AddHangfire(x => x.UseSqlServerStorage("Server=localhost\\SQLEXPRESS;Database=ExceptionDashboard;Trusted_Connection=True;TrustServerCertificate=True"));
builder.Services.AddHangfireServer();


var app = builder.Build();

app.UseHangfireDashboard();
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseAuthorization();


app.MapControllers();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Product}/{action=ViewProducts}/{id?}"
);



//app.UseSwagger();
//app.UseSwaggerUI();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.Run();
