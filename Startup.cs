using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ReportRegister.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using ReportRegister.Areas.Identity;

namespace ReportRegister
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

            services.AddDbContext<AppDbContext>(options =>
                    options.UseSqlServer(
                        Configuration.GetConnectionString("DefaultConnection")

                    ));


            services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedEmail = true)
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<AppDbContext>();

            services.AddTransient<IEmailSender, EmailSender>();
            services.AddHttpContextAccessor();
            services.AddControllersWithViews();
            services.AddRazorPages();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider srv)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();
            

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
            Task.Run(()=>CreateRoles(srv)).Wait();

        }

        private async Task CreateRoles(IServiceProvider serviceProvider)
        {
            //initializing custom roles 
            var RoleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var UserManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            string[] roleNames = { PredefinedRoles.Employee, PredefinedRoles.User};
            IdentityResult roleResult;

            foreach (var roleName in roleNames)
            {
                var roleExist = await RoleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    roleResult = await RoleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            var employee1 = new ApplicationUser
            {
                UserName = Configuration["employee1_email"],
                Email = Configuration["employee1_email"],
                EmailConfirmed = true//needed to login
            };
            var employee2 = new ApplicationUser
            {
                UserName = Configuration["employee2_email"],
                Email = Configuration["employee2_email"],
                EmailConfirmed = true
            };
            string employee1_password = Configuration["employee1_password"];
            string employee2_password = Configuration["employee2_password"];
            var em_1_user = await UserManager.FindByEmailAsync(Configuration["employee1_email"]);
            var em_2_user = await UserManager.FindByEmailAsync(Configuration["employee2_email"]);

            if (em_1_user == null)
            {
                var createEmployeeUser = await UserManager.CreateAsync(employee1, employee1_password);
                if (createEmployeeUser.Succeeded)
                {
                    await UserManager.AddToRoleAsync(employee1, PredefinedRoles.Employee);
                }
            }
            if (em_2_user == null)
            {
                var createEmployeeUser = await UserManager.CreateAsync(employee2, employee2_password);
                if (createEmployeeUser.Succeeded)
                {
                    await UserManager.AddToRoleAsync(employee2, PredefinedRoles.Employee);
                }
            }
        }
    }
}
