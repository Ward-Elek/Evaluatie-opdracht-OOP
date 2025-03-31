using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows;
using System.Linq;

namespace GraphLibrary
{
    public class Graph
    {
        private Canvas canvas;
        private string title;
        private List<Point> points;
        private double cutoffFrequency;
        private bool logScale;
        private double minFreq, maxFreq, minValue, maxValue;

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
            maxFreq = frequencies.Max();
            minFreq = frequencies.Min();
            maxValue = values.Max();
            minValue = values.Min();

            for (int i = 0; i < frequencies.Count; i++)
            {
                double x = logScale
                    ? 60 + Math.Log10(frequencies[i]) / Math.Log10(100000) * (width - 80)
                    : 60 + (frequencies[i] - minFreq) / (maxFreq - minFreq) * (width - 80);

                double y = 20 + (height - 60) - ((values[i] - minValue) / (maxValue - minValue)) * (height - 60);
                points.Add(new Point(x, y));
            }
        }

        public void Draw()
        {
            if (canvas == null || points.Count == 0) return;
            canvas.Children.Clear();

            DrawGrid();

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
            DrawLabels();

        }

        private void DrawAxes()
        {
            double marginLeft = 60;
            double marginBottom = 40;
            double marginTop = 20;

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

            TextBlock xTitle = new TextBlock
            {
                Text = XAxisTitle,
                Foreground = Brushes.Black,
                FontWeight = FontWeights.Bold
            };
            xTitle.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
            double actualXTitleWidth = xTitle.DesiredSize.Width;
            Canvas.SetLeft(xTitle, marginLeft + (canvas.Width - marginLeft - 20) / 2 - actualXTitleWidth / 2);
            Canvas.SetTop(xTitle, canvas.Height - marginBottom + 10);
            canvas.Children.Add(xTitle);

            TextBlock yTitle = new TextBlock
            {
                Text = YAxisTitle,
                Foreground = Brushes.Black,
                FontWeight = FontWeights.Bold,
                LayoutTransform = new RotateTransform(-90)
            };
            yTitle.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
            double actualYTitleWidth = yTitle.DesiredSize.Width;
            double actualYTitleHeight = yTitle.DesiredSize.Height;
            Canvas.SetLeft(yTitle, marginLeft - actualYTitleWidth - 5);
            Canvas.SetTop(yTitle, marginTop + (canvas.Height - marginTop - marginBottom) / 2 - actualYTitleHeight / 2);
            canvas.Children.Add(yTitle);
        }

        private void DrawCutoffMarker()
        {
            if (cutoffFrequency <= 0) return;

            double marginLeft = 60;
            double cutoffX = logScale
                ? marginLeft + Math.Log10(cutoffFrequency) / Math.Log10(100000) * (canvas.Width - 80)
                : marginLeft + (cutoffFrequency - minFreq) / (maxFreq - minFreq) * (canvas.Width - 80);

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

        private void DrawGrid()
        {
            double marginLeft = 60;
            double marginBottom = 40;
            double marginTop = 20;
            double graphWidth = canvas.Width - marginLeft - 20;
            double graphHeight = canvas.Height - marginBottom - marginTop;

            for (int i = 0; i <= 10; i++)
            {
                double x = marginLeft + i * (graphWidth / 10);
                Line vLine = new Line
                {
                    X1 = x,
                    Y1 = marginTop,
                    X2 = x,
                    Y2 = marginTop + graphHeight,
                    Stroke = Brushes.LightGray,
                    StrokeThickness = 0.5
                };
                canvas.Children.Add(vLine);
            }

            for (int i = 0; i <= 10; i++)
            {
                double y = marginTop + i * (graphHeight / 10);
                Line hLine = new Line
                {
                    X1 = marginLeft,
                    X2 = marginLeft + graphWidth,
                    Y1 = y,
                    Y2 = y,
                    Stroke = Brushes.LightGray,
                    StrokeThickness = 0.5
                };
                canvas.Children.Add(hLine);
            }
        }

        private void DrawLabels()
        {
            double marginLeft = 60;
            double marginBottom = 40;
            double marginTop = 20;
            double graphWidth = canvas.Width - marginLeft - 20;
            double graphHeight = canvas.Height - marginBottom - marginTop;

            // --- X-as labels (log of lineair)
            if (logScale)
            {
                double[] logFreqs = { 1, 10, 100, 1000, 10000, 100000 };

                foreach (double freq in logFreqs)
                {
                    double x = marginLeft + Math.Log10(freq) / Math.Log10(100000) * graphWidth;
                    string labelText = freq >= 1000 ? $"{freq / 1000:0.#}k" : $"{freq}";

                    TextBlock label = new TextBlock
                    {
                        Text = labelText,
                        Foreground = Brushes.Black,
                        FontSize = 10
                    };
                    Canvas.SetLeft(label, x - 10);
                    Canvas.SetTop(label, canvas.Height - marginBottom + 2);
                    canvas.Children.Add(label);
                }
            }
            else
            {
                for (int i = 0; i <= 10; i++)
                {
                    double freq = minFreq + i * (maxFreq - minFreq) / 10.0;
                    double x = marginLeft + i * (graphWidth / 10);
                    string labelText = freq >= 1000 ? $"{freq / 1000:0.#}k" : $"{freq:0}";

                    TextBlock label = new TextBlock
                    {
                        Text = labelText,
                        Foreground = Brushes.Black,
                        FontSize = 10
                    };
                    Canvas.SetLeft(label, x - 10);
                    Canvas.SetTop(label, canvas.Height - marginBottom + 2);
                    canvas.Children.Add(label);
                }
            }

            // --- Y-as labels
            double yMin = minValue;
            double yMax = maxValue;

            // Als het om een faseshift gaat: geforceerd 0–90°
            if (YAxisTitle.ToLower().Contains("phase"))
            {
                yMin = 0;
                yMax = 90;
            }

            for (int i = 0; i <= 10; i++)
            {
                double value = yMax - i * (yMax - yMin) / 10.0;
                double y = marginTop + i * (graphHeight / 10);

                TextBlock label = new TextBlock
                {
                    Text = $"{value:0}",
                    Foreground = Brushes.Black,
                    FontSize = 10
                };
                Canvas.SetLeft(label, marginLeft - 35);
                Canvas.SetTop(label, y - 6);
                canvas.Children.Add(label);
            }
        }


    }
}