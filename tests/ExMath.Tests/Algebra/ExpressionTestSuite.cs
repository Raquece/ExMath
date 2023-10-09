using ExMath.Algebra;
using ExMath.Statistics.Testing;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExMath.Tests.Algebra
{
    public class ExpressionTestSuite
    {
        private class EvaluationTesting : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                yield return new object[] { "x + 4 * 3", 14, variables };
                yield return new object[] { "(x + y) * 3", 15, variables };
                yield return new object[] { "x - y * 3 - y + 14", 4, variables };
                yield return new object[] { "(x / (y * 3) - (x + 14)) * z", -678.44444, variables };
            }

            public Dictionary<NumericVariable, dynamic> variables = new Dictionary<NumericVariable, dynamic>()
            {
                { new NumericVariable("x"), 2d },
                { new NumericVariable("y"), 3d },
                { new NumericVariable("z"), 43d },
            };

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }

        [Theory]
        [InlineData("3", 3)]
        [InlineData("2 + 4 * 3", 14)]
        [InlineData("(2 + 3) * 3", 15)]
        [InlineData("2 - 3 * 3 - 3 + 14", 4)]
        [InlineData("(2 / (3 * 3) - (2 + 14)) * 43", -678.44444)]
        public void Parse_Evaluate_ReturnsCorrect(string equation, double expected)
        {
            var e = Expression.Parse(equation);
            var ans = e.Evaluate();
            Assert.Equal((double)expected, (double)ans, 0.00001);
        }

        [Theory]
        [ClassData(typeof(EvaluationTesting))]
        public void ParseWithVariables_EvaluateWithVariableDictionary(string equation, double expected, Dictionary<NumericVariable, dynamic> variables)
        {
            var e = Expression.Parse(equation);
            var ans = e.Evaluate(variables);
            Assert.Equal((double)expected, (double)ans, 0.00001);
        }
    }
}
