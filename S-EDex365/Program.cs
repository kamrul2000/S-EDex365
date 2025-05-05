using Microsoft.AspNetCore.Authentication.Cookies;
using S_EDex365.Authorization;
using S_EDex365.Data.Interfaces;
using S_EDex365.Data.Services;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddHttpClient();
builder.Services.AddScoped<IUserTypeService,UserTypeService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<IOtpService,OtpService>();
builder.Services.AddScoped<ITeacherApprovalService, TeacherApprovalService>();
builder.Services.AddScoped<IUserVMService, UserVMService>();
builder.Services.AddScoped<IAdminDashboardService, AdminDashboardService>();
builder.Services.AddScoped<IStudentDashboardService, StudentDashboardService>();
builder.Services.AddScoped<IContactMeService, ContactMeService>();
builder.Services.AddScoped<IPostTypeService, PostTypeService>();
builder.Services.AddScoped<IPostTypeDetailsService, PostTypeDetailsService>();
builder.Services.AddScoped<IBanglaClassService, BanglaClassService>();



builder.Services.Configure<CookiePolicyOptions>(options =>
    {
        // This lambda determines whether user consent for non-essential cookies is needed for a given request.  
        options.CheckConsentNeeded = context => true;
        options.MinimumSameSitePolicy = SameSiteMode.None;
    });


builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie();
builder.Services.AddPermissionAuthorization();
var app = builder.Build();




// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.UseAuthentication(); // Add authentication middleware
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

//app.MapControllerRoute(
//		name: "default",
//		pattern: "{area=Admin}/{controller=Auth}/{action=Index}/{id?}");



app.Run();
