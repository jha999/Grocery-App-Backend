using APExittest.Business.Repository;
using APExittest.Bussiness.Data;
using APExittest.Bussiness.Model;
using APExittest.Bussiness.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APExittest
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
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
       .AddJwtBearer(options =>
       {
           options.TokenValidationParameters = new TokenValidationParameters
           {
               ValidateIssuer = true,
               ValidateAudience = true,
               ValidateIssuerSigningKey = true,
               ValidIssuer = Configuration["Jwt:ValidIssuer"],
               ValidAudience = Configuration["Jwt:ValidAudience"],
               IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Secret"]))
           };
       });

            services.AddControllers();

            services.AddDbContext<AppDbContext>(x => x.UseSqlServer(Configuration.GetConnectionString("SqlConnection")));

            services.AddAuthorization();

            //services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<AppDbContext>();

            services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();

            //services.AddDefaultIdentity<IdentityUser>().AddRoles<IdentityRole>();

            services.AddScoped<IAccountRepository, AccountRepository>();

            services.AddScoped<IProductRepository, ProductRepository>();

            services.AddScoped<IOrderRepository, OrderRepository>();

            // fetch the data from frontend 
            services.AddCors(options =>
            {
                options.AddPolicy("AllowSpecificOrigin", builder =>
                {
                    builder.WithOrigins("http://localhost:4200/").AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin();
                });
            });
        }

        public async Task CreateRoles(IServiceProvider serviceProvider)
        {
            //adding customs roles : Question 1
            var RoleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var UserManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            string[] roleNames = { "Admin"};
            IdentityResult roleResult;

            foreach (var roleName in roleNames)
            {
                var roleExist = await RoleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    //create the roles and seed them to the database: Question 2
                    roleResult = await RoleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            //Here you could create a super user who will maintain the web app
            var poweruser = new ApplicationUser
            {
                UserName = Configuration["AppSettings:UserEmail"],
                Email = Configuration["AppSettings:UserEmail"],
            };

            //Ensure you have these values in your appsettings.json file

            string userPWD = Configuration["AppSettings:UserPassword"];
            var _user = await UserManager.FindByEmailAsync(Configuration["AppSettings:UserEmail"]);

            if (_user == null)
            {
                var createPowerUser = await UserManager.CreateAsync(poweruser, userPWD);
                if (createPowerUser.Succeeded)
                {
                    //here we tie the new user to the role : Question 3
                    await UserManager.AddToRoleAsync(poweruser, "Admin");

                }
            }
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env , IServiceProvider serviceProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();

            app.UseCors("AllowSpecificOrigin");

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            CreateRoles(serviceProvider).Wait();
        }
    }
}
