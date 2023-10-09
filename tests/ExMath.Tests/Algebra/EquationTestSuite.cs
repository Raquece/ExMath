using ExMath.Algebra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExMath.Tests.Algebra
{
    public class EquationTestSuite
    {
        [Theory]
        [InlineData("x", "3", 3)]
        [InlineData("3", "x", 3)]
        [InlineData("x - 2", "3", 5)]
        [InlineData("3", "x - 2", 5)]
        [InlineData("x - 2", "(3 * (3 - 4) / 9)", 1.6666666)]
        [InlineData("(3 * (3 - 4) / 9)", "x - 2", 1.6666666)]
        [InlineData("(3 * (3 - 4) / 9)", "x * 3 - (12 / 6)", 0.555555)]
        [InlineData("x * 3 - (12 / 6)", "(3 * (3 - 4) / 9)", 0.555555)]
        public void SimpleSolve_SolvesCorrectly(string exp1, string exp2, double expected)
        {
            Expression e1 = Expression.Parse(exp1);
            Expression e2 = Expression.Parse(exp2);
            Equation eq1 = new Equation(e1, e2);

            Assert.Equal(expected, eq1.SimpleSolve(new NumericVariable("x"), null), 0.0001);
        }
    }
}
