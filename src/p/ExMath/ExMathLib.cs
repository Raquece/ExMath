namespace ExMath
{
    /// <summary>
    /// Contains static implementations of common mathematics functions
    /// </summary>
    public static class ExMathLib
    {
        public static double Factorial(uint @base)
        {
            double root = @base;

            if (root == 1 || root == 0)
            {
                return 1;
            }
            else
            {
                return root * Factorial((uint)(root - 1));
            }
        }

        public static double Combination(uint c, uint r)
        {
            if (r > c)
            {
                throw new ArgumentOutOfRangeException(nameof(r), "r must not be greater than c");
            }

            return Factorial(c) / (Factorial(r) * Factorial(c - r));
        }
    }
}
