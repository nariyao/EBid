using Microsoft.EntityFrameworkCore;
using EBid.Models;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Identity;

namespace EBid
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddDbContext<EBidDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("SQL_Server")));
            builder.Services.Configure<FormOptions>(options =>
            {
                options.MultipartBodyLengthLimit = 100L * 1024L * 1024L;
            });
            builder.Services.AddIdentity<IdentityUser, IdentityRole>()
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<EBidDbContext>();
            var app = builder.Build();


            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseStatusCodePages();
                app.UseHttpLogging();
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();


            using (var scope = app.Services.CreateScope())
            {
                var RoleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                var Roles = new string[] { "Admin", "User" };
                foreach (var roleName in Roles)
                {
                    if(!await RoleManager.RoleExistsAsync(roleName))
                        await RoleManager.CreateAsync(new IdentityRole(roleName)); //(RoleManager.)
                }
            }

            using (var scope = app.Services.CreateScope())
            {
                var userName = "Admin";
                var email = "admin@ebid.com";
                var password = "Admin@123";
                var UserManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
                if (await UserManager.FindByEmailAsync(email) == null)
                {
                    var user = new IdentityUser {
                        UserName = userName,
                        Email = email,
                        EmailConfirmed = true,
                    };
                    await UserManager.CreateAsync(user, password);
                    await UserManager.AddToRoleAsync(user, "Admin");

                }
            }

            app.Run();
        }
    }
}
