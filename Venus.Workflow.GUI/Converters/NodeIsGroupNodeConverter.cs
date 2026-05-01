using System.Globalization;
using System.Windows;
using System.Windows.Data;
using Venus.Workflow.Core.Nodes;

namespace Venus.Workflow.GUI.Converters;

public class NodeIsGroupNodeConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var node = value as FlowNode;
        if (node == null)
            return Visibility.Collapsed;

        // 检查是否是 GroupNode 类型（包括 FinalizeNode，它继承自 GroupNode）
        return node is GroupNode ? Visibility.Visible : Visibility.Collapsed;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
