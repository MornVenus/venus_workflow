namespace Venus.Workflow.Core.Models.Pipelines;

/// <summary>
/// 全局中间件注册表（单例）
/// </summary>
public static class GlobalMiddlewareRegistry
{
    private static readonly List<IMiddleware> _globalMiddlewares = new();
    private static readonly object _lock = new();

    /// <summary>
    /// 注册全局中间件
    /// </summary>
    public static void Register(IMiddleware middleware)
    {
        lock (_lock)
        {
            _globalMiddlewares.Add(middleware);
        }
    }

    /// <summary>
    /// 注册全局中间件
    /// </summary>
    public static void Register<T>() where T : IMiddleware, new()
    {
        lock (_lock)
        {
            _globalMiddlewares.Add(new T());
        }
    }

    /// <summary>
    /// 获取所有全局中间件
    /// </summary>
    public static IEnumerable<IMiddleware> GetAll()
    {
        lock (_lock)
        {
            return _globalMiddlewares.ToList(); // 返回副本
        }
    }

    /// <summary>
    /// 清空全局中间件
    /// </summary>
    public static void Clear()
    {
        lock (_lock)
        {
            _globalMiddlewares.Clear();
        }
    }

    /// <summary>
    /// 创建默认管道（包含全局中间件）
    /// </summary>
    public static IMiddlewarePipeline CreateDefaultPipeline()
    {
        var pipeline = new MiddlewarePipeline();

        // 添加全局中间件
        foreach (var middleware in GetAll())
        {
            pipeline.Use(middleware);
        }

        return pipeline;
    }
}

