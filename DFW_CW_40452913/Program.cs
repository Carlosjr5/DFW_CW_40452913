using DFW_CW_40452913.Controllers;
using DFW_CW_40452913.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.AspNetCore.Http.Features;
using DFW_CW_40452913.Middleware;
using DFW_CW_40452913.Middleware.DFW_CW_40452913.Middleware;

public class Program
{
  
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
        builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(connectionString));
        builder.Services.AddDatabaseDeveloperPageExceptionFilter();

        builder.Services.AddIdentity<IdentityUser, IdentityRole>()
            .AddDefaultUI()
            .AddDefaultTokenProviders()
            .AddRoles<IdentityRole>() // Adding identity role for admin and user.

            .AddEntityFrameworkStores<ApplicationDbContext>();

        builder.Services.AddSession(options =>
        {
            options.IdleTimeout = TimeSpan.FromMinutes(30); // Session timeout
            options.Cookie.HttpOnly = true;
            options.Cookie.IsEssential = true;
        });

        builder.Services.AddControllersWithViews();


        builder.Services.Configure<KestrelServerOptions>(options =>
        {
            options.Limits.KeepAliveTimeout = TimeSpan.FromMinutes(10);
            options.Limits.RequestHeadersTimeout = TimeSpan.FromMinutes(10);
        });

        builder.Services.Configure<FormOptions>(x =>
        {
            x.ValueLengthLimit = int.MaxValue;
            x.MultipartBodyLengthLimit = int.MaxValue; // Adjust according to your needs
        });

   


        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseMigrationsEndPoint();
        }
        else
        {
            app.UseExceptionHandler("/Home/Error");
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthentication();

        app.UseSession();

        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
        });

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
            endpoints.MapControllerRoute(
                name: "admin",
                pattern: "{controller=Admin}/{action=Index}/{id?}");
        });

        app.MapRazorPages();

        using (var scope = app.Services.CreateScope())
        {
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            var roles = new[] { "admin", "user" };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }
        }

        //Creating new temporary user.
        using (var scope = app.Services.CreateScope())
        {
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

            string email = "carlos23@gmail.com";
            string password = "Admin1234!";

            if (await userManager.FindByEmailAsync(email) == null)
            {
                var user = new IdentityUser
                {
                    UserName = email,
                    Email = email
                };

                var result = await userManager.CreateAsync(user, password);

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, "admin");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        Console.WriteLine(error.Description);
                    }
                }
            }
        }

        //Deletion Proccess
        app.UseDeletePetitionMiddleware();



        app.Run();

    }



}
