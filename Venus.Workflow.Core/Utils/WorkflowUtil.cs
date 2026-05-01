using Venus.Workflow.Core.Models.Pipelines;
using Venus.Workflow.Core.Nodes;

namespace Venus.Workflow.Core.Utils;

public class WorkflowUtil
{
    /// <summary>
    /// 执行多个节点，支持前后延时和状态管理
    /// </summary>
    /// <param name="context"></param>
    /// <param name="nodes"></param>
    /// <returns></returns>
    public static async Task ExecuteNodesAsync(FlowContext context, List<FlowNode> nodes)
    {
        foreach (var node in nodes)
        {
            context.CurrentNode = node;
            var pipeline = GlobalMiddlewareRegistry.CreateDefaultPipeline();
            await pipeline.ExecuteAsync(()=> node.ExecuteAsync(context), context);
            if (node.Exception != null)
            {
                throw node.Exception;
            }
        }
    }
}
