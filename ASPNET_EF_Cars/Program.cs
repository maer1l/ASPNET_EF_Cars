
using ASPNET_EF_Cars.Data;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using System.Security.Principal;
using static System.Net.WebRequestMethods;

namespace ASPNET_EF_Cars
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            string connection = builder.Configuration.GetConnectionString("DefaultConnection");
            builder.Services.AddDbContext<AspcarsContext>(options => options.UseSqlServer(connection));

            //create table cars(car_id int identity, brand varchar(25), model varchar(25), speed float, price money, year date, category_id int)
            //create table categories(category_id int identity, title varchar(25), description varchar(MAX), seats int)

            var app = builder.Build();

            // DESKTOP-3OSCRNB\SQLEXPRESS
            // Scaffold-DbContext "Server=DESKTOP-3OSCRNB\SQLEXPRESS;Database=aspcars;Trusted_Connection=True;TrustServerCertificate=True" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Models -ContextDir Data -Tables cars -DataAnnotations
            // Scaffold-DbContext "Server=DESKTOP-3OSCRNB\SQLEXPRESS;Database=aspcars;Trusted_Connection=True;TrustServerCertificate=True" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Models -ContextDir Data -Tables categories -DataAnnotations -Force

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthorization();

            app.MapStaticAssets();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}")
                .WithStaticAssets();

            app.Run();
        }
    }
}
