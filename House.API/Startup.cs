using Autofac;
using House.Core;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.IO;
using System.Linq;
using System.Reflection;

namespace House.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //����ע��SqlSugar
            //services.AddSqlsugarSetup(Configuration.GetSection("ConnectionStrings").GetSection("MSSQLConnection").Value);

            //�������Json����ĸСд����
            services.AddControllers().AddNewtonsoftJson(o => o.SerializerSettings.ContractResolver = new Newtonsoft.Json.Serialization.DefaultContractResolver());

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "House.API", Version = "v1" });
            });
            AddSwaggerGenServic(services);
            services.AddDbContext<MyDbConText>(options =>
            {
                var connectionString = this.Configuration["ConnectionStrings:MysqlConnection"];
                options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
            });

            services.AddScoped<MyDbConText>();
        }

        /// <summary>
        /// ���ã�
        /// 1�� Autofac
        /// 2�� Autofac.Extension.DependencyInjection
        /// 3�� Autofac.Configuration�������ļ�ע��ʱ��Ҫ��
        ///
        /// Autofac��.NET������Ϊ���е�IOC���֮һ��Ӧ�����ٶ�����һ��
        ///  �ŵ㣺
        ///  1������C#������ϵ�ܽ��ܣ�C#��ĺܶ��̷�ʽ������ΪAutofacʹ�ã����������Lambda���ʽע�����
        ///  2��ѧϰ���ǳ��ļ򵥣�ֻҪ�����IoC��DI�ĸ���Ϳ�����ʹ��autofac
        ///  3��֧��XML����
        ///  4��֧���Զ�װ�����
        ///  5��΢���Orchad��Դ����ʹ�õľ���Autofac
        /// </summary>
        public void ConfigureContainer(ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterModule<MyAutofacModule>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //���þ�̬�ļ�
            app.UseStaticFiles();

            //����
            app.UseCors(o => o.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());

            app.UseHttpsRedirection();//http�ض����м��

            app.UseRouting();//·���м��

            app.UseAuthorization();//

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/Login/swagger.json", "��¼����");
                c.SwaggerEndpoint("/swagger/Power/swagger.json", "Ȩ�޹���");
                c.SwaggerEndpoint("/swagger/Role/swagger.json", "��ɫ����");
                c.SwaggerEndpoint("/swagger/Personnel/swagger.json", "��Ա����");
                c.SwaggerEndpoint("/swagger/Device/swagger.json", "�豸����");
                c.SwaggerEndpoint("/swagger/Dict/swagger.json", "�ֵ����");
                c.SwaggerEndpoint("/swagger/Customerinfo/swagger.json", "�ͻ���Ϣ¼��");
                c.SwaggerEndpoint("/swagger/ContractInfo/swagger.json", "��ͬ¼��");
                c.SwaggerEndpoint("/swagger/Projectinfo/swagger.json", "��Ŀ����");
            });
        }

        public void AddSwaggerGenServic(IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("Login", new OpenApiInfo
                {
                    Version = "v1.0.0",
                    Title = "��¼����"
                });
                options.SwaggerDoc("Power", new OpenApiInfo
                {
                    Version = "v1.0.0",
                    Title = "Ȩ�޹���"
                });
                options.SwaggerDoc("Role", new OpenApiInfo
                {
                    Version = "v1.0.0",
                    Title = "��ɫ����"
                });
                options.SwaggerDoc("Personnel", new OpenApiInfo
                {
                    Version = "v1.0.0",
                    Title = "��Ա����"
                });
                options.SwaggerDoc("Device", new OpenApiInfo
                {
                    Version = "v1.0.0",
                    Title = "�豸����"
                });
                options.SwaggerDoc("Dict", new OpenApiInfo
                {
                    Version = "v1.0.0",
                    Title = "�ֵ����"
                });
                options.SwaggerDoc("Customerinfo", new OpenApiInfo
                {
                    Version = "v1.0.0",
                    Title = "�ͻ���Ϣ¼��"
                });
                options.SwaggerDoc("ContractInfo", new OpenApiInfo
                {
                    Version = "v1.0.0",
                    Title = "��ͬ¼��"
                });
                options.SwaggerDoc("Projectinfo", new OpenApiInfo
                {
                    Version = "v1.0.0",
                    Title = "��Ŀ����"
                });
                //���շ���ȡapi�ĵ�
                options.DocInclusionPredicate((docName, apiDes) =>
                {
                    if (!apiDes.TryGetMethodInfo(out MethodInfo method))
                        return false;
                    var version = method.DeclaringType.GetCustomAttributes(true).OfType<ApiExplorerSettingsAttribute>().Select(m => m.GroupName);
                    if (docName == "v1" && !version.Any())
                        return true;
                    var actionVersion = method.GetCustomAttributes(true).OfType<ApiExplorerSettingsAttribute>().Select(m => m.GroupName);
                    if (actionVersion.Any())
                        return actionVersion.Any(v => v == docName);
                    return version.Any(v => v == docName);
                });

                // Ϊ Swagger JSON and UI����xml�ĵ�ע��·��
                var basePath = Path.GetDirectoryName(typeof(Program).Assembly.Location);

                //��ȡӦ�ó�������Ŀ¼������·����
                var xmlPath = Path.Combine(basePath, "House.API.xml");

                //�����Ҫ��ʾ������ע��ֻ�轫�ڶ�����������Ϊtrue
                options.IncludeXmlComments(xmlPath, true);
            });
        }
    }
}