using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
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

        builder.Services.AddScoped<OdtsystemContext>();

        builder.Services.AddScoped<IUserRepository, UserRepository>();
        builder.Services.AddScoped<IBaseRepository, BaseRepository>();

        builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
        builder.Services.AddScoped<IAccountService, AccountService>();

        builder.Services.AddScoped<IMailHandler, MailHandler>();
        builder.Services.AddScoped<IJWTHandler, JWTHandler>();
        builder.Services.AddScoped<IBcryptHandler, BcryptHandler>();


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

        builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = "EuthorizationBearer";
                options.DefaultChallengeScheme = "EuthorizationBearer";
            })
            .AddScheme<EuthorizationBearerSchemeOptions, EuthorizationBearerHandler>("EuthorizationBearer", null);

        // Add services to the container.
        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();

        //if (builder.Environment.IsProduction())
        //{
        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });

            c.AddSecurityDefinition("Euthorization", new OpenApiSecurityScheme
            {
                Description = "Enter 'Bearer' [space] and then your token in the text input below.",
                Name = "Euthorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer"
            });

            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Euthorization"
                }
            },
            new string[] {}
        }
            });
        });
        //}
        //else
        //{
        //builder.Services.AddSwaggerGen(
        //options =>
        //{
        //    options.SwaggerDoc("v1", new OpenApiInfo { Title = "ClassNTutor", Version = "v1" });
        //    options.AddSecurityDefinition(
        //        "Bearer",
        //        new OpenApiSecurityScheme
        //{
        //    In = ParameterLocation.Header,
        //    Description = "Insert token.",
        //    Name = "Authorization",
        //    Type = SecuritySchemeType.Http,
        //    BearerFormat = "JWT",
        //    Scheme = "Bearer"
        //}
        //        );

        //    options.AddSecurityRequirement(
        //        new OpenApiSecurityRequirement()
        //{
        //        {
        //            new OpenApiSecurityScheme
        //            {
        //                Reference = new OpenApiReference
        //                {
        //                    Type = ReferenceType.SecurityScheme,
        //                    Id = "Bearer"
        //                },
        //            },
        //            new List<string>()
        //            }
        //    }
        //        );
        //}
        //);
        //}


        var app = builder.Build();

        // This middleware should come first
        app.Use(async (context, next) =>
        {
            var euthorizationHeader = context.Request.Headers["Euthorization"].FirstOrDefault();
            if (!string.IsNullOrEmpty(euthorizationHeader))
            {
                context.Request.Headers.Remove("Euthorization");
                context.Request.Headers.Add("Authorization", euthorizationHeader);
            }
            await next();
        });

        // Configure the HTTP request pipeline.
        //if (app.Environment.IsDevelopment())
        //{
        app.UseSwagger();
        app.UseSwaggerUI();
        //}

        app.UseHttpsRedirection();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}