using ExMath.Calculus.Functions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExMath.Calculus.Operators
{
    /// <summary>
    /// Function representing the power operation.
    /// </summary>
    public class PowerOperator : Function
    {
        /// <summary>
        /// The number of operands in the function.
        /// </summary>
        public override int Operands => 2;

        /// <summary>
        /// Calculates the power of the first number to the second.
        /// </summary>
        /// <param name="operands">The operands to substitute into the function.</param>
        /// <returns>The evaluated function.</returns>
        protected override dynamic EvaluateFunction(params dynamic[] operands)
        {
            return operands[0] ^ operands[1];
        }

        /// <summary>
        /// Gets the inverse of the power function.
        /// </summary>
        /// <returns>The root function.</returns>
        protected override IFunction GetInverse()
        {
            throw new NotImplementedException();
        }
    }
}
