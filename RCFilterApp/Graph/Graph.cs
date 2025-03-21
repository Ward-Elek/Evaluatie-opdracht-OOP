using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows;

namespace GraphLibrary
{
    public class Graph
    {
        private Canvas canvas;
        private string title;
        private List<Point> points;
        private double cutoffFrequency;
        private bool logScale;

        public Graph()
        {
            points = new List<Point>();
        }

        public void SetCanvas(Canvas canvas)
        {
            this.canvas = canvas;
        }

        public void SetTitle(string title)
        {
            this.title = title;
        }

        public void SetCutoffFrequency(double cutoff)
        {
            this.cutoffFrequency = cutoff;
        }

        public void SetData(List<double> frequencies, List<double> values, double width, double height, bool logScale = true)
        {
            points.Clear();
            this.logScale = logScale;
            double maxFreq = frequencies[^1];
            double minFreq = frequencies[0];
            double maxValue = -99999;
            double minValue = 99999;

            foreach (double value in values)
            {
                if (value > maxValue) maxValue = value;
                if (value < minValue) minValue = value;
            }

            for (int i = 0; i < frequencies.Count; i++)
            {
                double x = logScale
                    ? Math.Log10(frequencies[i]) / Math.Log10(maxFreq) * width
                    : (frequencies[i] - minFreq) / (maxFreq - minFreq) * width;

                double y = height - ((values[i] - minValue) / (maxValue - minValue)) * height;
                points.Add(new Point(x, y));
            }
        }

        public void Draw()
        {
            if (canvas == null || points.Count == 0) return;
            canvas.Children.Clear();

            Polyline line = new Polyline
            {
                Stroke = Brushes.Blue,
                StrokeThickness = 2
            };
            foreach (var pt in points)
            {
                line.Points.Add(pt);
            }
            canvas.Children.Add(line);

            DrawAxes();
            DrawCutoffMarker();
        }

        private void DrawAxes()
        {
            Line xAxis = new Line
            {
                X1 = 0,
                X2 = canvas.Width,
                Y1 = canvas.Height - 20,
                Y2 = canvas.Height - 20,
                Stroke = Brushes.Black,
                StrokeThickness = 1
            };
            canvas.Children.Add(xAxis);

            Line yAxis = new Line
            {
                X1 = 20,
                X2 = 20,
                Y1 = 0,
                Y2 = canvas.Height,
                Stroke = Brushes.Black,
                StrokeThickness = 1
            };
            canvas.Children.Add(yAxis);
        }

        private void DrawCutoffMarker()
        {
            if (cutoffFrequency <= 0) return;

            double cutoffX = logScale
                ? Math.Log10(cutoffFrequency) / Math.Log10(100000) * canvas.Width
                : (cutoffFrequency - 10) / (100000 - 10) * canvas.Width;

            Line cutoffLine = new Line
            {
                X1 = cutoffX,
                X2 = cutoffX,
                Y1 = 0,
                Y2 = canvas.Height,
                Stroke = Brushes.Red,
                StrokeThickness = 2,
                StrokeDashArray = new DoubleCollection { 4, 4 }
            };
            canvas.Children.Add(cutoffLine);

            TextBlock cutoffLabel = new TextBlock
            {
                Text = $"fc = {cutoffFrequency:F0} Hz",
                Foreground = Brushes.Red,
                FontSize = 12,
                FontWeight = FontWeights.Bold
            };
            Canvas.SetLeft(cutoffLabel, cutoffX + 5);
            Canvas.SetTop(cutoffLabel, 5);
            canvas.Children.Add(cutoffLabel);
        }
    }
}
