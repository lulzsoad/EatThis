using EatThisAPI.Database;
using EatThisAPI.Helpers;
using EatThisAPI.Models;
using EatThisAPI.Repositories;
using EatThisAPI.Services;
using EatThisAPI.Validators;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EatThisAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var authenticationSettings = new AuthenticationSettings();
            var noReplyEmailSettings = new NoReplyEmailSettings();
            Configuration.GetSection("Authentication").Bind(authenticationSettings);
            Configuration.GetSection("NoReplyEmailConfiguration").Bind(noReplyEmailSettings);
            services.AddAuthentication(option =>
            {
                option.DefaultAuthenticateScheme = "Bearer";
                option.DefaultScheme = "Bearer";
                option.DefaultChallengeScheme = "Bearer";
            }).AddJwtBearer(cfg =>
            {
                cfg.RequireHttpsMetadata = false;
                cfg.SaveToken = true;
                cfg.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = authenticationSettings.JwtIssuer,
                    ValidAudience = authenticationSettings.JwtIssuer,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authenticationSettings.JwtKey))
                };
            });

            services.AddSingleton(authenticationSettings);
            services.AddSingleton(noReplyEmailSettings);

            //Konfiguracja CORS
            services.AddCors(o => o.AddPolicy("MyPolicy", builder =>
            {
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader();
            }));

            // Rejestracja kontekstu bazy danych
            services.AddDbContext<AppDbContext>(options => options.UseSqlServer(Configuration["ConnectionString"]));
            // Rejestracja Seedera
            services.AddScoped<Seeder>();
            services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();

            services.AddScoped<IValidator, Validator>();
            services.AddScoped<IIngredientValidator, IngredientValidator>();
            services.AddScoped<ICategoryValidator, CategoryValidator>();
            services.AddScoped<IIngredientCategoryValidator, IngredientCategoryValidator>();
            services.AddScoped<IUnitValidator, UnitValidator>();
            services.AddScoped<IUserValidator, UserValidator>();

            services.AddScoped<IIngredientRepository, IngredientRepository>();
            services.AddScoped<IIngredientService, IngredientService>();
            services.AddScoped<IIngredientCategoryRepository, IngredientCategoryRepository>();
            services.AddScoped<IIngredientCategoryService, IngredientCategoryService>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IUnitRepository, UnitRepository>();
            services.AddScoped<IUnitService, UnitService>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IUserActivatingCodeRepository, UserActivatingCodeRepository>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IPasswordResetCodeRepository, PasswordResetCodeRepository>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<IUserHelper, UserHelper>();

            services.AddAutoMapper(this.GetType().Assembly);
            services.AddControllers();
            services.AddScoped<ErrorHandlingMiddleware>();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "EatThisAPI", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, Seeder seeder)
        {
            seeder.Seed();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "EatThisAPI v1"));
            }

            app.UseCors("MyPolicy");

            app.UseMiddleware<ErrorHandlingMiddleware>();
            app.UseAuthentication();
            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
