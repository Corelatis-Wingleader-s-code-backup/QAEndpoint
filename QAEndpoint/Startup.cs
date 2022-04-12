using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QAEndpoint {
    public class Startup {
        public readonly string anyAllowSpecificOrigins = "any";
        public Startup(IConfiguration configuration) {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services) {

            services.AddControllers();
            services.AddSwaggerGen(c => {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "QAEndpoint", Version = "v1" });
            });

            //inject DataRepository to controller
            //在服务容器注册接口及其实现类
            services.AddScoped<QAEndpoint.Data.IDataRepository, 
                //指定需要这个接口的哪个实现类
                QAEndpoint.Data.DataRepository>();

            //GetSection：获取指定配置节的内容
            var configuration = Configuration.GetSection("ClientInformation");
            //读取配置内容之后，注册一个类型TOption，用于绑定配置内容
            services.Configure<AppSettings>(configuration);
            //将配置信息绑定到给定的TOption类型的实例上（配置节中的每一项名称与TOpetion类型的实例成员名称要求一一对应）
            //绑定成功的场合，返回一个TOption类型的实例
            var settings = configuration.Get<AppSettings>();
            Console.WriteLine(settings);
            AppHelper.AppSettings = settings;

            services.AddCors(options => {
                options.AddPolicy(anyAllowSpecificOrigins, corsbuilder => {
                    var corsPath = Configuration.GetSection("CorsPaths").GetChildren().Select(p => p.Value).ToArray();
                    corsbuilder.WithOrigins(corsPath)
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials();
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "QAEndpoint v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseCors(anyAllowSpecificOrigins);
            app.UseAuthorization();

            app.UseEndpoints(endpoints => {
                endpoints.MapControllers().RequireCors(anyAllowSpecificOrigins);
            });
        }
    }
}
