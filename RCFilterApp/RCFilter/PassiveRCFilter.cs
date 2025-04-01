namespace RCFilter
{
    public class PassiveRCFilter
    {
        // Attributen
        private double resistance;
        private double capacitance;

        // Constructor
        public PassiveRCFilter(double R, double C)
        {
            resistance = R;
            capacitance = C;
        }

        // Properties
        public double Resistance
        {
            get { return resistance; }
            set { resistance = value; }
        }

        public double Capacitance
        {
            get { return capacitance; }
            set { capacitance = value; }
        }

        // Methode Uout te berekenen met F
        public double Vout(double vin, double frequency)
        {
            double omega = 2 * Math.PI * frequency;
            double reactance = 1 / (omega * capacitance);
            double outputVoltage = vin * (reactance / Math.Sqrt(Math.Pow(resistance, 2) + Math.Pow(reactance, 2)));

            return outputVoltage;
        }

        // Methode F-cutoff
        public double CutOff()
        {
            return 1 / (2 * Math.PI * resistance * capacitance);
        }

        // Faseverschuiving
        public double PhaseShift(double frequency)
        {
            double omega = 2 * Math.PI * frequency;
            double reactance = 1 / (omega * capacitance);
            return Math.Atan(reactance / resistance) * (180 / Math.PI);
        }
    }
}

