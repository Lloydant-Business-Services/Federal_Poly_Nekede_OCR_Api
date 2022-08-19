using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using APIs.Middleware;
using BusinessLayer.Interface;
using BusinessLayer.Services;
using BusinessLayer.Services.Email;
using BusinessLayer.Services.Email.Interface;
using DataLayer.Model;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

namespace APIs
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<FPNOOCRContext>(opt => opt.UseSqlServer(Configuration.GetConnectionString("FPNO_ARCHIVE"),
                                                    sqlServerOptionsAction: sqlOptions => {
                                                        sqlOptions.MigrationsAssembly("APIs");                                                      
                                                    })
                                                   , ServiceLifetime.Transient
         );
            services.AddAutoMapper(typeof(Startup));
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IFacultyService, FacultySchoolService>();
            services.AddScoped<IDepartmentService, DepartmentService>();
            services.AddScoped<IDepartmentOptionService, DepartmentOptionService>();
            services.AddScoped<ICourseService, CourseService>();
            services.AddScoped<ISessionSemesterService, SessionSemesterService>();
            services.AddScoped<IReportingSevice, ReportingService>();
            services.AddScoped<IAuditService, AuditService>();
            services.AddScoped<IEmailServiceAlt, EmailServiceAlt>();
            services.AddScoped<IResultVettingService, ResultVettingService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IStudentResultService, StudentResultService>();
            services.AddScoped<IStudentCarryOverService, StudentCarryOverService>();
            services.AddScoped<IProgrammeService, ProgrammeService>();
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

            services
             .AddFluentEmail("support@fpnoocr.ng")
             .AddMailGunSender(
                 Configuration.GetValue<string>("MailGun:domain"),
                 Configuration.GetValue<string>("MailGun:apiKey"),
                 FluentEmail.Mailgun.MailGunRegion.EU
                 )
         .AddRazorRenderer();

            services.AddSwaggerGen(c =>
            {
                
                c.SwaggerDoc("v1", new OpenApiInfo { 
                    Title = "FPNO OCR/Result Archiving API's", 
                    Version = "v2.1",
                    Contact = new OpenApiContact
                    {
                        Name = "Godspeed Miracle",
                        Email = "miracleoghenemado@gmail.com"
                    }
                });

                var securityScheme = new OpenApiSecurityScheme
                {
                    Name = "JWT Authentication",
                    Description = "Enter JWT Bearer token **_only_**",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer", // must be lower case
                    BearerFormat = "JWT",
                    Reference = new OpenApiReference
                    {
                        Id = JwtBearerDefaults.AuthenticationScheme,
                        Type = ReferenceType.SecurityScheme
                    }
                };

                c.AddSecurityDefinition(securityScheme.Reference.Id, securityScheme);
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {securityScheme, new string[] { }}
                });

                c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());


            });
           


            services.AddControllers();

            services.AddRouting();
            services.AddHttpClient();
            services.AddAuthentication();

            services.AddHealthChecks();



        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            UpdateDatabase(app);

            //to serve static files

            app.UseStaticFiles();
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
                    Path.Combine(Directory.GetCurrentDirectory(), @"Resources")),
                RequestPath = "/Resources"

            });
            app.UseMiddleware<JWTMiddleware>();
            if (!env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseCors(
             options => options.SetIsOriginAllowed(x => _ = true)
             .AllowAnyMethod()
             .AllowAnyHeader()
             .AllowCredentials()
         //.WithOrigins(MyAllowSpecificOrigins)
         );
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("../swagger/v1/swagger.json", "FPNO OCR");
                //c.RoutePrefix = string.Empty;
            });
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private static void UpdateDatabase(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices
                .GetRequiredService<IServiceScopeFactory>()
                .CreateScope())
            {
                using (var context = serviceScope.ServiceProvider.GetService<FPNOOCRContext>())
                {
                    context.Database.Migrate();
                }
            }
        }
    }
}
