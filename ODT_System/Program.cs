using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ODT_System.Mapper;
using ODT_System.Models;
using ODT_System.Repository;
using ODT_System.Repository.Interface;
using ODT_System.Services;
using ODT_System.Services.Interface;
using ODT_System.SharedObject;
using ODT_System.Utils;
using ODT_System.Utils.Interface;
using System.Text;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        //Add mapper service
        builder.Services.AddAutoMapper(typeof(DTOToModel).Assembly);
        builder.Services.AddAutoMapper(typeof(ModelToDTO).Assembly);

        // Add DbContext configuration
        builder.Services.AddDbContext<OdtsystemContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("connectionDeploy")));

        // Add DbContext services
        builder.Services.AddScoped<OdtsystemContext>();

        // Add services
        builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
        builder.Services.AddScoped<IUserRepository, UserRepository>();
        builder.Services.AddScoped<IJWTHandler, JWTHandler>();
        builder.Services.AddScoped<IBcryptHandler, BcryptHandler>();
        builder.Services.AddScoped<IBaseRepository, BaseRepository>();
        builder.Services.AddScoped<IAccountService, AccountService>();
        builder.Services.AddScoped<IMailHandler, MailHandler>();

        #region Add JWT Authentication
        //get secret key from appsettings
        var secretKey = builder.Configuration.GetSection("AppSettings:Secret").Value;
        var secretKeyBytes = Encoding.UTF8.GetBytes(secretKey);
        builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));
        //add authentication
        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,

                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,

                    IssuerSigningKey = new SymmetricSecurityKey(secretKeyBytes),

                    ClockSkew = TimeSpan.Zero
                };
            });
        #endregion

        // Add services to the container.
        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        //if (app.Environment.IsDevelopment())
        //{
        app.UseSwagger();
        app.UseSwaggerUI();
        //}

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}