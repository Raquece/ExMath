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
    public interface IFunction
    {
        /// <summary>
        /// The number of operands in the function.
        /// </summary>
        int Operands { get; }

        public IFunction InverseFunction { get; protected set; }

        /// <summary>
        /// Evaluates the function substituting in operands as the parameters.
        /// </summary>
        /// <param name="operands">The operands to substitute into the function.</param>
        /// <returns>The evaluated function.</returns>
        public dynamic Evaluate(params dynamic[] operands);

        /// <summary>
        /// Evaluates the function substituting in operands as the parameters.
        /// </summary>
        /// <param name="variables">The values for dynamic variables (if present).</param>
        /// <param name="operands">The operands to substitute into the function.</param>
        /// <returns>The evaluated function.</returns>
        public dynamic Evaluate(Dictionary<NumericVariable, dynamic>? variables, params dynamic[] operands);

    }
}
