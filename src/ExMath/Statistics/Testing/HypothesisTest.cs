using ExMath.Statistics.Distributions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExMath.Statistics.Testing
{
    public static class HypothesisTest
    {
        /// <summary>
        /// Tests whether a stated hypothesis is correct.
        /// </summary>
        /// <typeparam name="T">The type parameter of the variable being tested (this will be in the summary of each distribution).</typeparam>
        /// <param name="distribution">The distribution being used as a test.</param>
        /// <param name="alternate">The alternate hypothesis that is being tested. Null hypothesis will be assumed from this.</param>
        /// <param name="actual">The observed value of the testing parameter</param>
        /// <param name="significanceLevel">The significance level that determines the critical region of the test</param>
        /// <returns>Whether the new hypothesis should be accepted.</returns>        
        /// <exception cref="ArgumentException">Throws if hypotheses is invalid</exception>
        public static bool Test<T>(IDistribution<T> distribution, Hypothesis alternate, T actual, double significanceLevel = 0.05d)
            where T : struct, IComparable<T>
        {
            if (alternate.Equality == Equality.Equal || alternate.Equality == Equality.GreaterThanOrEqual || alternate.Equality == Equality.LessThanOrEqual)
            {
                throw new ArgumentException("The alternate hypothesis cannot be equal, greater than or equal or less than equal.");
            }

            double pGreater = 1;
            double pLower = 1;

            if (alternate.Equality == Equality.NotEqual)
            {
                pGreater = distribution.GetCumulativeProbability(actual, CumulativeMode.GreaterThanOrEqual);
                pLower = distribution.GetCumulativeProbability(actual, CumulativeMode.LessThanOrEqual);
                significanceLevel /= 2;
            }
            else if (alternate.Equality == Equality.GreaterThan)
            {
                pGreater = distribution.GetCumulativeProbability(actual, CumulativeMode.GreaterThanOrEqual);
            }
            else if (alternate.Equality == Equality.LessThan)
            {
                pLower = distribution.GetCumulativeProbability(actual, CumulativeMode.LessThanOrEqual);
            }

            return (pGreater <= significanceLevel) || (pLower <= significanceLevel);
        }
    }
}
