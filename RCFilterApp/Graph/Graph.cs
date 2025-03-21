using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;


namespace Graphs
{
    public class Graph
    {
        // Attributen
        private string title;
        private List<Point> dataPoints;
        private double width;
        private double height;

        // Constructor
        public Graph(string title, double width, double height)
        {
            this.title = title;
            this.width = width;
            this.height = height;
            this.dataPoints = new List<Point>();
        }

        // Dataset instellen
        public void SetData(List<double> frequencies, List<double> values, bool isLogX = true)
        {
            dataPoints.Clear();
            double maxFreq = frequencies[^1];
            double minFreq = frequencies[0];

            for (int i = 0; i < frequencies.Count; i++)
            {
                double x = isLogX
                    ? Math.Log10(frequencies[i]) / Math.Log10(maxFreq) * width
                    : (frequencies[i] - minFreq) / (maxFreq - minFreq) * width;

                double y = height / 2 - values[i] * 2; // Ruimtelijke scaling
                dataPoints.Add(new Point(x, y));
            }
        }

        // Grafiek tekenen
        public Path Draw()
        {
            PolyLineSegment segment = new PolyLineSegment();
            foreach (var point in dataPoints)
            {
                segment.Points.Add(point);
            }

            PathFigure figure = new PathFigure
            {
                StartPoint = dataPoints.Count > 0 ? dataPoints[0] : new Point(0, 0),
                Segments = new PathSegmentCollection { segment }
            };

            PathGeometry geometry = new PathGeometry();
            geometry.Figures.Add(figure);

            Path path = new Path
            {
                Stroke = Brushes.Blue,
                StrokeThickness = 2,
                Data = geometry
            };

            return path;
        }
    }
}