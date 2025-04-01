using Xunit;
using RCFilter;

namespace XUnitTestRCFilter
{
    public class RCFiltertest
    {
        [Fact]
        public void Vout_ShouldReturnExpectedOutput()
        {
            // Arrange
            double R = 1000;
            double C = 0.000001; // 1 µF
            double vin = 5; // 5 V
            double freq = 1000; // 1 kHz
            var filter = new PassiveRCFilter(R, C);

            // Act
            double vout = filter.Vout(vin, freq);

            // Assert
            // Verwachte output berekend met formule
            double omega = 2 * Math.PI * freq;
            double reactance = 1 / (omega * C);
            double expectedVout = vin * (reactance / Math.Sqrt(R * R + reactance * reactance));
            Assert.Equal(expectedVout, vout, 5); // precisie tot op 5 decimalen
        }

        [Fact]
        public void CutOff_ShouldReturnCorrectFrequency()
        {
            // Arrange
            double R = 1000;
            double C = 0.000001;
            var filter = new PassiveRCFilter(R, C);

            // Act
            double cutoff = filter.CutOff();

            // Assert
            double expectedCutoff = 1 / (2 * Math.PI * R * C);
            Assert.Equal(expectedCutoff, cutoff, 5);
        }

        [Fact]
        public void PhaseShift_ShouldReturnCorrectAngle()
        {
            // Arrange
            double R = 1000;
            double C = 0.000001;
            double freq = 1000;
            var filter = new PassiveRCFilter(R, C);

            // Act
            double phase = filter.PhaseShift(freq);

            // Assert
            double omega = 2 * Math.PI * freq;
            double reactance = 1 / (omega * C);
            double expectedPhase = Math.Atan(reactance / R) * (180 / Math.PI);
            Assert.Equal(expectedPhase, phase, 5);
        }
    }
}
