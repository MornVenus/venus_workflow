using Microsoft.Extensions.Logging;

namespace Venus.Workflow.Core.Nodes;

/// <summary>
/// For循环节点 - 数值范围迭代
/// 支持指定起始值、结束值和步长
/// </summary>
public class ForNode : GroupNode
{
    /// <summary>
    /// 循环起始值（包含）
    /// </summary>
    public int Start { get; set; } = 0;

    /// <summary>
    /// 循环结束值（不包含）
    /// </summary>
    public int End { get; set; } = 1;

    /// <summary>
    /// 步长，每次迭代的增量
    /// </summary>
    public int Step { get; set; } = 1;

    /// <summary>
    /// 循环变量名，用于在循环体中访问当前索引
    /// </summary>
    public string IndexVariableName { get; set; } = "i";

    public override async Task ExecuteAsync(FlowContext context)
    {
        State = FlowState.Running;
        context.Logger?.LogInformation("开始执行For循环节点: {Name}, 范围: [{Start}, {Count}), 步长: {Step}",
            Name, Start, End, Step);

        try
        {
            // 执行前延时
            if (DelayBefore > 0)
            {
                await Task.Delay(DelayBefore, context.CancellationToken);
            }

            var stopwatch = System.Diagnostics.Stopwatch.StartNew();
            int iterationCount = 0;

            // 验证步长
            if (Step == 0)
            {
                throw new ArgumentException("步长不能为0", nameof(Step));
            }

            // 执行循环
            for (int i = Start;
                 (Step > 0 && i < End) || (Step < 0 && i > End);
                 i += Step)
            {
                context.CancellationToken.ThrowIfCancellationRequested();

                // 设置循环变量
                context.SetVariable(IndexVariableName, i);
                context.Logger?.LogDebug("For循环迭代 {Iteration}, 当前索引: {Index}", iterationCount, i);

                // 执行循环体中的所有节点（继承自 GroupNode 的 Nodes 属性）
                foreach (var node in Nodes)
                {
                    context.CancellationToken.ThrowIfCancellationRequested();
                    context.CurrentNode = node;
                    context.Logger?.LogDebug("执行For循环体节点: {NodeName}", node.Name);

                    await node.ExecuteAsync(context);
                }

                iterationCount++;
            }

            stopwatch.Stop();
            ExecutionTime = (int)stopwatch.ElapsedMilliseconds;

            context.Logger?.LogInformation("For循环执行完成: {Name}, 迭代次数: {Count}, 耗时: {Time}ms",
                Name, iterationCount, ExecutionTime);

            // 执行后延时
            if (DelayAfter > 0)
            {
                await Task.Delay(DelayAfter, context.CancellationToken);
            }

            State = FlowState.Complete;
        }
        catch (OperationCanceledException)
        {
            State = FlowState.Cancel;
            context.Logger?.LogWarning("For循环执行被取消: {Name}", Name);
            throw;
        }
        catch (Exception ex)
        {
            State = FlowState.Error;
            context.Logger?.LogError(ex, "For循环执行出错: {Name}", Name);
            throw;
        }
    }
}
