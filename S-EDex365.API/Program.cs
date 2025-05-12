using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using S_EDex365.API.Interfaces;
using S_EDex365.API.Middlewares;
using S_EDex365.API.Services;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors();
builder.Services.AddScoped<ITokenService,TokenService>();
builder.Services.AddScoped<IAuthService,AuthService>();
builder.Services.AddScoped<ISubjectService, SubjectService>();
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<IOtpService, OtpService>();
builder.Services.AddScoped<IProblemsPost, ProblemsPost>();
builder.Services.AddScoped<IClassService, ClassService>();
builder.Services.AddScoped<ITeacherService,TeacherService>();
builder.Services.AddScoped<ISolutionPostService, SolutionPostService>();
builder.Services.AddScoped<ISubjectUpdateService, SubjectUpdateService>();
builder.Services.AddScoped<ITeacherApprovalService, TeacherApprovalService>();
builder.Services.AddScoped<IFirebaseNotificationService, FirebaseNotificationService>();
builder.Services.AddScoped<ITeacherNotificationService, TeacherNotificationService>();
builder.Services.AddScoped<IStudentDashBoard, StudentDashBoard>();
builder.Services.AddScoped<IWalletService, WalletService>();
builder.Services.AddScoped<ICommunicationService, CommunicationService>();
builder.Services.AddScoped<IPostTypeService,PostTypeService>();
builder.Services.AddScoped<IEnglishClassService,EnglishClassService>();
builder.Services.AddScoped<IClaimCommunicationService,ClaimCommunicationService>();



builder.Services.AddHttpContextAccessor();
builder.Services.AddDistributedMemoryCache();


builder.Services.AddHostedService<PeriodicTaskService>();
builder.Services.AddHostedService<PaymentPeriodicTaskService>();

//Add Session
builder.Services.AddSession(options =>
{
    //set session timeout
    options.IdleTimeout = TimeSpan.FromSeconds(180);
    options.Cookie.HttpOnly= true;
    options.Cookie.IsEssential= true;
});
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options => {
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
});


// Add FirebaseNotificationService as a singleton
builder.Services.AddSingleton<FirebaseNotificationService>();

// Add controllers
builder.Services.AddControllers();

//FirebaseApp.Create(new AppOptions()
//{
//    //Credential = GoogleCredential.FromFile("path/to/your/firebase-adminsdk.json")
//    Credential = GoogleCredential.FromFile("S-EDex365.API/wwwroot/edexnotification-firebase-adminsdk-z281l-732ec6b106.json")
//});

var env = builder.Environment;
var firebaseJsonPath = Path.Combine(env.WebRootPath, "edex-365-firebase-adminsdk-fhdtp-fbacd5b738.json");

// Initialize Firebase with the constructed path
FirebaseApp.Create(new AppOptions
{
    Credential = GoogleCredential.FromFile(firebaseJsonPath)
});



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseSwagger();
app.UseSwaggerUI();

app.UseDeveloperExceptionPage();

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseSession();

app.UseMiddleware<ErrorHandlerMiddleware>();

app.MapControllers();

app.Run();
