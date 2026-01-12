using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using PersonalPages.Repositories;
using PersonalPages.Services;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

if(builder.Environment.IsProduction())
{
    Console.WriteLine("In Production");
    builder.Configuration.SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("/home/pi/codename399/personalpages/appsettings.json");
}
else
{
    Console.WriteLine("In Development");
}

builder.Services.AddControllers();
builder.Services.AddSwaggerGen();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey =
            new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["JwtKey"]))
    };
});

builder.Services.AddScoped<IAuthRepository, AuthRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IJournalRepository, JournalRepository>();

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IJournalService, JournalService>();


// ✅ ADD CORS POLICY
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular",
       policy =>
       {
           policy
               .WithOrigins(
                   "http://localhost:4200",
                   "https://personalpages-api.codename399.com",
                   "https://personalpages.codename399.com"
               )
               .AllowAnyHeader()
               .AllowAnyMethod()
               .AllowCredentials();
       });
});

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();
// ✅ ENABLE CORS
app.UseCors("AllowAngular");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
