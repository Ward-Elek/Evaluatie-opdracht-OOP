using GraphLibrary;
using RCFilter;
using System;
using System.Collections.Generic;
using System.Windows;

namespace RCFilterApp
{
    public partial class MainWindow : Window
    {
        private BodePlot bodePlot;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnCalculate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Input uitlezen
                double resistance = double.Parse(txtResistance.Text);
                double capacitance = double.Parse(txtCapacitance.Text);
                double vin = double.Parse(txtVin.Text);
                double frequency = double.Parse(txtFrequency.Text);

                // Berekeningen uitvoeren met RCFilter
                PassiveRCFilter filter = new PassiveRCFilter(resistance, capacitance);

                double cutoff = filter.CutOff();
                double vout = filter.Vout(vin, frequency);
                double phaseShift = filter.PhaseShift(frequency);

                // Resultaten tonen in labels
                lblVout.Text = $"Vout = {vout:F2} V";
                lblCutOff.Text = $"Cut-off = {cutoff:F0} Hz";
                lblPhaseShift.Text = $"Phase shift = {phaseShift:F0}°";

                // BodePlot genereren en tekenen
                bodePlot = new BodePlot(resistance, capacitance, vin);
                bodePlot.SetCanvas(canvasBodePlot, canvasPhasePlot);
                bodePlot.GenerateData();
                bodePlot.Draw();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fout: {ex.Message}", "Fout", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}