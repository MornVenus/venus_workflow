using System.Globalization;
using System.Windows.Controls;
using System.Windows.Data;
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
        if (values[0] is List<FlowNode> nodes1 && values[1] is List<FlowNode> nodes2)
        {
            return nodes1.Concat(nodes2).ToList();
        }
        return null;
    }
    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
