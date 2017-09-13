using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ProjetoRedehost.Data;
using ProjetoRedehost.Models;
using ProjetoRedehost.Services;
using ProjetoRedehost.Services.tld;
using ProjetoRedehost.Services.tld.cache;
using ProjetoRedehost.Services.whois;

namespace ProjetoRedehost
{
    public class Startup
    { 
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder() 
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

            if (env.IsDevelopment())
            {
                // For more details on using the user secret store see https://go.microsoft.com/fwlink/?LinkID=532709
                builder.AddUserSecrets<Startup>();
            }

            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            
            services.AddSingleton<IConfiguration>(Configuration);
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlite(Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.AddMvc();

            // Add application services.
            services.AddTransient<IEmailSender, AuthMessageSender>();
            services.AddTransient<ISmsSender, AuthMessageSender>();
            services.AddSingleton<ITldCache,TldCacheService>(sp =>
            {
                return new TldCacheService(
                    Configuration.GetConnectionString("RedisConnection"),
                    Configuration.GetSection("TldKey").Value
                );
            });
            services.AddSingleton<IWhois, WhoisService>(ws =>
            { 
                return new WhoisService(new WhoisStruct()
                {
                    URI = Configuration["Whois:URI"],
                    Param = Configuration["Whois:Param"]
                });
            });
            services.AddTransient<ITld, TldServices>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/error");
            }

            app.UseIdentity();
            app.UseFacebookAuthentication(new FacebookOptions
            {
                AppId = Configuration["Authentication:Facebook:AppId"],
                AppSecret = Configuration["Authentication:Facebook:AppSecret"],
                Scope = { "email" },
                Fields = { "name", "email" },
                SaveTokens = true, 
            });

            app.UseGitHubAuthentication(options =>
            {
                options.ClientId = Configuration["Authentication:GitHub:ClientId"];
                options.ClientSecret = Configuration["Authentication:GitHub:SecretId"];
            });

            app.Use(async (context, next) => {
                await next();
                if (context.Response.StatusCode == 404 &&
                    !Path.HasExtension(context.Request.Path.Value) &&
                    !context.Request.Path.Value.StartsWith("/api/")) 
                {
                    context.Request.Path = "/home";
                    await next();
                }
            });
            app.UseMvcWithDefaultRoute();
            app.UseDefaultFiles();
            app.UseStaticFiles();

        }
    }
}
