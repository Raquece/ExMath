using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExMath.Calculus.Functions
{
    /// <summary>
    /// Function representing the divide operation.
    /// </summary>
    public class DivideOperator : Function
    {
        /// <summary>
        /// The number of operands in the function.
        /// </summary>
        public override int Operands => 2;

        /// <summary>
        /// Divides the two numbers.
        /// </summary>
        /// <param name="operands">The operands to substitute into the function.</param>
        /// <returns>The evaluated function.</returns>
        protected override dynamic EvaluateFunction(params dynamic[] operands)
        {
            return operands[0] / operands[1];
        }

        /// <summary>
        /// Gets the inverse of the divide function.
        /// </summary>
        /// <returns>The mutiply function.</returns>
        protected override IFunction GetInverse()
        {
            return new MultiplyOperator();
        }
    }
}
