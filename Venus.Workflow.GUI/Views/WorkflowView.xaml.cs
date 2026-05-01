using System.Globalization;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using Venus.Workflow.Core.Nodes;

namespace Venus.Workflow.GUI.Views;

public partial class WorkflowView : UserControl
{
    public WorkflowView()
    {
        InitializeComponent();
    }
}

public class NodesConverter: IMultiValueConverter
{
    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
        if (values[0] is List<FlowNode> nodes1 && values[1] is FlowNode finalizeNode)
        {
            var nodes = nodes1.ToList();
            nodes.Add(finalizeNode);
            return nodes;
        }
        return null;
    }
    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

public class StateToTextConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is not FlowState state)
            return "未知";

        return state switch
        {
            FlowState.Idle => "等待中",
            FlowState.Running => "运行中",
            FlowState.Waiting => "等待中",
            FlowState.Cancel => "已取消",
            FlowState.Error => "错误",
            FlowState.Skip => "已跳过",
            FlowState.Complete => "已完成",
            _ => "未知"
        };
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

public class StateToBrushConverter : IMultiValueConverter
{
    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
        if (values.Length == 0 || values[0] is not FlowState state)
            return Brushes.Gray;

        var brushType = parameter as string ?? "Indicator";

        return brushType switch
        {
            "Indicator" => GetIndicatorBrush(state),
            "TagBackground" => GetTagBackgroundBrush(state),
            "TagDot" => GetTagDotBrush(state),
            "TagText" => GetTagTextBrush(state),
            _ => GetIndicatorBrush(state)
        };
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }

    private static Brush GetIndicatorBrush(FlowState state)
    {
        return state switch
        {
            FlowState.Idle => new SolidColorBrush(Color.FromRgb(128, 128, 128)),
            FlowState.Running => new SolidColorBrush(Color.FromRgb(33, 150, 243)),
            FlowState.Waiting => new SolidColorBrush(Color.FromRgb(255, 193, 7)),
            FlowState.Cancel => new SolidColorBrush(Color.FromRgb(156, 39, 176)),
            FlowState.Error => new SolidColorBrush(Color.FromRgb(244, 67, 54)),
            FlowState.Skip => new SolidColorBrush(Color.FromRgb(158, 158, 158)),
            FlowState.Complete => new SolidColorBrush(Color.FromRgb(76, 175, 80)),
            _ => Brushes.Gray
        };
    }

    private static Brush GetTagBackgroundBrush(FlowState state)
    {
        return state switch
        {
            FlowState.Idle => new SolidColorBrush(Color.FromRgb(245, 245, 245)),
            FlowState.Running => new SolidColorBrush(Color.FromRgb(227, 242, 253)),
            FlowState.Waiting => new SolidColorBrush(Color.FromRgb(255, 248, 225)),
            FlowState.Cancel => new SolidColorBrush(Color.FromRgb(243, 229, 245)),
            FlowState.Error => new SolidColorBrush(Color.FromRgb(255, 235, 238)),
            FlowState.Skip => new SolidColorBrush(Color.FromRgb(250, 250, 250)),
            FlowState.Complete => new SolidColorBrush(Color.FromRgb(232, 245, 233)),
            _ => new SolidColorBrush(Color.FromRgb(245, 245, 245))
        };
    }

    private static Brush GetTagDotBrush(FlowState state)
    {
        return GetIndicatorBrush(state);
    }

    private static Brush GetTagTextBrush(FlowState state)
    {
        return state switch
        {
            FlowState.Idle => new SolidColorBrush(Color.FromRgb(97, 97, 97)),
            FlowState.Running => new SolidColorBrush(Color.FromRgb(21, 101, 192)),
            FlowState.Waiting => new SolidColorBrush(Color.FromRgb(245, 127, 23)),
            FlowState.Cancel => new SolidColorBrush(Color.FromRgb(123, 31, 162)),
            FlowState.Error => new SolidColorBrush(Color.FromRgb(198, 40, 40)),
            FlowState.Skip => new SolidColorBrush(Color.FromRgb(117, 117, 117)),
            FlowState.Complete => new SolidColorBrush(Color.FromRgb(46, 125, 50)),
            _ => new SolidColorBrush(Color.FromRgb(97, 97, 97))
        };
    }
}

public class NodeTypeToIconColorConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var node = value as FlowNode;
        if (node == null)
            return new SolidColorBrush(Color.FromRgb(108, 117, 125));

        var typeName = node.GetType().Name;

        return typeName switch
        {
            "ActionNode" => new SolidColorBrush(Color.FromRgb(33, 150, 243)),
            "GroupNode" => new SolidColorBrush(Color.FromRgb(255, 152, 0)),
            "FinalizeNode" => new SolidColorBrush(Color.FromRgb(76, 175, 80)),
            _ => new SolidColorBrush(Color.FromRgb(108, 117, 125))
        };
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
