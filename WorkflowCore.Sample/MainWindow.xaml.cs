using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using System.Windows;
using Venus.Workflow.Core;
using Venus.Workflow.Core.Models.Pipelines;
using Venus.Workflow.Core.Nodes;
using Venus.Workflow.GUI.Views;

namespace WorkflowCore.Sample;

public partial class MainWindow : Window
{
    private  IServiceProvider _serviceProvider;

    private ILogger _logger;

    private FlowEngine _engine;

    public MainWindow()
    {
        InitializeComponent();

        var services = new ServiceCollection();
        services.AddLogging(loggerBuilder =>
        {
            loggerBuilder.AddNLog();
        });

        _serviceProvider = services.BuildServiceProvider();

        var logFactory = _serviceProvider.GetRequiredService<ILoggerFactory>();
        _logger = logFactory.CreateLogger<MainWindow>();
    }

    private async void StartBtn_Click(object sender, RoutedEventArgs e)
    {
        GlobalMiddlewareRegistry.Register(new LogMiddleware());

        FlowContext context = new FlowContext()
        {
            Logger = _logger,
            ServiceProvider = _serviceProvider,
        };

        FlowEngine engine = new FlowEngine(context);
        _engine = engine;

        ActionNode node1 = new ActionNode(async (ctx) =>
        {
            _logger.LogInformation("Executing Node1");
            await Task.Delay(3000, ctx.CancellationToken);
        })
        {
            Name = "Node1",
        };

        GroupNode gNode = new() { Name = "GroupNode"};

        ActionNode node2 = new ActionNode(async (ctx) =>
        {
            _logger.LogInformation("Executing Node2");
            await Task.Delay(1000);
            throw new Exception("test exception");
        })
        {
            Name = "Node2",
        };

        gNode.Nodes.Add(node2);

        engine.Nodes.Add(node1);
        engine.Nodes.Add(gNode);

        ActionNode node3 = new ActionNode(async (ctx) =>
        {
            _logger.LogInformation("Executing Node3");
            await Task.Delay(1000);
        })
        {
            Name = "Node3",
        };

        engine.FinalizeNode.Nodes.Add(node3);

        WorkflowView.DataContext = engine;

        await engine.ExecuteAsync();
        if (engine.Exception != null)
        {
            _logger.LogError(engine.Exception.StackTrace);
        }
    }

    private void CancelBtn_Click(object sender, RoutedEventArgs e)
    {
        _engine.Cancel();
    }
}