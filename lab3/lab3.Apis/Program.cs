
using lab3.DAL.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

namespace lab3.Apis
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var connectionString = builder.Configuration.GetConnectionString("Company");
            builder.Services.AddDbContext<CompanyContext>(options =>
                options.UseSqlServer(connectionString));

            builder.Services
    .AddIdentity<Employee, IdentityRole>(options =>
    {
        options.Password.RequireUppercase = false;
        options.Password.RequireLowercase = false;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequireDigit = true;
        options.Password.RequiredLength = 5;

        options.User.RequireUniqueEmail = true;
    })
    .AddEntityFrameworkStores<CompanyContext>();


            #region authenticaton
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = "Cool";
                options.DefaultChallengeScheme = "Cool";
            })
.AddJwtBearer("Cool", options =>
{
    var secretKeyString = builder.Configuration.GetValue<string>("SecretKey");
    var secretyKeyInBytes = Encoding.ASCII.GetBytes(secretKeyString ?? string.Empty);
    var secretKey = new SymmetricSecurityKey(secretyKeyInBytes);

    options.TokenValidationParameters = new TokenValidationParameters
    {
        IssuerSigningKey = secretKey,
        ValidateIssuer = false,
        ValidateAudience = false,
    };
});

            #endregion

            #region authorization
            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("adminsonly", policy => policy
                    .RequireClaim(ClaimTypes.Role, "Admin")
                    .RequireClaim(ClaimTypes.NameIdentifier));

                options.AddPolicy("usersandadmins", policy => policy
                    .RequireClaim(ClaimTypes.Role, "User" ,"Admin")
                    .RequireClaim(ClaimTypes.NameIdentifier));
            });
            #endregion


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

            app.Run();
        }
    }
}