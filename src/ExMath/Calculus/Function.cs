using ExMath.Algebra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExMath.Calculus
{
    /// <summary>
    /// Represents a function.
    /// </summary>
    public abstract class Function : IFunction
    {
        /// <summary>
        /// The number of operands in the function.
        /// </summary>
        public abstract int Operands { get; }

        int IFunction.Operands => throw new NotImplementedException();

        IFunction IFunction.InverseFunction
        {
            get
            {
                _inverse ??= GetInverse();
                return _inverse;
            }
            set
            {
                _inverse = value;
            }
        }

        private IFunction? _inverse;

        /// <summary>
        /// Evaluates the function substituting in operands as the parameters.
        /// </summary>
        /// <param name="operands">The operands to substitute into the function.</param>
        /// <returns>The evaluated function.</returns>
        public dynamic Evaluate(params dynamic[] operands)
        {
            return Evaluate(null, operands);
        }

        public dynamic Evaluate(Dictionary<NumericVariable, dynamic>? variables, params dynamic[] operands)
        {
            if (operands.Length != Operands)
            {
                throw new ArgumentException($"Number of operands must be equal to {Operands}");
            }

            if (variables != null)
            {
                for (int i = 0; i < operands.Length; i++)
                {
                    if (operands[i] is NumericVariable v)
                    {
                        operands[i].Set(variables[v]);
                    }
                }
            }

            return EvaluateFunction(operands);
        }

        /// <summary>
        /// Performs the calculation required to evaluate the function.
        /// </summary>
        /// <param name="operands">The operands to substitute into the function.</param>
        /// <returns>The evaluated function.</returns>
        protected abstract dynamic EvaluateFunction(params dynamic[] operands);

        /// <summary>
        /// Gets the inverse of this function.
        /// </summary>
        /// <returns>The function representing the inverse of this function.</returns>
        protected abstract IFunction GetInverse();
    }
}
