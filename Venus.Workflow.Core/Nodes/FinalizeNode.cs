namespace Venus.Workflow.Core.Nodes;

public class FinalizeNode: GroupNode
{
    protected async override Task InnerExecuteAsync(FlowContext context)
    {
        foreach (var node in Nodes)
        {
            await node.ExecuteAsync(context);
            if (node.Exception is OperationCanceledException)
            {
                throw node.Exception;
            }
        }
    }
}
