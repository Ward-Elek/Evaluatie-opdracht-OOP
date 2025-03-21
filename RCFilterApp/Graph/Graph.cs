using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Graphs
{
    public class Graph
    {
        private Canvas canvas;
        private string title;
        private List<Point> dataPoints;

        // Constructor
        public Graph()
        {
            dataPoints = new List<Point>();
        }

        // Properties
        public Canvas Canvas
        {
            get { return canvas; }
            set { canvas = value; }
        }

        public string Title
        {
            get { return title; }
            set { title = value; }
        }

        public List<Point> DataPoints
        {
            get { return dataPoints; }
            set { dataPoints = value; }
        }

        // Methode om de grafiek te tekenen
        public void Draw()
        {
            if (canvas == null || dataPoints.Count == 0) return;
            canvas.Children.Clear();

            double minX = double.MaxValue, maxX = double.MinValue;
            double minY = double.MaxValue, maxY = double.MinValue;

            // Zoek min en max waarden van X en Y
            foreach (var point in dataPoints)
            {
                if (point.X < minX) minX = point.X;
                if (point.X > maxX) maxX = point.X;
                if (point.Y < minY) minY = point.Y;
                if (point.Y > maxY) maxY = point.Y;
            }

            double scaleX = (canvas.Width - 20) / (maxX - minX);
            double scaleY = (canvas.Height - 20) / (maxY - minY);

            Polyline polyline = new Polyline { Stroke = Brushes.Blue, StrokeThickness = 2 };
            PointCollection points = new PointCollection();

            foreach (var point in dataPoints)
            {
                double x = 10 + (point.X - minX) * scaleX;
                double y = canvas.Height - 10 - (point.Y - minY) * scaleY;
                points.Add(new Point(x, y));
            }

            polyline.Points = points;
            canvas.Children.Add(polyline);
        }

    }
}
