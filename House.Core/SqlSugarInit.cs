using Microsoft.Extensions.DependencyInjection;
using SqlSugar;
using System;
using System.Linq;

namespace House.Core
{
    /// <summary>
    /// SqlSugar 依赖注入
    /// </summary>
    public static class SqlSugarInit
    {
        public static void AddSqlsugarSetup(this IServiceCollection services, string sqlConn)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            //作用域模式注入
            services.AddScoped<ISqlSugarClient>(o =>
            {
                var client = new SqlSugarClient(new ConnectionConfig()
                {
                    //必填, 数据库连接字符串
                    ConnectionString = sqlConn,

                    //必填, 数据库类型
                    DbType = (DbType)DbType.SqlServer,

                    //默认false, 是否自动关闭数据库连接, 设置为true无需使用using或者Close操作
                    IsAutoCloseConnection = false,

                    //默认SystemTable, 字段信息读取, 如：该属性是不是主键，标识列等等信息
                    InitKeyType = InitKeyType.Attribute
                });

                //用来打印Sql方便调式
                client.Aop.OnLogExecuting = (sql, pars) =>
                {
                    Console.WriteLine(sql + "\r\n" +
                    client.Utilities.SerializeObject(pars.ToDictionary(it => it.ParameterName, it => it.Value)));
                    Console.WriteLine();
                };
                client.Aop.OnError = (err) =>
                {
                    Console.WriteLine(err + "\r\n");
                    Console.WriteLine();
                };

                return client;
            });
        }
    }
}