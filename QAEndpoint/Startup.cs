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
            //�ڷ�������ע��ӿڼ���ʵ����
            services.AddScoped<QAEndpoint.Data.IDataRepository, 
                //ָ����Ҫ����ӿڵ��ĸ�ʵ����
                QAEndpoint.Data.DataRepository>();

            //GetSection����ȡָ�����ýڵ�����
            var configuration = Configuration.GetSection("ClientInformation");
            //��ȡ��������֮��ע��һ������TOption�����ڰ���������
            services.Configure<AppSettings>(configuration);
            //��������Ϣ�󶨵�������TOption���͵�ʵ���ϣ����ý��е�ÿһ��������TOpetion���͵�ʵ����Ա����Ҫ��һһ��Ӧ��
            //�󶨳ɹ��ĳ��ϣ�����һ��TOption���͵�ʵ��
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
