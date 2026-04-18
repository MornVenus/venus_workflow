using Venus.Workflow.Core.Models;
using Venus.Workflow.Core.Nodes;
using Venus.Workflow.Core.Utils;

namespace Venus.Workflow.Core;

/// <summary>
/// 工作流引擎，负责工作流的执行
/// </summary>
public class FlowEngine: ViewModelBase
{
    public FlowEngine(FlowContext context)
    {
        Context = context;
    }

    public FlowContext Context { get; }

    public Exception? Exception { get; private set; }

    private CancellationTokenSource? _cts;

    /// <summary>
    /// 工作流节点列表
    /// </summary>
    public List<FlowNode> Nodes { get; } = new();

    public FinalizeNode FinalizeNode { get; } = new FinalizeNode() { Name = "Finalize" };

    #region MVVMProperty State

    private FlowState _state;

    /// <summary>
    /// 当前状态
    /// </summary>
    public FlowState State
    {
        get => _state;
        set => Set(ref _state, value);
    }

    #endregion	MVVMProperty State

    /// <summary>
    /// 执行工作流
    /// </summary>
    /// <returns>异步任务</returns>
    public async Task ExecuteAsync()
    {
        if (State == FlowState.Running)
        {
            throw new InvalidOperationException("工作流正在执行中，无法重复启动");
        }

        _cts = new CancellationTokenSource();
        Context.CancellationToken = _cts.Token;

        try
        {
            await WorkflowUtil.ExecuteNodesAsync(Context, Nodes);
        }
        catch (OperationCanceledException ex)
        {
            State = FlowState.Cancel;
            Exception = ex;
        }
        catch (Exception ex)
        {
            State = FlowState.Error;
            Exception = ex;
        }
        finally
        {
            if (FinalizeNode.Nodes.Count > 0)
            {
                // 为 Finalize 节点创建一个新的 CancellationToken
                // 这样即使主流程被取消，Finalize 节点也能正常执行
                using var finalizeCts = new CancellationTokenSource();
                Context.CancellationToken = finalizeCts.Token;
                await FinalizeNode.ExecuteAsync(Context);
            }
        }
    }

    /// <summary>
    /// 取消工作流执行
    /// </summary>
    public void Cancel()
    {
        _cts?.Cancel();
    }
}
