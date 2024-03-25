using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ShuffleLit.Data;
using ShuffleLit.Helpers;
using ShuffleLit.Interfaces;
using ShuffleLit.Models;
using ShuffleLit.Repository;
using ShuffleLit.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

//  CUSTOM SERVICES
//  for all auth related actions
builder.Services.AddScoped<IUserRepository, UserRepository>();
//  crud
builder.Services.AddScoped<ILiteratureRepository, LiteratureRepository>();
//      SMTP
//  Gmail Smtp service
builder.Services.AddTransient<IGSMTPService, GSMTPService>();


//  SQL server
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});
//  Identity framework
builder.Services.AddIdentity<AppUser, IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();
builder.Services.AddMemoryCache();
//      cookie auth
builder.Services.AddSession();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie();

//  reset password config
builder.Services.Configure<IdentityOptions>(opt =>
{
    opt.Password.RequiredLength = 5;
    opt.Password.RequireLowercase = true;
    //  lockout timespan
    opt.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromSeconds(10);
    opt.Lockout.MaxFailedAccessAttempts = 5;
    //  confirmed account
    opt.SignIn.RequireConfirmedAccount = false;
});
//  Gmail smtp settings
builder.Services.Configure<GSMTPSettings>(builder.Configuration.GetSection("GSMTPSettings"));



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

//  role based auth
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
