using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PDFGenerator.Data;
using PDFGenerator.Models.AccountModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PDFGenerator.Services;
using PDFGenerator.Models.ClientModels;
using PDFGenerator.Models;

namespace PDFGenerator
{
    public class Startup
    {

        public Startup(IConfiguration configuration) => Configuration = configuration;
        public IConfiguration Configuration { get; }
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            services.AddSession();
            services.AddAuthentication();
            services.AddControllers();
            services.AddDbContext<AppIdentityDbContext>(options =>
                options.UseSqlServer(Configuration["Data:ConnectionStrings:PDF_Users"]
            ));
            services.AddDbContext<ApplicationDbContext>(o =>
                o.UseSqlServer(Configuration["Data:ConnectionStrings:PDF_Base"]
            ));
            services.AddIdentity<AppUser, IdentityRole>(options =>
            {
                options.Password.RequireNonAlphanumeric = false;
                options.User.RequireUniqueEmail = true;
            })
                .AddEntityFrameworkStores<AppIdentityDbContext>()
                .AddDefaultTokenProviders();

            services.Configure<EmailSettings>(Configuration.GetSection("EmailSettings"));
            services.AddSingleton<IEmailSender, EmailSender>();
            services.AddTransient<IFixRepository, EFFixRepository>();
            services.AddTransient<IClientRepository, EFClientRepository>();
            services.AddTransient<IClientFirmRelationRepository, EFClientFirmRelationRepository>();
            services.AddTransient<IFirmRepository, EFFirmRepository>();
            services.AddTransient<IAccesoryRepository, EFAccesoryRepository>();
        }

       
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, UserManager<AppUser> usr)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}"
                );
            });
            SeedPpl.EnsurePopulated(app, usr);
        }
    }
}
