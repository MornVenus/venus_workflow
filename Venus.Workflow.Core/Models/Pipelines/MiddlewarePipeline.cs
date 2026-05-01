namespace Venus.Workflow.Core.Models.Pipelines;

/// <summary>
/// 中间件管道构建器
/// </summary>
public interface IMiddlewarePipeline
{
    /// <summary>
    /// 添加中间件
    /// </summary>
    void Use(IMiddleware middleware);

    /// <summary>
    /// 执行管道
    /// </summary>
    Task ExecuteAsync(Func<Task> action, FlowContext context);
}

/// <summary>
/// 中间件管道实现
/// </summary>
public class MiddlewarePipeline : IMiddlewarePipeline
{
    private readonly List<IMiddleware> _middlewares = new();

    /// <summary>
    /// 添加中间件实例
    /// </summary>
    public void Use(IMiddleware middleware)
    {
        _middlewares.Add(middleware);
    }

    /// <summary>
    /// 添加中间件类型（将通过Activator创建实例）
    /// </summary>
    public void Use<T>() where T : IMiddleware, new()
    {
        _middlewares.Add(new T());
    }

    /// <summary>
    /// 执行管道
    /// </summary>
    public async Task ExecuteAsync(Func<Task> action, FlowContext context)
    {
        // 构建管道
        MiddlewareDelegate pipeline = async ctx =>
        {
            await action();
        };

        // 逆序包装中间件（最后添加的中间件最外层）
        foreach (var middleware in _middlewares.AsEnumerable().Reverse())
        {
            var next = pipeline;
            pipeline = async ctx => await middleware.ExecuteAsync(ctx, next);
        }

        // 执行管道
        await pipeline(context);
    }

    /// <summary>
    /// 清空所有中间件
    /// </summary>
    public void Clear()
    {
        _middlewares.Clear();
    }
}