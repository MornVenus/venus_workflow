using Microsoft.Extensions.Logging;
using Venus.Workflow.Core;
using Venus.Workflow.Core.Models.Pipelines;
using Venus.Workflow.Core.Nodes;

namespace WorkflowCore.Sample;

public class LogMiddleware : IMiddleware
{
    public async Task ExecuteAsync(FlowContext context, MiddlewareDelegate next)
    {
        if (context.CurrentNode is FinalizeNode)
        {
            context.Logger?.LogInformation($"[{context.CurrentNode?.Name} 节点开始]");
            await next(context);
            context.Logger?.LogInformation($"[{context.CurrentNode?.Name} 节点结束]");
        }
        else
        {
            await next(context);
        }
    }
}
