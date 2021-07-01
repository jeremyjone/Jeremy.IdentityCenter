using IdentityServer4.Extensions;
using Jeremy.IdentityCenter.Business.Common.CollectionServices.Email;
using Jeremy.IdentityCenter.Business.Common.CollectionServices.Email.QQ;
using Jeremy.IdentityCenter.Business.Common.Extensions;
using Jeremy.IdentityCenter.Business.Handlers;
using Jeremy.IdentityCenter.Database.Entities;
using Jeremy.IdentityCenter.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Net;

namespace Jeremy.IdentityCenter
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IHostEnvironment environment)
        {
            Configuration = configuration;
            Environment = environment;
        }

        public IConfiguration Configuration { get; }
        public IHostEnvironment Environment { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            services.AddSameSiteCookiePolicy();
            services.AddSession(options =>
            {
                //配置session的有效时间,单位秒
                options.IdleTimeout = TimeSpan.FromSeconds(30);
            });


            // 配置跨域
            var origins = new List<string>();
            Configuration.GetSection("Origins").Bind(origins);
            services.AddIdcCors(new IdcCorsConfiguration
            {
                CorsAllowOrigins = origins.ToArray()
            });

            // 配置数据库
            services.AddIdcDatabase(Configuration);

            // 配置身份服务
            services.AddIdcIdentity<IdcIdentityUser, IdcIdentityRole, int>(Configuration, Environment.IsDevelopment());

            // 配置认证
            services.AddIdcAuthentication(Configuration);

            // 配置授权策略
            services.AddIdcAuthorization();

            // 邮箱服务
            services.AddEmailSender<IdcEmailSender>()
                .AddQQEmail(options =>
                {
                    options.Address = Configuration["Email:Address"];
                    options.Credentials = new NetworkCredential(Configuration["Email:Username"],
                        Configuration["Email:Password"]);
                });


            // 添加服务
            services.AddIdcServices();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // 配置 IdentityServer
                app.Use(async (context, next) =>
                {
                    context.SetIdentityServerOrigin("");
                    context.SetIdentityServerBasePath(Configuration["Url:Home"]);
                    await next();
                });

                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseCookiePolicy();
            app.UseSession();

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseIdentityServer();
            app.UseCors();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}
