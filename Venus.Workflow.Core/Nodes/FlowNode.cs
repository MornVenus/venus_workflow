using Venus.Workflow.Core.Models;

namespace Venus.Workflow.Core.Nodes;

/// <summary>
/// 工作流状态枚举，可表示整个工作流的状态或某个节点的状态
/// </summary>
public enum FlowState
{
    /// <summary>
    /// 空闲状态，等待执行
    /// </summary>
    Idle,

    /// <summary>
    /// 正在运行
    /// </summary>
    Running,

    /// <summary>
    /// 已取消
    /// </summary>
    Cancel,

    /// <summary>
    /// 等待中（如等待外部事件、延时等）
    /// </summary>
    Waiting,

    /// <summary>
    /// 执行出错
    /// </summary>
    Error,

    /// <summary>
    /// 被跳过（如条件不满足）
    /// </summary>
    Skip,

    /// <summary>
    /// 执行完成
    /// </summary>
    Complete
}

/// <summary>
/// 工作流节点基类，所有工作流节点的抽象基类
/// </summary>
public abstract class FlowNode: ViewModelBase
{
    public FlowNode()
    {
        _name = this.GetType().Name;
        _description = _name;
    }

    #region Properties

    #region MVVMProperty Name

    private string _name;

    /// <summary>
    /// 节点名称
    /// </summary>
    public string Name
    {
        get => _name;
        set => Set(ref _name, value);
    }

    #endregion	MVVMProperty Name

    #region MVVMProperty Description

    private string _description;

    /// <summary>
    /// 节点描述
    /// </summary>
    public string Description
    {
        get => _description;
        set => Set(ref _description, value);
    }

    #endregion	MVVMProperty Description

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

    #region MVVMProperty ExecutionTime

    private int _executionTime;

    /// <summary>
    /// 执行耗时（毫秒）
    /// </summary>
    public int ExecutionTime
    {
        get => _executionTime;
        set => Set(ref _executionTime, value);
    }

    #endregion	MVVMProperty ExecutionTime

    #region MVVMProperty Message

    private string _message;

    public string Message
    {
        get => _message;
        set => Set(ref _message, value);
    }

    #endregion	MVVMProperty Message

    #region MVVMProperty Exception

    private Exception? _exception;

    public Exception? Exception
    {
        get => _exception;
        internal set => Set(ref _exception, value);
    }

    #endregion	MVVMProperty Exception


    /// <summary>
    /// 执行前延时（毫秒）
    /// </summary>
    public int DelayBefore { get; set; }

    /// <summary>
    /// 执行后延时（毫秒）
    /// </summary>
    public int DelayAfter { get; set; }

    #endregion Properties

    #region Methods

    /// <summary>
    /// 执行节点逻辑
    /// </summary>
    /// <param name="context">工作流执行上下文</param>
    /// <returns>异步任务</returns>
    public virtual async Task ExecuteAsync(FlowContext context)
    {
        Exception = null;
        context.CurrentNode = this;
        State = FlowState.Running;
        var startTime = Environment.TickCount; // 记录开始时间

        try
        {
            context.CancellationToken.ThrowIfCancellationRequested();
            // 执行前延时
            if (DelayBefore > 0)
            {
                await Task.Delay(DelayBefore, context.CancellationToken);
            }

            await InnerExecuteAsync(context);

            // 执行后延时
            if (DelayAfter > 0)
            {
                await Task.Delay(DelayAfter, context.CancellationToken);
            }

            if (State == FlowState.Running)
            {
                State = FlowState.Complete;
            }
        }
        catch (Exception ex)
        {
            OnException(context, ex);
        }
        finally
        {
            ExecutionTime = Environment.TickCount - startTime; // 计算执行耗时
        }
    }

    /// <summary>
    /// 内部执行方法，子类必须实现具体的执行逻辑
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    protected abstract Task InnerExecuteAsync(FlowContext context);

    public override string ToString()
    {
        return $"{Name}:{Description}";
    }

    protected virtual void OnException(FlowContext context, Exception ex)
    {
        Exception = ex;
        if (ex is OperationCanceledException)
        {
            State = FlowState.Cancel;
        }
        else
        {
            State = FlowState.Error;
        }
    }

    #endregion Methods
}
