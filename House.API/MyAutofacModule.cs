using Autofac;
using System.Reflection;

namespace House.API
{
    public class MyAutofacModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            // 第一种方式：通过反射程序集方式集中注册服务（自动装配服务）
            Assembly service = Assembly.Load("House.Repository");
            Assembly iservice = Assembly.Load("House.IRepository");
            builder.RegisterAssemblyTypes(service, iservice)
            .Where(t => t.FullName.EndsWith("Repository") && !t.IsAbstract) //类名以Repository结尾，且类型不能是抽象的　
                .InstancePerLifetimeScope() //作用域生命周期
                .AsImplementedInterfaces()
                .PropertiesAutowired();

            ////第二种方式：Autofac 基于配置文件的集中注册服务 （1、新建Config 及autofac.json；2、添加接口或实现类均需要配置）
            //IConfigurationBuilder configurationBuilder = new ConfigurationBuilder();
            //configurationBuilder.AddJsonFile("Config/autofac.json");
            //IConfigurationRoot root = configurationBuilder.Build();
            ////开始读取配置文件中的内容
            //ConfigurationModule module = new ConfigurationModule(root);
            ////根据配置文件的内容注册服务
            //builder.RegisterModule(module);
        }
    }
}