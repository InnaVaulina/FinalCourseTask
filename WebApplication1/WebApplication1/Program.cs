using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using WebApplication1.Context;
using WebApplication1.Context.DBQuery;
using WebApplication1.Models;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddDbContext<ServiceContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));



builder.Services.AddSingleton<IRequestQuery, RequestQuery>();
builder.Services.AddSingleton<IBlogQuery, BlogQuery>();
builder.Services.AddSingleton<IProgectQuery, ProgectQuery>();
builder.Services.AddSingleton<IServiceQuery, ServiceQuery>();
builder.Services.AddSingleton<IContactQuery, ContactQuery>();
builder.Services.AddSingleton<IHeaderQuery, HeaderQuery>();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddIdentity<AppUser, IdentityRole>()
    .AddEntityFrameworkStores<ServiceContext>()
    .AddDefaultTokenProviders();


byte[] signingKey = Encoding.UTF8.GetBytes(builder.Configuration.GetValue<string>("Jwt:Key"));
builder.Services.AddAuthentication(authOptions =>
{
    authOptions.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    authOptions.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(jwtOptions =>
{
    jwtOptions.SaveToken = true;
    jwtOptions.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateAudience = false,
        ValidateIssuer = false,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(signingKey),
        ValidateLifetime = true,
        RequireExpirationTime = true
    };
});


builder.Services.Configure<IdentityOptions>(
    options =>
    {
        options.Password.RequireDigit = true;
        options.Password.RequiredLength = 6;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequireUppercase = false;
        options.Password.RequireLowercase = true;

        options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
        options.Lockout.MaxFailedAccessAttempts = 10;
        options.Lockout.AllowedForNewUsers = true;

        options.User.RequireUniqueEmail = false;

    });



var provider = builder.Services.BuildServiceProvider();
await provider.InitializeUserAsync();
provider.GetRequiredService<ServiceContext>()
    .InitializeReqests();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}





app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(Directory.GetCurrentDirectory(), "IMG")),
    RequestPath = "/IMG"
});

app.Run();
