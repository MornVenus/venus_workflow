using Venus.Workflow.Core.Models.Pipelines;

namespace Venus.Workflow.Core.Nodes;

public class FinalizeNode: GroupNode
{
    protected async override Task InnerExecuteAsync(FlowContext context)
    {
        foreach (var node in Nodes)
        {
            context.CurrentNode = node;
            var pipeline = GlobalMiddlewareRegistry.CreateDefaultPipeline();
            await pipeline.ExecuteAsync(() => node.ExecuteAsync(context), context);
            if (node.Exception is OperationCanceledException)
            {
                throw node.Exception;
            }
        }
    }
}
