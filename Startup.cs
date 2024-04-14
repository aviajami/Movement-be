using Microsoft.OpenApi.Models;
using Movement_be.AuthorizeHelpers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc;
using Movement_be.Dal;
using Movement_be.Interfaces;
using Movement_be.Services;
using Movement_be.Bl;

namespace Movement_be
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
            services.Configure<MvcNewtonsoftJsonOptions>(config =>
        config.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);

            services.AddDbContext<BackendDbContext>(opt => opt.UseSqlite("Data Source=UsersManager.db"));


            services.AddSingleton(typeof(IHttpService), typeof(HttpService));
            services.AddSingleton(typeof(IAutoMapperService), typeof(AutoMapperService));
            services.AddScoped<IUsersBl, UsersBl>();
            services.AddScoped<IUsersDal, UsersDal>();

            services.AddControllers().AddNewtonsoftJson();
            services.AddCors(o => o.AddPolicy("MyCorsPolicy", builder =>
            {
                builder.WithOrigins("http://localhost:4200", "https://www.google.com", "https://www.facebook.com", "http://localhost:4202")
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            }));

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Movement", Version = "v1" });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = @"JWT Authorization header using the Bearer scheme.",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header,

                        },
                        new List<string>()
                    }
                });
            });

            SeedDefaultData(services);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "MovementBE v1");
                });
            }

            app.UseCors("MyCorsPolicy");
            app.UseRouting();
            app.UseMiddleware<JwtMiddleware>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private async Task SeedDefaultData(IServiceCollection services)
        {
            using (var servicesContainer = services.BuildServiceProvider().CreateScope())
            {
                var dbContext = servicesContainer.ServiceProvider.GetRequiredService<BackendDbContext>();

                // Note:
                // Any data inserted to the DB is deleted on load
                // To persist data remark the following line
                dbContext.Database.EnsureDeleted();

                var notExisted = dbContext.Database.EnsureCreated();
                if (notExisted)
                    await dbContext.SeedMockData();
            }
        }
    }
}
