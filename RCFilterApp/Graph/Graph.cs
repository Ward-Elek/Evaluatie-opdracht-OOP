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

        // Dynamische as-titels
        public string XAxisTitle { get; set; } = "Frequency [Hz]";
        public string YAxisTitle { get; set; } = "Value";

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
                    ? 60 + Math.Log10(frequencies[i] / minFreq) / Math.Log10(maxFreq / minFreq) * (width - 80)
                    : 60 + (frequencies[i] - minFreq) / (maxFreq - minFreq) * (width - 80);

                double y = 20 + (height - 60) - ((values[i] - minValue) / (maxValue - minValue)) * (height - 60);
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
            double marginLeft = 60;
            double marginBottom = 40;
            double marginTop = 20;

            // X-as tekenen
            Line xAxis = new Line
            {
                X1 = marginLeft,
                X2 = canvas.Width - 20,
                Y1 = canvas.Height - marginBottom,
                Y2 = canvas.Height - marginBottom,
                Stroke = Brushes.Black,
                StrokeThickness = 1
            };
            canvas.Children.Add(xAxis);

            // Y-as tekenen
            Line yAxis = new Line
            {
                X1 = marginLeft,
                X2 = marginLeft,
                Y1 = marginTop,
                Y2 = canvas.Height - marginBottom,
                Stroke = Brushes.Black,
                StrokeThickness = 1
            };
            canvas.Children.Add(yAxis);

            // X-as titel aanmaken
            TextBlock xTitle = new TextBlock
            {
                Text = XAxisTitle,
                Foreground = Brushes.Black,
                FontWeight = FontWeights.Bold
            };

            // Werkelijke breedte meten
            xTitle.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
            double actualXTitleWidth = xTitle.DesiredSize.Width;

            // X-as titel precies gecentreerd plaatsen onder de X-as
            Canvas.SetLeft(xTitle, marginLeft + (canvas.Width - marginLeft - 20) / 2 - actualXTitleWidth / 2);
            Canvas.SetTop(xTitle, canvas.Height - marginBottom + 10);
            canvas.Children.Add(xTitle);

            // Y-as titel aanmaken (geroteerd)
            TextBlock yTitle = new TextBlock
            {
                Text = YAxisTitle,
                Foreground = Brushes.Black,
                FontWeight = FontWeights.Bold,
                LayoutTransform = new RotateTransform(-90)
            };

            // Werkelijke grootte van geroteerde tekst meten
            yTitle.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
            double actualYTitleWidth = yTitle.DesiredSize.Width;
            double actualYTitleHeight = yTitle.DesiredSize.Height;

            // Y-as titel precies gecentreerd plaatsen naast de Y-as
            Canvas.SetLeft(yTitle, marginLeft - actualYTitleWidth - 5); 
            Canvas.SetTop(yTitle, marginTop + (canvas.Height - marginTop - marginBottom) / 2 - actualYTitleHeight / 2);
            canvas.Children.Add(yTitle);
        }




        private void DrawCutoffMarker()
        {
            if (cutoffFrequency <= 0) return;

            double marginLeft = 60;
            double cutoffX = logScale
                ? marginLeft + Math.Log10(cutoffFrequency / 10) / Math.Log10(100000 / 10) * (canvas.Width - 80)
                : marginLeft + (cutoffFrequency - 10) / (100000 - 10) * (canvas.Width - 80);

            Line cutoffLine = new Line
            {
                X1 = cutoffX,
                X2 = cutoffX,
                Y1 = 20,
                Y2 = canvas.Height - 40,
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
