using Graphs;
using RCFilter;
using System.Windows;
using System.Windows.Media.Media3D;

namespace RCFilterApp
{
    public partial class MainWindow : Window
    {
        private BodePlot bodePlot;

        public MainWindow()
        {
            InitializeComponent();
            bodePlot = new BodePlot();
            bodePlot.SetCanvas(canvasBodePlot, canvasPhasePlot);
        }

        private void btnCalculate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                double resistance = double.Parse(txtResistance.Text);
                double capacitance = double.Parse(txtCapacitance.Text);
                double vin = double.Parse(txtVin.Text);

                PassiveRCFilter filter = new PassiveRCFilter(resistance, capacitance);

                double cutoff = filter.CutOff();
                double vout = filter.Vout(vin, cutoff);
                double phaseShift = filter.PhaseShift(cutoff);

                // Zorg dat de berekende waarden correct in de labels verschijnen
                lblVout.Text = $"Vout = {vout:F2} V";
                lblCutOff.Text = $"Cut-off = {cutoff:F0} Hz";
                lblPhaseShift.Text = $"Phase shift = {phaseShift:F0}°";

                // Genereer de Bode-plot
                List<double> frequencies = new List<double>();
                List<double> magnitudes = new List<double>();
                List<double> phases = new List<double>();

                for (double f = 10; f <= 100000; f *= 1.2)
                {
                    frequencies.Add(f);
                    double voutF = filter.Vout(vin, f);
                    magnitudes.Add(20 * Math.Log10(voutF / vin));
                    phases.Add(filter.PhaseShift(f));
                }

                BodePlot bodePlot = new BodePlot(resistance, capacitance, vin);

                bodePlot.GenerateData();
                var paths = bodePlot.Draw();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fout: {ex.Message}", "Fout", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

    }
}

