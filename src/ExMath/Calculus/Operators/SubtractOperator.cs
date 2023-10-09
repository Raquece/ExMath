using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExMath.Calculus.Functions
{
    /// <summary>
    /// Function representing the subtract operation.
    /// </summary>
    public class SubtractOperator : Function
    {
        /// <summary>
        /// The number of operands in the function.
        /// </summary>
        public override int Operands => 2;

        /// <summary>
        /// Subtracts the two numbers.
        /// </summary>
        /// <param name="operands">The operands to substitute into the function.</param>
        /// <returns>The evaluated function.</returns>
        protected override dynamic EvaluateFunction(params dynamic[] operands)
        {
            return operands[0] - operands[1];
        }

        /// <summary>
        /// Gets the inverse of the subtract function.
        /// </summary>
        /// <returns>The add function.</returns>
        protected override IFunction GetInverse()
        {
            return new AddOperator();
        }
    }
}
