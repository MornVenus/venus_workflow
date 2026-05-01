namespace Venus.Workflow.Core.Models.Pipelines;

/// <summary>
/// 中间件执行委托
/// </summary>
public delegate Task MiddlewareDelegate(FlowContext context);

/// <summary>
/// 中间件接口
/// </summary>
public interface IMiddleware
{
    Task ExecuteAsync(FlowContext context, MiddlewareDelegate next);
}
