using Microsoft.Extensions.Logging;
using Venus.Workflow.Core.Models;
using Venus.Workflow.Core.Nodes;

namespace Venus.Workflow.Core;

public class FlowContext: ViewModelBase
{
    #region Properties

    public ILogger? Logger { get; set; }

    /// <summary>
    /// IOC依赖容器
    /// </summary>
    public IServiceProvider? ServiceProvider { get; set; }

    /// <summary>
    /// 取消令牌
    /// </summary>
    public CancellationToken CancellationToken { get; set; }

    #region MVVMProperty CurrentNode

    private FlowNode? _currentNode;

    public FlowNode? CurrentNode
    {
        get => _currentNode;
        set => Set(ref _currentNode, value);
    }

    #endregion	MVVMProperty CurrentNode

    #endregion Properties
}
