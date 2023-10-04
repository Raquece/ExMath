using System.Diagnostics.CodeAnalysis;

namespace ExMath.Statistics.Testing
{
    /// <summary>
    /// Represents a mathematical hypothesis
    /// </summary>
    public class Hypothesis : IEquatable<Hypothesis>
    {
        /// <summary>
        /// The type of equality of the hypothesis.
        /// </summary>
        public Equality Equality { get; set; }

        /// <summary>
        /// The probability of the hypothesis occuring.
        /// </summary>
        public double Probability { get; set; }

        private static readonly Dictionary<string, Equality> EqualityValues = new Dictionary<string, Equality>()
        {
            { "=", Equality.Equal },
            { "!=", Equality.NotEqual },
            { ">", Equality.GreaterThan },
            { ">=", Equality.GreaterThanOrEqual },
            { "<", Equality.LessThan },
            { "<=", Equality.LessThanOrEqual }
        };

        /// <summary>
        /// Generates a hypothesis object.
        /// </summary>
        /// <param name="equality">The type of equality to hypothesise.</param>
        /// <param name="probability">The probability of the hypothesis occuring.</param>
        /// <returns>The hypothesis of the inputted parameters.</returns>
        public static Hypothesis Import(Equality equality, double probability)
        {
            return new Hypothesis()
            {
                Equality = equality,
                Probability = probability
            };
        }

        /// <summary>
        /// Returns the hypothesis in a string form.
        /// </summary>
        /// <returns>The hypothesis as a string.</returns>
        public override string ToString()
        {
            return "p " + EqualityValues.FirstOrDefault((x) => x.Value == Equality).Key + " " + Probability;
        }

        /// <summary>
        /// Compares two hypotheses.
        /// </summary>
        /// <param name="obj">The object to compare</param>
        /// <returns>Whether they are the same</returns>
        public override bool Equals(object obj)
        {
            return Equals(obj as Hypothesis);
        }

        /// <summary>
        /// Compares two hypotheses.
        /// </summary>
        /// <param name="other">The hypothesis to compare</param>
        /// <returns>Whether they are the same</returns>
        public bool Equals([AllowNull] Hypothesis other)
        {
            return other != null
                && Equality == other.Equality
                && Probability == other.Probability;
        }

        /// <summary>
        /// Compares two hypotheses.
        /// </summary>
        /// <param name="left">The first hypothesis to compare</param>
        /// <param name="right">The second hypothesis to compare</param>
        /// <returns>Whether they are the same</returns>
        public static bool operator ==(Hypothesis left, Hypothesis right)
        {
            return EqualityComparer<Hypothesis>.Default.Equals(left, right);
        }

        /// <summary>
        /// Compares two hypotheses.
        /// </summary>
        /// <param name="left">The first hypothesis to compare</param>
        /// <param name="right">The second hypothesis to compare</param>
        /// <returns>Whether they are not the same</returns>
        public static bool operator !=(Hypothesis left, Hypothesis right)
        {
            return !(left == right);
        }
    }
}
