using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Firewall;
using JwtAuthDemo.Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using WallPaperApp.Controllers;
using WallPaperApp.Infrastructure;
using WallPaperApp.Mapper;
using WallPaperApp.Repository.Account;
using WallPaperApp.Repository.Comment;
using WallPaperApp.Repository.Like;
using WallPaperApp.Repository.Post;
using WallPaperApp.Utility;
using WallPaperApp.Utility.Extensions;

namespace WallPaperApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        private IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            InitJwt(services);
            InitSwagger(services);

            var mapperConfig = new MapperConfiguration(mc => { mc.AddProfile(new MappingProfile()); });

            var mapper = mapperConfig.CreateMapper();
            services.AddSingleton(mapper);


            /*
            services.AddControllers().AddNewtonsoftJson(options =>
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            );
            */


            services.AddControllers();
            services.AddMongoDbSettings(Configuration);
            services.AddSingleton<IJwtAuthManager, JwtAuthManager>();
            services.AddHostedService<JwtRefreshTokenCache>();
            services.AddScoped<Security>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<IAccountRepository, AccountRepository>();
            services.AddSingleton<IPostRepository, PostRepository>();
            services.AddSingleton<ILikeRepository, LikeRepository>();
            services.AddSingleton<ICommentRepository, CommentRepository>();
        }


        private static void InitSwagger(IServiceCollection services)
        {
            services.AddSwaggerDocument(config =>
            {
                config.PostProcess = document =>
                {
                    document.Info.Version = "v1";
                    document.Info.Title = "Wallpaper API";
                    document.Info.Description = "";
                    document.Info.Contact = new NSwag.OpenApiContact
                    {
                        Name = "Kemal TUNÇ",
                        Email = "kemaltunc191@gmail.com",
                        Url = "https://github.com/kemaltunc"
                    };
                };
            });

            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll",
                    builder => { builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader(); });
            });
        }

        private void InitJwt(IServiceCollection services)
        {
            var jwtTokenConfig = Configuration.GetSection("jwtTokenConfig").Get<JwtTokenConfig>();
            services.AddSingleton(jwtTokenConfig);
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = true;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = jwtTokenConfig.Issuer,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtTokenConfig.Secret)),
                    ValidAudience = jwtTokenConfig.Audience,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.FromMinutes(1)
                };
            });
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
                app.UseHsts();
            }

            /*   var allowedIPs =
                  new List<IPAddress> {
                      IPAddress.Parse ("127.0.0.1"),
                      IPAddress.Parse ("192.168.1.21")
                  };

              app.UseFirewall (
                  FirewallRulesEngine
                  .DenyAllAccess ()
                  .ExceptFromIPAddresses (allowedIPs)
              ); */

            app.UseRouting();

            app.UseCors("AllowAll");
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseStaticFiles();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

            app.UseOpenApi();
            app.UseSwaggerUi3();

            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
                    Path.Combine(env.ContentRootPath, "wwwroot")),
                RequestPath = StaticFiles.path
            });

            app.Run(async (context) => { await context.Response.WriteAsync("Could Not Find Anything"); });
        }
    }
}