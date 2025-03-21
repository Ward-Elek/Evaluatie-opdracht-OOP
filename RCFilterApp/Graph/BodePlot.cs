using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;


namespace Graphs
{
    public class BodePlot
    {
        private Graph gainGraph;
        private Graph phaseGraph;

        private double resistance;
        private double capacitance;
        private double vin;

        // Constructor
        public BodePlot(double resistance, double capacitance, double vin, double width, double height)
        {
            this.resistance = resistance;
            this.capacitance = capacitance;
            this.vin = vin;

            gainGraph = new Graph("Gain", width, height);
            phaseGraph = new Graph("Phase", width, height);
        }

        // Data genereren en doorgeven
        public void GenerateData()
        {
            List<double> frequencies = new List<double>();
            List<double> gainValues = new List<double>();
            List<double> phaseValues = new List<double>();

            for (double f = 1; f <= 100000; f *= 1.2)
            {
                frequencies.Add(f);

                double gain = 1 / Math.Sqrt(1 + Math.Pow(2 * Math.PI * f * resistance * capacitance, 2));
                gainValues.Add(20 * Math.Log10(gain));

                double phase = -Math.Atan(2 * Math.PI * f * resistance * capacitance) * (180 / Math.PI);
                phaseValues.Add(phase);
            }

            gainGraph.SetData(frequencies, gainValues);
            phaseGraph.SetData(frequencies, phaseValues);
        }

        // Grafieken tekenen
        public (Path gainPath, Path phasePath) Draw()
        {
            Path gain = gainGraph.Draw();
            Path phase = phaseGraph.Draw();
            return (gain, phase);
        }
    }
}
