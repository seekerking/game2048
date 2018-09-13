using System;
using System.Text;
using Game2048Api.Common;
using Game2048EF;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Models.Common;

namespace Game2048Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {

            Configuration = configuration;
        }

        public IConfiguration Configuration;
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {




            var tokenValidationParameters = new TokenValidationParameters
            {
                // The signing key must match!
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Configuration["JwtSecretKey"])),

                // Validate the JWT Issuer (iss) claim
                ValidateIssuer = false,
                //ValidIssuer = "Game2048",

                // Validate the JWT Audience (aud) claim
                ValidateAudience = false,
                //ValidAudience = "WNL",

                // Validate the token expiry
                ValidateLifetime = false,

                // If you want to allow a certain amount of clock drift, set that here:
                ClockSkew = TimeSpan.Zero
            };


            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>

                {
                    options.TokenValidationParameters = tokenValidationParameters;
                    options.SaveToken = true;
                    options.RequireHttpsMetadata = false;
                }
            );

            //我们使用延迟加载并不鼓励全部都这么写，如果是汇总类的，要考虑join还是进行计算得到最优的速度,如果不打算使用则使用include，
            //其实类似于join后给对应的virtual进行的一个赋值操作,是平铺开属性还是经属性归为一个对象属性里面
            services.AddDbContext<GameDbContext>(options => options.UseLazyLoadingProxies(false).UseSqlServer(Configuration["Connection"], x => x.UseRowNumberForPaging()));
            services.AddScoped(typeof(ConfigHelper), typeof(ConfigHelper));


            services.MappingRepository();
            services.MappingService();
            services.AddMvc().AddJsonOptions(options =>
            {
                //设置序列化默认时间格式以及首字母小写
                options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
                options.SerializerSettings.ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver();
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {

            loggerFactory.AddLog4Net();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseAuthentication();
            app.UseCors(builder => builder
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials());
            app.UseMvc();
        }
    }
}
