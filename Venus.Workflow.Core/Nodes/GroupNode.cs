using Venus.Workflow.Core.Models.Pipelines;

namespace Venus.Workflow.Core.Nodes;

public class GroupNode: FlowNode
{
    public List<FlowNode> Nodes { get; set; } = new();

    protected override async Task InnerExecuteAsync(FlowContext context)
    {
        foreach (var node in Nodes)
        {
            context.CurrentNode = node;
            var pipeline = GlobalMiddlewareRegistry.CreateDefaultPipeline();
            await pipeline.ExecuteAsync(() => node.ExecuteAsync(context), context);
            if (node.Exception != null)
            {
                Exception = node.Exception;
                break;
            }
        }
    }
}
