using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExMath.Statistics.Distributions
{
    /// <summary>
    /// Represents a statistical distribution object
    /// </summary>
    /// <typeparam name="T">The variable type</typeparam>
    public interface IDistribution<T>
        where T : struct, IComparable<T>
    {
        /// <summary>
        /// Returns the probability of the parameter occuring.
        /// </summary>
        /// <param name="parameter">The parameter to test</param>
        /// <returns>The probability</returns>
        double GetProbability(T parameter);

        /// <summary>
        /// Returns tthe cumulative probability of the parameter occuring.
        /// </summary>
        /// <param name="parameter">The parameter to test</param>
        /// <returns>The probability</returns>
        double GetCumulativeProbability(T parameter);

        /// <summary>
        /// Returns the cumulative probability of the parameter occuring.
        /// </summary>
        /// <param name="parameter">The parameter to test</param>
        /// <param name="mode">The mode to calculate the probability.</param>
        /// <returns>The probability of the result.</returns>
        double GetCumulativeProbability(T parameter, CumulativeMode mode);

        /// <summary>
        /// Returns the inverse of the distribution based on a quantile.
        /// </summary>
        /// <param name="quantile">The chance of the parameter occuring.</param>
        /// <returns>The parameter calculated from the inverse</returns>
        T GetInverse(double quantile);

        /// <summary>
        /// Returns the mean or expected value of the distribution.
        /// </summary>
        /// <returns>The mean value</returns>
        double GetExpected();

        /// <summary>
        /// Gets the variance of the distriubtion.
        /// </summary>
        /// <returns>The variance</returns>
        double GetVariance();

        /// <summary>
        /// Gets the standard deviation of the distribution.
        /// </summary>
        /// <returns>The standard deviation</returns>
        double GetStandardDeviation();
    }
}
