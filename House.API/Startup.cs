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
            //依赖注入SqlSugar
            //services.AddSqlsugarSetup(Configuration.GetSection("ConnectionStrings").GetSection("MSSQLConnection").Value);

            //解决生成Json首字母小写问题
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
        /// 引用：
        /// 1、 Autofac
        /// 2、 Autofac.Extension.DependencyInjection
        /// 3、 Autofac.Configuration（配置文件注入时需要）
        ///
        /// Autofac是.NET领域最为流行的IOC框架之一，应该是速度最快的一个
        ///  优点：
        ///  1、它是C#语言联系很紧密，C#里的很多编程方式都可以为Autofac使用，例如可以用Lambda表达式注册组件
        ///  2、学习它非常的简单，只要理解了IoC和DI的概念就可以了使用autofac
        ///  3、支持XML配置
        ///  4、支持自动装配服务
        ///  5、微软的Orchad开源程序使用的就是Autofac
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

            //启用静态文件
            app.UseStaticFiles();

            //跨域
            app.UseCors(o => o.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());

            app.UseHttpsRedirection();//http重定向中间件

            app.UseRouting();//路由中间件

            app.UseAuthorization();//

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/Login/swagger.json", "登录管理");
                c.SwaggerEndpoint("/swagger/Power/swagger.json", "权限管理");
                c.SwaggerEndpoint("/swagger/Role/swagger.json", "角色管理");
                c.SwaggerEndpoint("/swagger/Personnel/swagger.json", "人员管理");
                c.SwaggerEndpoint("/swagger/Device/swagger.json", "设备管理");
                c.SwaggerEndpoint("/swagger/Dict/swagger.json", "字典管理");
                c.SwaggerEndpoint("/swagger/Customerinfo/swagger.json", "客户信息录入");
                c.SwaggerEndpoint("/swagger/ContractInfo/swagger.json", "合同录入");
                c.SwaggerEndpoint("/swagger/Projectinfo/swagger.json", "项目立项");
            });
        }

        public void AddSwaggerGenServic(IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("Login", new OpenApiInfo
                {
                    Version = "v1.0.0",
                    Title = "登录管理"
                });
                options.SwaggerDoc("Power", new OpenApiInfo
                {
                    Version = "v1.0.0",
                    Title = "权限管理"
                });
                options.SwaggerDoc("Role", new OpenApiInfo
                {
                    Version = "v1.0.0",
                    Title = "角色管理"
                });
                options.SwaggerDoc("Personnel", new OpenApiInfo
                {
                    Version = "v1.0.0",
                    Title = "人员管理"
                });
                options.SwaggerDoc("Device", new OpenApiInfo
                {
                    Version = "v1.0.0",
                    Title = "设备管理"
                });
                options.SwaggerDoc("Dict", new OpenApiInfo
                {
                    Version = "v1.0.0",
                    Title = "字典管理"
                });
                options.SwaggerDoc("Customerinfo", new OpenApiInfo
                {
                    Version = "v1.0.0",
                    Title = "客户信息录入"
                });
                options.SwaggerDoc("ContractInfo", new OpenApiInfo
                {
                    Version = "v1.0.0",
                    Title = "合同录入"
                });
                options.SwaggerDoc("Projectinfo", new OpenApiInfo
                {
                    Version = "v1.0.0",
                    Title = "项目立项"
                });
                //按照分组取api文档
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

                // 为 Swagger JSON and UI设置xml文档注释路径
                var basePath = Path.GetDirectoryName(typeof(Program).Assembly.Location);

                //获取应用程序所在目录（绝对路径）
                var xmlPath = Path.Combine(basePath, "House.API.xml");

                //如果需要显示控制器注释只需将第二个参数设置为true
                options.IncludeXmlComments(xmlPath, true);
            });
        }
    }
}