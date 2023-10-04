namespace ExMath.Statistics.Distributions
{
    /// <summary>
    /// Represents a binomial distribution
    /// </summary>
    public class BinomialDistribution : IDistribution<uint>
    {
        /// <summary>
        /// The number of trials
        /// </summary>
        public uint Trials { get; private set; }

        /// <summary>
        /// The probability of a success
        /// </summary>
        public double Probability { get; private set; }

        /// <summary>
        /// Models the current distribution
        /// </summary>
        /// <param name="trials">The number of trials</param>
        /// <param name="probability">The probability of a success</param>
        public void Model(uint trials, double probability)
        {
            if (trials <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(trials), "The number of trials cannot be less than 0.");
            }
            if (probability < 0 || probability > 1)
            {
                throw new ArgumentOutOfRangeException(nameof(probability), "The probability of a success must be between 0 and 1.");
            }

            Trials = trials;
            Probability = probability;
        }

        /// <summary>
        /// Returns the probabilty that there are less than or equal to <paramref name="x"/> successes.
        /// </summary>
        /// <param name="x">The number of trials to expect</param>
        /// <returns>The probability of the result</returns>
        public double GetCumulativeProbability(uint x)
        {
            if (x > Trials)
            {
                throw new ArgumentOutOfRangeException(nameof(x), "The number of successes expected cannot be greater than the number of trials.");
            }

            return x == 0 ? GetProbability(0) : GetProbability(x) + GetCumulativeProbability(x - 1);
        }

        /// <summary>
        /// Returns the probabilty of successes matching the specified cumulative mode.
        /// </summary>
        /// <param name="x">The number of trials to expect</param>
        /// <param name="mode">The mode of which to calculate the probability.</param>
        /// <returns>The probability of the result</returns>
        public double GetCumulativeProbability(uint x, CumulativeMode mode)
        {
            if (x > Trials)
            {
                throw new ArgumentOutOfRangeException(nameof(x), "The number of successes expected cannot be greater than the number of trials.");
            }

            return mode switch
            {
                CumulativeMode.LessThanOrEqual => x == 0 ? GetProbability(0) : GetProbability(x) + GetCumulativeProbability(x - 1),
                CumulativeMode.LessThan => x == 0 ? 0 : GetProbability(x - 1) + GetCumulativeProbability(x - 2),
                CumulativeMode.GreaterThanOrEqual => x == 0 ? 1 : 1 - GetProbability(x - 1) - GetCumulativeProbability(x - 2),
                CumulativeMode.GreaterThan => x == 0 ? 1 - GetProbability(0) : 1 - GetProbability(x) - GetCumulativeProbability(x - 1),
                _ => throw new ArgumentException("Cumulative mode is invalid"),
            };
        }

        /// <summary>
        /// Returns the probabilty that there are exactly <paramref name="x"/> successes.
        /// </summary>
        /// <param name="x">The number of trials to expect.</param>
        /// <returns>The probability of the result.</returns>
        public double GetProbability(uint x)
        {
            if (x > Trials)
            {
                throw new ArgumentOutOfRangeException(nameof(x), "The number of successes expected cannot be greater than the number of trials.");
            }

            // Calculates the probability
            return ExMathLib.Combination(Trials, x) * Math.Pow((double)Probability, x) * Math.Pow(1 - (double)Probability, Trials - x);
        }

        /// <summary>
        /// Returns the inverse of the distribution based on a quantile.
        /// </summary>
        /// <param name="quantile">The chance of the parameter occuring.</param>
        /// <returns>The parameter calculated from the inverse</returns>
        public uint GetInverse(double quantile)
        {
            if (quantile < 0 || quantile > 1)
            {
                throw new ArgumentOutOfRangeException(nameof(quantile), "Quantile must be a valid probability");
            }

            for (uint i = 0; i < Trials; i++)
            {
                if (GetCumulativeProbability((uint)i) >= quantile)
                {
                    return i;
                }
            }

            return Trials;
        }

        /// <summary>
        /// Returns the mean or expected value of the distribution.
        /// </summary>
        /// <returns>The mean value</returns>
        public double GetExpected()
        {
            return Trials * Probability;
        }

        /// <summary>
        /// Gets the variance of the distriubtion.
        /// </summary>
        /// <returns>The variance</returns>
        public double GetVariance()
        {
            return GetExpected() * (1 - Probability);
        }

        /// <summary>
        /// Gets the standard deviation of the distribution.
        /// </summary>
        /// <returns>The standard deviation</returns>
        public double GetStandardDeviation()
        {
            return Math.Sqrt(GetVariance());
        }
    }
}
