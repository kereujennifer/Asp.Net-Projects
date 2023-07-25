using HospitalManagement.Controllers;
using HospitalManagement.Data;
using HospitalManagement.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Configuration;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddControllersWithViews();
        builder.Services.AddDbContextPool<ApplicationDBContext>(
                        options => options.UseSqlServer(builder.Configuration.GetConnectionString("HospitalDBConnect")));

        builder.Services.AddDefaultIdentity<IdentityUser>().AddDefaultTokenProviders().AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDBContext>();
        builder.Services.AddScoped<PatientsController>();
        builder.Services.AddScoped<IRepository, Repository>();
        //builder.Services.AddTransient<IEmailSender, EmailSender>();
        

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
        }
        app.UseStaticFiles();

        app.UseRouting();
        app.UseAuthentication();

        app.UseAuthorization();
        app.MapRazorPages();

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllerRoute(
              name: "areas",
              pattern: "{area:exists}/{controller=Home}/{action=Index=di}/{id?}"
            );


        });

        app.Run();
    }
}