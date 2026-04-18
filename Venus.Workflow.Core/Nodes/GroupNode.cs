using Venus.Workflow.Core.Utils;

namespace Venus.Workflow.Core.Nodes;

public class GroupNode: FlowNode
{
    public List<FlowNode> Nodes { get; set; } = new();

    protected override async Task InnerExecuteAsync(FlowContext context)
    {
        await WorkflowUtil.ExecuteNodesAsync(context, Nodes);
    }
}
