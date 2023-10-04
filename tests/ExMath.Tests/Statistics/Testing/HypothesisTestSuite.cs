using ExMath.Statistics.Distributions;
using ExMath.Statistics.Testing;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestSuite.Statistics.Testing
{
    public class HypothesisTestSuite
    {
        private class HypothesisTesting : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                yield return new object[] { NewHypo(Equality.GreaterThan, 0.7d), 50, 40, false };
                yield return new object[] { NewHypo(Equality.LessThan, 0.45d), 50, 18, false };
                yield return new object[] { NewHypo(Equality.NotEqual, 0.45d), 50, 0, true };
                yield return new object[] { NewHypo(Equality.NotEqual, 0.35d), 20, 9, false };
                yield return new object[] { NewHypo(Equality.NotEqual, 0.35d), 20, 12, true };
                yield return new object[] { NewHypo(Equality.NotEqual, 0.35d), 20, 11, false };
            }

            private static Hypothesis NewHypo(Equality equality, double probability)
            {
                return new Hypothesis()
                {
                    Equality = equality,
                    Probability = probability
                };
            }

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }

        [Fact]
        public void Test_InvalidEquality_ThrowsException()
        {
            var d = new BinomialDistribution();
            d.Model(50, 0.05d);

            var current = new Hypothesis()
            {
                Equality = Equality.Equal,
                Probability = 0.05d
            };

            var alt = new Hypothesis()
            {
                Equality = Equality.GreaterThanOrEqual,
                Probability = 0.05d
            };

            Assert.ThrowsAny<Exception>(() => HypothesisTest.Test<uint>(d, alt, 3));
        }

        [Theory]
        [ClassData(typeof(HypothesisTesting))]
        public void Test_Binomial_ReturnsCorrect(Hypothesis alt, uint trials, uint actual, bool expected)
        {
            var d = new BinomialDistribution();
            d.Model(trials, alt.Probability);

            Assert.Equal(expected, HypothesisTest.Test(d, alt, actual));
        }
    }
}
