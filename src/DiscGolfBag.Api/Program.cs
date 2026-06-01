using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using DiscGolfBag.Api.Common.Data;
using DiscGolfBag.Api.Common.Models;
using DiscGolfBag.Api.Features.Auth;
using DiscGolfBag.Api.Features.Discs;
using DiscGolfBag.Api.Features.Friends;
using DiscGolfBag.Api.Features.Profiles;



var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"))); 


builder.Services.AddIdentity<AppUser, IdentityRole>(options =>
{
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 6;
})
.AddEntityFrameworkStores<AppDbContext>()
.AddDefaultTokenProviders();

var jwtkey = builder.Configuration["Jwt:Key"] ?? "default_secret_key_should_be_long_and_secure";
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtkey)),
        
    };
});

builder.Services.AddAuthorization();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins("http://localhost:5173")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});


builder.Services.AddOpenApi();

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseCors();
app.UseAuthentication();
app.UseAuthorization();
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new Microsoft.Extensions.FileProviders.PhysicalFileProvider(
        Path.Combine(builder.Environment.ContentRootPath, "wwwroot")),
    RequestPath = ""
});
app.MapMeEndpoint();
app.MapRegisterEndpoint();
app.MapLoginEndpoint();
app.MapCreateDiscEndpoint();
app.MapGetDiscsEndpoint();
app.MapDeleteDiscEndpoint();
app.MapSendRequestEndpoint();
app.MapAcceptRequestEndpoint();
app.MapDeclineRequestEndpoint();
app.MapGetFriendsEndpoint();
app.MapGetRequestsEndpoint();
app.MapGetProfileEndpoint();

app.Run();
