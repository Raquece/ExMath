using ExMath.Exceptions;

namespace ExMath.Extensions
{
    /// <summary>
    /// Provides extension methods for numerical objects
    /// </summary>
    public static class Numerics
    {
        /// <summary>
        /// Tests whether an object is a number
        /// </summary>
        /// <typeparam name="T">The type to test</typeparam>
        /// <returns>Whether or not the type is a number</returns>
        public static bool IsNumber<T>()
        {
            return typeof(T) == typeof(sbyte)
                    || typeof(T) == typeof(byte)
                    || typeof(T) == typeof(short)
                    || typeof(T) == typeof(ushort)
                    || typeof(T) == typeof(int)
                    || typeof(T) == typeof(uint)
                    || typeof(T) == typeof(long)
                    || typeof(T) == typeof(ulong)
                    || typeof(T) == typeof(float)
                    || typeof(T) == typeof(double)
                    || typeof(T) == typeof(decimal);
        }

        /// <summary>
        /// Throws if <see cref="T"/> is not a numerical type
        /// </summary>
        /// <typeparam name="T">The type to test</typeparam>
        public static void ThrowIfNotNumber<T>()
        {
            if (!IsNumber<T>())
                throw new InvalidNumericTypeException($"The type '{nameof(T)}' must be a numerical type");
        }

        /// <summary>
        /// Returns the factorial of a number.
        /// </summary>
        /// <param name="number">The number to calculate the factorial of.</param>
        /// <returns>The value of the factorial</returns>
        public static ulong Factorial(this ulong number)
        {
            if (number == 1 || number == 0)
            {
                return 1;
            }
            else
            {
                return number * Factorial(number - 1);
            }
        }

        /// <summary>
        /// Returns the factorial of a number.
        /// </summary>
        /// <param name="number">The number to calculate the factorial of.</param>
        /// <returns>The value of the factorial</returns>
        public static ulong Factorial(this int number)
        {
            return Factorial((ulong)number);
        }
    }
}
