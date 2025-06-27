using System.Reflection;
using NewLife.Agent;

namespace EasyTemplate.Schedule.Common;

public class ServiceCore
{
    private static IConfiguration configuration { get; set; }
    private static ServiceBase? svc { get; set; }
    public static string Prefix { get; set; }

    static ServiceCore()
    {
        // 创建配置构建器
        configuration = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();

        string? serviceName = configuration["Service:Name"];
        Prefix = configuration["Service:Prefix"];
        Prefix = string.IsNullOrWhiteSpace(Prefix) ? "" : Prefix + ".";
    }

    /// <summary>
    /// 获取配置
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public static string GetConfig(string key)
    {
        return configuration[key] ?? "";
    }

    /// <summary>
    /// 打印颜色文字 1成功输出 2报错输出 3校验未填写输出 0默认白色字体输出
    /// </summary>
    /// <param name="s"></param>
    /// <param name="type"></param>
    public static void WriteLine(string content, string s, int type = 0)
    {
        switch (type)
        {
            case 1: Console.ForegroundColor = ConsoleColor.Green; break;
            case 2: Console.ForegroundColor = ConsoleColor.Red; break;
            case 3: Console.ForegroundColor = ConsoleColor.Blue; break;
            default: Console.ForegroundColor = ConsoleColor.White; break;
        }
        Console.WriteLine($"{content}:  " + DateTime.Now + "   " + s);
        XTrace.Log.Info(content + ": {0}", s);
    }

    /// <summary>
    /// 启动时删除Config文件夹，防止修改服务后配置文件不同步
    /// </summary>
    public static void CleanConfigCache()
    {
        var path = $"{AppDomain.CurrentDomain.BaseDirectory}Config\\";
        var filePath = $"{path}Agent.config";
        if (Directory.Exists(path) && File.Exists(filePath))
        {
            File.Delete(filePath);
        }
    }

    /// <summary>
    /// 创建服务实例
    /// </summary>
    /// <param name="className"></param>
    /// <returns></returns>
    public static ServiceBase? CreateServiceInstance(string className)
    {
        // 获取当前程序集
        var assembly = Assembly.GetExecutingAssembly();

        // 查找与类名匹配的类型
        var type = assembly.GetTypes()
            .FirstOrDefault(t => t.Name.Equals(className, StringComparison.OrdinalIgnoreCase) && t.IsSubclassOf(typeof(ServiceBase)));

        // 如果类型存在且继承自 ServiceBase，则实例化对象
        if (type != null)
        {
            return Activator.CreateInstance(type) as ServiceBase;
        }

        // 如果类型不存在或不符合条件，返回 null
        return null;
    }

    /// <summary>
    /// 注册服务
    /// </summary>
    /// <param name="args"></param>
    /// <param name="services"></param>
    public static void Regist(string[] args)
    {
        CleanConfigCache();

        var serviceName = GetConfig("Service:Name");
        svc = CreateServiceInstance(serviceName);
        
        Console.ForegroundColor = ConsoleColor.Yellow;
        if (svc != null)
        {
            svc.ServiceName = svc.DisplayName = $"{Prefix}{serviceName}";
            Console.WriteLine($"\r\n当前服务：{svc.ServiceName} - {svc.Description}\r\n");
            svc.Main(args);
        }
        else
        {
            Console.WriteLine($"\r\n当前暂无可运行服务，请检查服务名称是否正确：{serviceName}\r\n");
            Console.ReadKey();
        }
    }
}
