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
        //����IConfiguration�������õĶ�ȡ
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // ��������Ŀ�е�������ע�ᵽ������ �ɿ���Դ������� ͳһ�������е���������
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
        // app.Use app.Run app.UseXXXXX ʹ���м��������HTTP�������
        // HTTP������� => ������յ�һ��HTTP����֮�� ʹ��һϵ�����ͨ���ܶ������ ȥ�����������д���� �������
        // .NET 5�������ᰴ�� Configure���� �����õ�˳�� ���ε����м�� ��HTTP������д���
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "QAEndpoint v1"));
            }
            //����HTTPS�ض��򣺰�һ����ͨ��HTTP���� �ض���HTTPS����ȥ
            app.UseHttpsRedirection();
          
            //����·�ɽ���
            app.UseRouting();
            app.UseCors(anyAllowSpecificOrigins);
            app.UseAuthorization();

            // next:RequestDelegate => ��ʾһ�����ڴ���HTTP����ĺ���
            // context:HttpContext => Http����������
            app.Use(next => async context => {
                Console.WriteLine($"method:{context.Request.Method}");
                Console.WriteLine($"Host:{context.Request.Host}");
                Console.WriteLine($"Body:{context.Request.Body}");
                Console.WriteLine($"ContentType:{context.Request.ContentType}");
                Console.WriteLine($"ContentLength: {context.Request.ContentLength}");
                Console.WriteLine($"Path:{context.Request.Path}");
                await next.Invoke(context);
            });
            //����MVC�ս������
            app.UseEndpoints(endpoints => {
                endpoints.MapControllers().RequireCors(anyAllowSpecificOrigins);
            });
        }
    }
}
