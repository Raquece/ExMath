using ExMath.Statistics.Distributions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestSuite.Statistics.Distributions
{
    public class BinomialTestSuite
    {
        [Theory]
        [InlineData(0, 0.05)] // Trials cannot be 0
        [InlineData(5, 2)] // Probability cannot be above 1
        [InlineData(5, -1)] // Probability cannot be less than 0
        public void ModelDistribution_IncorrectData_ThrowsException(uint trials, double probability)
        {
            var bd = new BinomialDistribution();
            Assert.ThrowsAny<Exception>(() => bd.Model(trials, probability));
        }

        [Theory]
        [InlineData(1, 0)]
        [InlineData(5, 1)]
        [InlineData(3, 0.52)]
        public void ModelDistribution_CorrectData_DoesNotThrow(uint trials, double probability)
        {
            var bd = new BinomialDistribution();
            bd.Model(trials, probability);
        }

        [Fact]
        public void ComputeProbability_ValidData_ReturnsCorrect()
        {
            const uint TRIALS = 5;
            const double PROBABILITY = 0.25d;
            const uint SUCCESSES = 3;
            const double EXPECTED = 0.08789d;

            var bd = new BinomialDistribution();
            bd.Model(TRIALS, PROBABILITY);
            var actual = bd.GetProbability(SUCCESSES);

            Assert.Equal(EXPECTED, Math.Round(actual, 5));
        }

        [Fact]
        public void ComputerProbability_InvalidData_ThrowsException()
        {
            const uint TRIALS = 5;
            const double PROBABILITY = 0.25d;

            var bd = new BinomialDistribution();
            bd.Model(TRIALS, PROBABILITY);
            Assert.ThrowsAny<Exception>(() => bd.GetProbability(8)); // Successes cannot be over trials
        }

        [Theory]
        [InlineData(3, 0.98438)]
        [InlineData(0, 0.23730)]
        public void ComputeCumulativeProbability_ValidData_ReturnsCorrect(uint SUCCESSES, double EXPECTED)
        {
            const uint TRIALS = 5;
            const double PROBABILITY = 0.25d;

            var bd = new BinomialDistribution();
            bd.Model(TRIALS, PROBABILITY);
            var actual = bd.GetCumulativeProbability(SUCCESSES);

            Assert.Equal(EXPECTED, Math.Round(actual, 5));
        }

        [Fact]
        public void ComputeCumulativeProbability_InvalidData_ThrowsException()
        {
            const uint TRIALS = 5;
            const double PROBABILITY = 0.25d;

            var bd = new BinomialDistribution();
            bd.Model(TRIALS, PROBABILITY);
            Assert.ThrowsAny<Exception>(() => bd.GetCumulativeProbability(7)); // Successes cannot be over trials
        }

        [Theory]
        [InlineData(CumulativeMode.LessThanOrEqual, 3, 0.98438)]
        [InlineData(CumulativeMode.LessThan, 3, 0.89648)]
        [InlineData(CumulativeMode.GreaterThanOrEqual, 3, 0.10352)]
        [InlineData(CumulativeMode.GreaterThan, 3, 0.01562)]
        [InlineData(CumulativeMode.LessThanOrEqual, 0, 0.23730)]
        [InlineData(CumulativeMode.LessThan, 0, 0)]
        [InlineData(CumulativeMode.GreaterThanOrEqual, 0, 1)]
        [InlineData(CumulativeMode.GreaterThan, 0, 0.76270)]
        public void ComputeCumulativeProbabilty_DifferentModes_ReturnsCorrect(CumulativeMode mode, uint SUCCESSES, double expected)
        {
            const uint TRIALS = 5;
            const double PROBABILITY = 0.25d;

            var bd = new BinomialDistribution();
            bd.Model(TRIALS, PROBABILITY);
            var actual = bd.GetCumulativeProbability(SUCCESSES, mode);

            Assert.Equal(expected, Math.Round(actual, 5));
        }

        [Theory]
        [InlineData(CumulativeMode.LessThanOrEqual)]
        [InlineData(CumulativeMode.LessThan)]
        [InlineData(CumulativeMode.GreaterThanOrEqual)]
        [InlineData(CumulativeMode.GreaterThan)]
        public void ComputeCumulativeProbabiltyModesOverride_InvalidData_ThrowsException(CumulativeMode mode)
        {
            const uint TRIALS = 5;
            const double PROBABILITY = 0.25d;

            var bd = new BinomialDistribution();
            bd.Model(TRIALS, PROBABILITY);
            Assert.ThrowsAny<Exception>(() => bd.GetCumulativeProbability(7, mode)); // Successes cannot be over trials
        }

        [Fact]
        public void ComputeCumulativeProbabilty_InvalidMode_ThrowsException()
        {
            const uint TRIALS = 5;
            const double PROBABILITY = 0.25d;

            var bd = new BinomialDistribution();
            bd.Model(TRIALS, PROBABILITY);
            Assert.ThrowsAny<Exception>(() => bd.GetCumulativeProbability(3, (CumulativeMode)514));
        }

        [Theory]
        [InlineData(-0.2)]
        [InlineData(4)]
        public void GetInverse_InvalidQuantile_ThrowsRelevantException(double quantile)
        {
            const uint TRIALS = 100;
            const double PROBABILITY = 0.3;

            var bd = new BinomialDistribution();
            bd.Model(TRIALS, PROBABILITY);

            Assert.ThrowsAny<ArgumentException>(() => bd.GetInverse(quantile));
        }

        [Theory]
        [InlineData(0.7, 32)]
        [InlineData(0, 0)]
        [InlineData(1, 100)]
        public void GetInverse_ReturnsCorrect(double quantile, uint expected)
        {
            const uint TRIALS = 100;
            const double PROBABILITY = 0.3;

            var bd = new BinomialDistribution();
            bd.Model(TRIALS, PROBABILITY);

            Assert.Equal(expected, bd.GetInverse(quantile));
        }

        [Fact]
        public void GetExpected_ReturnsCorrect()
        {
            const uint TRIALS = 10;
            const double PROBABILITY = 0.25d;

            var bd = new BinomialDistribution();
            bd.Model(TRIALS, PROBABILITY);

            Assert.Equal(2.5, bd.GetExpected());
        }

        [Fact]
        public void GetVariance_ReturnsCorrect()
        {
            const uint TRIALS = 10;
            const double PROBABILITY = 0.25d;

            var bd = new BinomialDistribution();
            bd.Model(TRIALS, PROBABILITY);

            Assert.Equal(1.8750, bd.GetVariance());
        }

        [Fact]
        public void GetStandardDeviation_ReturnsCorrect()
        {
            const uint TRIALS = 10;
            const double PROBABILITY = 0.25d;

            var bd = new BinomialDistribution();
            bd.Model(TRIALS, PROBABILITY);

            Assert.Equal(1.3693, bd.GetStandardDeviation(), 0.0001);
        }
    }
}
