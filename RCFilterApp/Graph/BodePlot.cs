using System.Windows;
using System.Windows.Controls;

namespace Graphs
{
    public class BodePlot
    {
        private Graph magnitudePlot;
        private Graph phasePlot;

        public BodePlot()
        {
            magnitudePlot = new Graph();
            phasePlot = new Graph();
        }

        public void SetCanvas(Canvas magnitudeCanvas, Canvas phaseCanvas)
        {
            magnitudePlot.Canvas = magnitudeCanvas;
            phasePlot.Canvas = phaseCanvas;
        }

        public void Draw(List<double> frequencies, List<double> magnitudes, List<double> phases)
        {
            List<Point> magnitudePoints = new List<Point>();
            List<Point> phasePoints = new List<Point>();

            double minFreq = Math.Log10(frequencies[0]);
            double maxFreq = Math.Log10(frequencies[frequencies.Count - 1]);

            for (int i = 0; i < frequencies.Count; i++)
            {
                double x = (Math.Log10(frequencies[i]) - minFreq) / (maxFreq - minFreq) * 600;
                double yMag = 200 - magnitudes[i] * 5;  // Omgekeerde schaal gefixt
                double yPhase = 200 - phases[i];

                magnitudePoints.Add(new Point(x, yMag));
                phasePoints.Add(new Point(x, yPhase));
            }

            magnitudePlot.DataPoints = magnitudePoints;
            magnitudePlot.Draw();
            phasePlot.DataPoints = phasePoints;
            phasePlot.Draw();
        }

    }
}
