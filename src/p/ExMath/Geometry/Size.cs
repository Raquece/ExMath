using System;
using System.Linq;
using ExMath.Extensions;

namespace ExMath.Geometry
{
    /// <summary>
    /// Represents a stored dimensional size.
    /// </summary>
    [Serializable]
    public class Size : IEquatable<Size>, ICloneable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Size" /> class.
        /// </summary>
        /// <param name="dimensions">The dimensions of the object.</param>
        public Size(params int[] dimensions)
        {
            if (dimensions.Length == 0)
            {
                throw new ArgumentException("Cannot create 0 dimensional size.");
            }
            if (dimensions.Any(i => i <= 0))
            {
                throw new ArgumentException("All dimensions must be a positive integer.");
            }

            Dimensions = dimensions;
        }

        /// <summary>
        /// Gets or sets the dimensions of the shape.
        /// </summary>
        public int[] Dimensions { get; set; }

        /// <summary>
        /// Gets the dimension of the shape.
        /// </summary>
        public int Dimension => Dimensions.Length;

        /// <summary>
        /// Inverts the dimensions of the shape.
        /// </summary>
        public void Invert()
        {
            Dimensions = Dimensions.Reverse().ToArray();
        }

        /// <summary>
        /// Inverts the dimensions of the shape.
        /// </summary>
        /// <param name="size">The dimensions of the shape.</param>
        /// <returns>The inverted size object.</returns>
        public static Size Invert(Size size)
        {
            Size s = size.Clone() as Size;
            s.Invert();
            return s;
        }

        /// <summary>
        /// Compares this size with another size.
        /// </summary>
        /// <param name="other">The size to compare.</param>
        /// <returns>Whether the sizes are equal.</returns>
        public bool Equals(Size other)
        {
            return Dimensions.SequenceEqual(other.Dimensions);
        }

        /// <summary>
        /// Creates a shallow copy of the size.
        /// </summary>
        /// <returns>The shallow copy.</returns>
        public object Clone()
        {
            return MemberwiseClone();
        }

        /// <summary>
        /// Compares this size with another size.
        /// </summary>
        /// <param name="other">The size to compare.</param>
        /// <returns>Whether the sizes are equal.</returns>
        public override bool Equals(object other)
        {
            return Equals(other as Size);
        }

        /// <summary>
        /// Converts the size into a string.
        /// </summary>
        /// <returns>The size as a string.</returns>
        public override string ToString()
        {
            return "(" + Dimensions.SubArray(1, Dimensions.Length - 1)
                .Aggregate(Dimensions[0].ToString(), (a, b) => a + " x " + b) + ")";
        }
    }
}
