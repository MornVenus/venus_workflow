using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Venus.Workflow.Core.Models;
using Venus.Workflow.Core.Nodes;

namespace Venus.Workflow.Core;

public class FlowContext: ViewModelBase
{
    /// <summary>
    /// 变量字典，用于节点间数据传递
    /// </summary>
    private readonly Dictionary<string, object> _variables = new();

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

    #region Methods

    /// <summary>
    /// 获取指定类型的变量值
    /// </summary>
    /// <typeparam name="T">变量类型</typeparam>
    /// <param name="key">变量键名</param>
    /// <returns>变量值</returns>
    /// <exception cref="KeyNotFoundException">变量不存在时抛出</exception>
    /// <exception cref="InvalidCastException">类型转换失败时抛出</exception>
    public T GetVariable<T>(string key)
    {
        if (!_variables.TryGetValue(key, out var value))
        {
            throw new KeyNotFoundException($"变量 '{key}' 不存在");
        }

        if (value is T typedValue)
        {
            return typedValue;
        }

        throw new InvalidCastException($"变量 '{key}' 的类型转换失败。期望类型: {typeof(T).FullName}, 实际类型: {value.GetType().FullName}");
    }

    /// <summary>
    /// 获取指定类型的变量值，不存在时返回默认值
    /// </summary>
    /// <typeparam name="T">变量类型</typeparam>
    /// <param name="key">变量键名</param>
    /// <param name="defaultValue">默认值</param>
    /// <returns>变量值或默认值</returns>
    public T GetVariable<T>(string key, T defaultValue)
    {
        if (!_variables.TryGetValue(key, out var value))
        {
            return defaultValue;
        }

        if (value is T typedValue)
        {
            return typedValue;
        }

        return defaultValue;
    }

    /// <summary>
    /// 尝试获取指定类型的变量值
    /// </summary>
    /// <typeparam name="T">变量类型</typeparam>
    /// <param name="key">变量键名</param>
    /// <param name="value">输出变量值</param>
    /// <returns>是否成功获取</returns>
    public bool TryGetVariable<T>(string key, out T value)
    {
        value = default!;

        if (!_variables.TryGetValue(key, out var objValue))
        {
            return false;
        }

        if (objValue is T typedValue)
        {
            value = typedValue;
            return true;
        }

        return false;
    }

    /// <summary>
    /// 设置变量值
    /// </summary>
    /// <typeparam name="T">变量类型</typeparam>
    /// <param name="key">变量键名</param>
    /// <param name="value">变量值</param>
    public void SetVariable<T>(string key, T value)
    {
        _variables[key] = value!;
    }

    public T GetService<T>() where T : class
    {
        return this.ServiceProvider?.GetService<T>();
    }

    #endregion Methods
}
