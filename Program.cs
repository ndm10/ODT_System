using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using ODT_System.Hubs;
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

        // Add mapper service
        builder.Services.AddAutoMapper(typeof(DTOToModel).Assembly);
        builder.Services.AddAutoMapper(typeof(ModelToDTO).Assembly);

        // Add DbContext configuration
        builder.Services.AddDbContext<OdtsystemContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("connectionDeploy")));

        builder.Services.AddScoped<OdtsystemContext>();

        builder.Services.AddScoped<IUserRepository, UserRepository>();
        builder.Services.AddScoped<IPostRepository, PostRepository>();
        builder.Services.AddScoped<IStudyTimeRepository, StudyTimeRepository>();
        builder.Services.AddScoped<IChatRepository, ChatRepository>();
        builder.Services.AddScoped<IFeedbackRepository, FeedbackRepository>();

        builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
        builder.Services.AddScoped<IAccountService, AccountService>();
        builder.Services.AddScoped<ICommonService, CommonService>();
        builder.Services.AddScoped<IAdminService, AdminService>();
        builder.Services.AddScoped<IFeedbackService, FeedbackService>();

        builder.Services.AddScoped<IMailHandler, MailHandler>();
        builder.Services.AddScoped<IJWTHandler, JWTHandler>();
        builder.Services.AddScoped<IBcryptHandler, BcryptHandler>();

        builder.Services.AddSignalR();

        // Configure API behavior
        //builder.Services.AddControllers().ConfigureApiBehaviorOptions(options =>
        //{
        //    options.SuppressModelStateInvalidFilter = true;
        //});

        // Add JWT Authentication

        var secretKey = builder.Configuration.GetSection("AppSettings:Secret").Value;
        var secretKeyBytes = Encoding.UTF8.GetBytes(secretKey);
        builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));
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

        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });

            // Define the BearerAuth scheme
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = "bearer"
            });

            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    new string[] {}
                }
            });
        });

        // Add services to the container.
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("SignalRCorsPolicy", policy =>
            {
                policy.WithOrigins("http://localhost:3000")
                      .AllowAnyHeader()
                      .AllowAnyMethod()
                      .AllowCredentials();
            });
        });

        builder.Services.AddControllers();

        builder.Services.AddMemoryCache();
        builder.Services.AddEndpointsApiExplorer();

        var app = builder.Build();

        app.UseCors("SignalRCorsPolicy");

        app.UseSwagger();
        app.UseSwaggerUI();

        app.MapHub<ChatHub>("/chatHub");

        app.UseHttpsRedirection();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}
