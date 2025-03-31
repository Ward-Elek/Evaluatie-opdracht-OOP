using System;
using System.Collections.Generic;
using System.Windows.Controls;
using RCFilter;

namespace GraphLibrary
{
    public class BodePlot
    {
        private Graph gainGraph;
        private Graph phaseGraph;
        private PassiveRCFilter rcFilter;
        private double vin;
        private bool logScale;

        public BodePlot(double resistance, double capacitance, double vin, bool logScale = true)
        {
            // Maak RCFilter instantie
            rcFilter = new PassiveRCFilter(resistance, capacitance);
            this.vin = vin;
            this.logScale = logScale;

            // Maak grafieken voor Gain en Phase
            gainGraph = new Graph();
            phaseGraph = new Graph();
        }

        public void SetCanvas(Canvas gainCanvas, Canvas phaseCanvas)
        {
            // Instellen canvas en titels voor Gain grafiek
            gainGraph.SetCanvas(gainCanvas);
            gainGraph.SetTitle("Gain vs Frequency");
            gainGraph.XAxisTitle = "Frequency [Hz]";
            gainGraph.YAxisTitle = "Gain [dB]";

            // Instellen canvas en titels voor Phase grafiek
            phaseGraph.SetCanvas(phaseCanvas);
            phaseGraph.SetTitle("Phase vs Frequency");
            phaseGraph.XAxisTitle = "Frequency [Hz]";
            phaseGraph.YAxisTitle = "Phase Shift [°]";
        }

        public void GenerateData()
        {
            // Lijsten voor frequenties, amplitudes en faseverschuivingen
            List<double> frequencies = new List<double>();
            List<double> magnitudes = new List<double>();
            List<double> phases = new List<double>();

            // Haal cutoff frequentie uit RCFilter klasse
            double cutoff = rcFilter.CutOff();
            gainGraph.SetCutoffFrequency(cutoff);
            phaseGraph.SetCutoffFrequency(cutoff);

            // Bereken waarden voor frequentiebereik
            for (double f = 1; f <= 100000; f *= 1.2)
            {
                frequencies.Add(f);

                // Bereken uitgangsspanning via RCFilter
                double vout = rcFilter.Vout(vin, f);
                // Vermijd NaN of negatieve oneindige waarden
                double gainDb = double.IsNaN(vout) || vout <= 0 ? -100 : 20 * Math.Log10(vout / vin);
                magnitudes.Add(gainDb);

                // Bereken faseverschuiving via RCFilter en spiegel deze
                double phase = -rcFilter.PhaseShift(f);
                phases.Add(phase);
            }

            // Stel gegevens in voor grafieken (gebruik breedte en hoogte zoals in UI voorzien)
            gainGraph.SetData(frequencies, magnitudes, 600, 250, logScale);
            phaseGraph.SetData(frequencies, phases, 600, 250, logScale);
        }

        public void Draw()
        {
            // Teken beide grafieken
            gainGraph.Draw();
            phaseGraph.Draw();
        }
    }
}