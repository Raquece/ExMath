using ExMath.Algebra.Analysis;
using ExMath.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExMath.Algebra
{
    /// <summary>
    /// Represents an unknown, variable numeric type.
    /// </summary>
    public struct NumericVariable : IEquatable<NumericVariable>
    {
        public NumericVariable(string name)
            => (ID) = (name.GetHashCode());

        /// <summary>
        /// The hashcode and identifier of the variable.
        /// </summary>
        public int ID { get; private set; }

        /// <summary>
        /// The internal value of the variable.
        /// </summary>
        private dynamic? _value;

        public static implicit operator NumericVariable(Token token)
        {
            if (token == null)
            {
                throw new ArgumentNullException(nameof(token));
            }

            if (token.LexicalToken != LexicalToken.NUMERIC_UNKNOWN)
            {
                throw new ArgumentException("Token is not an unknown numeric type.", nameof(token));
            }

            if (token.Data == null)
            {
                throw new ArgumentException("Token data is malformed.");
            }

            return new NumericVariable((string)token.Data);
        }

        public static implicit operator NumericVariable(string name)
        {
            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }

            return new NumericVariable(name);
        }

        public static implicit operator NumericVariable(char character)
        {
            return new NumericVariable(character.ToString());
        }

        public static bool operator ==(NumericVariable left, NumericVariable right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(NumericVariable left, NumericVariable right)
        {
            return !(left == right);
        }

        public static dynamic operator +(NumericVariable a, dynamic b)
        {
            if (a._value == null)
            {
                throw new UnknownNumericVariableException("A numeric variable has not been resolved");
            }

            return a._value + b;
        }

        public static dynamic operator +(dynamic a, NumericVariable b)
        {
            if (b._value == null)
            {
                throw new UnknownNumericVariableException("A numeric variable has not been resolved");
            }

            return a + b._value;
        }

        public static dynamic operator +(NumericVariable a, NumericVariable b)
        {
            if (a._value == null || b._value == null)
            {
                throw new UnknownNumericVariableException("A numeric variable has not been resolved");
            }

            return a._value + b._value;
        }

        public static dynamic operator -(NumericVariable a, dynamic b)
        {
            if (a._value == null)
            {
                throw new UnknownNumericVariableException("A numeric variable has not been resolved");
            }

            return a._value - b;
        }

        public static dynamic operator -(dynamic a, NumericVariable b)
        {
            if (b._value == null)
            {
                throw new UnknownNumericVariableException("A numeric variable has not been resolved");
            }

            return a - b._value;
        }

        public static dynamic operator -(NumericVariable a, NumericVariable b)
        {
            if (a._value == null || b._value == null)
            {
                throw new UnknownNumericVariableException("A numeric variable has not been resolved");
            }

            return a._value - b._value;
        }

        public static dynamic operator *(NumericVariable a, dynamic b)
        {
            if (a._value == null)
            {
                throw new UnknownNumericVariableException("A numeric variable has not been resolved");
            }

            return a._value * b;
        }

        public static dynamic operator *(dynamic a, NumericVariable b)
        {
            if (b._value == null)
            {
                throw new UnknownNumericVariableException("A numeric variable has not been resolved");
            }

            return a * b._value;
        }

        public static dynamic operator *(NumericVariable a, NumericVariable b)
        {
            if (a._value == null || b._value == null)
            {
                throw new UnknownNumericVariableException("A numeric variable has not been resolved");
            }

            return a._value * b._value;
        }

        public static dynamic operator /(NumericVariable a, dynamic b)
        {
            if (a._value == null)
            {
                throw new UnknownNumericVariableException("A numeric variable has not been resolved");
            }

            return a._value / b;
        }

        public static dynamic operator /(dynamic a, NumericVariable b)
        {
            if (b._value == null)
            {
                throw new UnknownNumericVariableException("A numeric variable has not been resolved");
            }

            return a / b._value;
        }

        public static dynamic operator /(NumericVariable a, NumericVariable b)
        {
            if (a._value == null || b._value == null)
            {
                throw new UnknownNumericVariableException("A numeric variable has not been resolved");
            }

            return a._value / b._value;
        }

        /// <summary>
        /// Sets this variable to a value.
        /// </summary>
        /// <param name="val">The value to set it to.</param>
        public void Set(dynamic val)
        {
            _value = val;
        }

        /// <summary>
        /// Clears this variable of its value.
        /// </summary>
        public void Clear()
        {
            _value = null;
        }

        /// <summary>
        /// Compares two numerical variables.
        /// </summary>
        /// <param name="other">The variable to comapre to.</param>
        /// <returns>Whether these variables are the same.</returns>
        public readonly bool Equals(NumericVariable other)
        {
            return ID == other.ID
                && _value == other._value;
        }

        /// <summary>
        /// Compares two numerical variables.
        /// </summary>
        /// <param name="obj">The object to comapre to.</param>
        /// <returns>Whether these variables are the same.</returns>

        public override bool Equals(object? obj)
        {
            return obj is NumericVariable variable
                && Equals(variable);
        }

        /// <summary>
        /// Gets the hashcode of the variable.
        /// </summary>
        /// <returns>The hashcode.</returns>
        public override readonly int GetHashCode()
            => ID;
    }
}
