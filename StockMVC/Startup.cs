using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using StockMVC.Models;
using StockMVC.Data;
using StockMVC.Interfaces;
using StockMVC.Repositories;
using Microsoft.AspNetCore.Identity;

namespace StockMVC
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
            services.AddControllersWithViews();
            services.AddScoped<IUnit, UnitRepository>();
            services.AddScoped<ICategory, CategoryRepository>();
            services.AddScoped<IBrand, BrandRepository>();
            services.AddScoped<IProductProfile, ProductProfileRepository>();
            services.AddScoped<IProductGroup, ProductGroupRepository>();
            services.AddScoped<IProductAttribute, ProductAttributeRepository>();
            services.AddDbContext<StockContext>(options => options.UseSqlServer(Configuration.GetConnectionString("dbconn")));
        }
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
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
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
