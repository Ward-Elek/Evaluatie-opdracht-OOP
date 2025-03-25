using System;
using System.Collections.Generic;
using System.Windows.Controls;

namespace GraphLibrary
{
    public class BodePlot
    {
        private Graph gainGraph;
        private Graph phaseGraph;
        private double resistance;
        private double capacitance;
        private double vin;

        public BodePlot(double resistance, double capacitance, double vin)
        {
            this.resistance = resistance;
            this.capacitance = capacitance;
            this.vin = vin;
            gainGraph = new Graph();
            phaseGraph = new Graph();
        }

        public void SetCanvas(Canvas gainCanvas, Canvas phaseCanvas)
        {
            gainGraph.SetCanvas(gainCanvas);
            gainGraph.SetTitle("Gain vs Frequency");
            gainGraph.XAxisTitle = "Frequency [Hz]";
            gainGraph.YAxisTitle = "Gain [dB]";

            phaseGraph.SetCanvas(phaseCanvas);
            phaseGraph.SetTitle("Phase vs Frequency");
            phaseGraph.XAxisTitle = "Frequency [Hz]";
            phaseGraph.YAxisTitle = "Phase Shift [°]";
        }

        public void GenerateData()
        {
            List<double> frequencies = new List<double>();
            List<double> magnitudes = new List<double>();
            List<double> phases = new List<double>();

            double cutoff = 1 / (2 * Math.PI * resistance * capacitance);
            gainGraph.SetCutoffFrequency(cutoff);
            phaseGraph.SetCutoffFrequency(cutoff);

            for (double f = 10; f <= 100000; f *= 1.2)
            {
                frequencies.Add(f);
                double omega = 2 * Math.PI * f;
                double denom = Math.Sqrt(1 + Math.Pow(omega * resistance * capacitance, 2));
                double gain = vin / denom;
                double gainDb = 20 * Math.Log10(gain / vin);
                double phase = -Math.Atan(omega * resistance * capacitance) * 180 / Math.PI;

                magnitudes.Add(gainDb);
                phases.Add(phase);
            }

            gainGraph.SetData(frequencies, magnitudes, 600, 250);
            phaseGraph.SetData(frequencies, phases, 600, 250);
        }

        public void Draw()
        {
            gainGraph.Draw();
            phaseGraph.Draw();
        }
    }
}
