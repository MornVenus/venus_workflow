using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using Venus.Workflow.Core.Nodes;

namespace Venus.Workflow.GUI.Converters;

public class NodeTypeToIconConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var node = value as FlowNode;
        if (node == null)
            return Geometry.Empty;

        var typeName = node.GetType().Name;

        return typeName switch
        {
            "ActionNode" => CreateActionIcon(),
            "GroupNode" => CreateGroupIcon(),
            "FinalizeNode" => CreateFinalizeIcon(),
            _ => CreateDefaultIcon()
        };
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }

    private static Geometry CreateActionIcon()
    {
        // 播放三角形 ▶
        var geometry = new PathGeometry();
        var figure = new PathFigure { StartPoint = new System.Windows.Point(4, 0) };
        figure.Segments.Add(new LineSegment(new System.Windows.Point(20, 10), true));
        figure.Segments.Add(new LineSegment(new System.Windows.Point(4, 20), true));
        figure.IsClosed = true;
        geometry.Figures.Add(figure);
        return geometry;
    }

    private static Geometry CreateGroupIcon()
    {
        // 文件夹图标 📁
        var geometry = new PathGeometry();
        var figure = new PathFigure { StartPoint = new System.Windows.Point(2, 4) };
        figure.Segments.Add(new LineSegment(new System.Windows.Point(2, 18), true));
        figure.Segments.Add(new ArcSegment(new System.Windows.Point(4, 20), new System.Windows.Size(2, 2), 0, false, SweepDirection.Clockwise, true));
        figure.Segments.Add(new LineSegment(new System.Windows.Point(18, 20), true));
        figure.Segments.Add(new ArcSegment(new System.Windows.Point(20, 18), new System.Windows.Size(2, 2), 0, false, SweepDirection.Clockwise, true));
        figure.Segments.Add(new LineSegment(new System.Windows.Point(20, 8), true));
        figure.Segments.Add(new ArcSegment(new System.Windows.Point(18, 6), new System.Windows.Size(2, 2), 0, false, SweepDirection.Clockwise, true));
        figure.Segments.Add(new LineSegment(new System.Windows.Point(10, 6), true));
        figure.Segments.Add(new LineSegment(new System.Windows.Point(8, 4), true));
        figure.Segments.Add(new ArcSegment(new System.Windows.Point(6, 2), new System.Windows.Size(2, 2), 0, false, SweepDirection.Clockwise, true));
        figure.Segments.Add(new LineSegment(new System.Windows.Point(4, 2), true));
        figure.Segments.Add(new ArcSegment(new System.Windows.Point(2, 4), new System.Windows.Size(2, 2), 0, false, SweepDirection.Clockwise, true));
        figure.IsClosed = true;
        geometry.Figures.Add(figure);
        return geometry;
    }

    private static Geometry CreateFinalizeIcon()
    {
        // 旗帜/完成图标 🏁
        var geometry = new PathGeometry();
        var figure = new PathFigure { StartPoint = new System.Windows.Point(4, 2) };
        figure.Segments.Add(new LineSegment(new System.Windows.Point(4, 20), true));
        figure.Segments.Add(new LineSegment(new System.Windows.Point(6, 20), true));
        figure.Segments.Add(new LineSegment(new System.Windows.Point(6, 12), true));
        figure.Segments.Add(new LineSegment(new System.Windows.Point(18, 8), true));
        figure.Segments.Add(new LineSegment(new System.Windows.Point(6, 4), true));
        figure.Segments.Add(new LineSegment(new System.Windows.Point(6, 2), true));
        figure.IsClosed = true;
        geometry.Figures.Add(figure);
        return geometry;
    }

    private static Geometry CreateDefaultIcon()
    {
        // 默认圆点
        var geometry = new EllipseGeometry(new System.Windows.Point(10, 10), 6, 6);
        return geometry;
    }
}
