using Logging;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using JYD.DataContext;
using JYD.DataContext.Repository;
using JYD.Helper;
using JYD.Mailer;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Text;

namespace JYD
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(WebApplicationBuilder builder)
        {
            builder.Services.UseLoggerService();

            builder.Services.Configure<MailSmtp>(options => {
                var mailconfig = new ConfigurationBuilder()
                 .SetBasePath(Directory.GetCurrentDirectory())
                 .AddJsonFile("mailsettings.json")
                 .Build();

                options.Host = mailconfig.GetSection("MailSmtp:Host").Value;
                options.Port = Convert.ToInt32(mailconfig.GetSection("MailSmtp:Port").Value);
                options.DisplayName = mailconfig.GetSection("MailSmtp:DisplayName").Value;
                options.Username = mailconfig.GetSection("MailSmtp:Username").Value;
                options.Password = mailconfig.GetSection("MailSmtp:Password").Value;
                options.EnableSsl = Convert.ToBoolean(mailconfig.GetSection("MailSmtp:EnableSsl").Value);
            });
            builder.Services.AddSingleton<IMailSmtpClient, MailSmtpClient>();
            builder.Services.AddScoped<IMailService, MailService>();

            
            builder.Services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });
            builder.Services.AddScoped<IProcedureRepository, ProcedureRepository>();

         
            builder.Services.AddCors();

            builder.Services.AddAuthentication(AuthOptions => {
                AuthOptions.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                AuthOptions.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                AuthOptions.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(JwtOptions =>
            {
                var jwtBearer = Security.JwtBearer.Parameters.ReadFromJsonFile("paramsettings.json");
                JwtOptions.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true,
                    ValidAudience = jwtBearer.AudienceId,
                    ValidIssuer = $"{jwtBearer.Instance}{jwtBearer.AuthorityId}",
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtBearer.SecretKey)),
                    ClockSkew = TimeSpan.Zero,
                };

                JwtOptions.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = context =>
                    {
                        if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                        {
                            context.Response.Headers.Add("Token-Expired", "true");
                        }

                        return Task.CompletedTask;
                    },
                    OnChallenge = async (context) =>
                    {
                        context.HandleResponse();

                        if (context.AuthenticateFailure != null)
                        {
                            var result = new ResponseParameter { Code = HttpStatusCode.Unauthorized, Data = null, ErrorMessage = "Unauthorized, Request access denied." };                            

                            context.Response.StatusCode = 401;

                            await context.HttpContext.Response.WriteAsync(JsonConvert.SerializeObject(result, Formatting.Indented));
                        }
                    },
                    OnTokenValidated = context =>
                    {
                        var tokenHandler = new JwtSecurityTokenHandler();
                        var principal = tokenHandler.ValidateToken(
                            context.HttpContext.Request.Headers["authorization"].ToString().Replace("Bearer ", ""),
                            new TokenValidationParameters
                            {
                                ValidateIssuer = true,
                                ValidateAudience = true,
                                ValidateIssuerSigningKey = true,
                                ValidateLifetime = true,
                                ValidAudience = jwtBearer.AudienceId,
                                ValidIssuer = $"{jwtBearer.Instance}{jwtBearer.AuthorityId}",
                                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtBearer.SecretKey)),
                                ClockSkew = TimeSpan.Zero,
                            }, out SecurityToken validatedToken);

                        var jwtSecurityToken = validatedToken as JwtSecurityToken;
                        var isValid = jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase);
                        if (isValid)
                        {
                            var result = new Dictionary<string, string>() {
                                { "Failed", "Token validation has failed. Request access denied." }
                            };

                            context.Fail(JsonConvert.SerializeObject(result, Formatting.Indented));
                        }

                        return Task.CompletedTask;
                    }
                };
            });
                        
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddSwaggerGen(options => {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = Configuration["AppName"],
                    Version = "v1"
                });
                
                options.AddSecurityDefinition("Bearer,", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter a valid token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "Bearer"
                });
                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type=ReferenceType.SecurityScheme,
                                Id="Bearer"
                            }
                        },
                        new string[]{}
                    }
                });
            });

           
        }

        public void Configure(WebApplication app)
        {
            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint("/swagger/v1/swagger.json", Configuration["AppName"] + " v1");
                });                
            }

            app.UseCors(corsPolicyBuilder => corsPolicyBuilder
                .AllowAnyHeader()
                .AllowAnyMethod()
                .SetIsOriginAllowed((host) => true)
                .AllowCredentials()
            );

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoint =>
            {
                endpoint.MapControllers();
            });

            app.Run();
        }
    }
}
