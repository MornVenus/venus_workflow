namespace Venus.Workflow.Core.Nodes;

/// <summary>
/// 简单的动作节点，用于测试
/// </summary>
public class ActionNode : FlowNode
{
    public Func<FlowContext, Task> Action { get; }

    public ActionNode(Func<FlowContext, Task> action)
    {
        Action = action;
    }

    protected async override Task InnerExecuteAsync(FlowContext context)
    {
        await Action(context);
    }
}
