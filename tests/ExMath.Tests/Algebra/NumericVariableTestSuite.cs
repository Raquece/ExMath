using ExMath.Algebra;

namespace ExMath.Tests.Algebra
{
    public class NumericVariableTestSuite
    {
        [Fact]
        public void AddOperator_UnknownVariable_Throws()
        {
            NumericVariable a = "x";
            NumericVariable b = "y";
            Assert.ThrowsAny<UnknownNumericVariableException>(() => a + 5);
            Assert.ThrowsAny<UnknownNumericVariableException>(() => 5 + a);
            Assert.ThrowsAny<UnknownNumericVariableException>(() => a + b);
        }

        [Fact]
        public void AddOperator_SetVariable_ReturnsCorrect()
        {
            NumericVariable a = "x";
            NumericVariable b = "y";
            a.Set(4);
            b.Set(5);
            Assert.Equal(9, a + 5);
            Assert.Equal(9, 5 + a);
            Assert.Equal(9, a + b);
        }

        [Fact]
        public void MinusOperator_UnknownVariable_Throws()
        {
            NumericVariable a = "x";
            NumericVariable b = "y";
            Assert.ThrowsAny<UnknownNumericVariableException>(() => a - 5);
            Assert.ThrowsAny<UnknownNumericVariableException>(() => 5 - a);
            Assert.ThrowsAny<UnknownNumericVariableException>(() => a - b);
        }

        [Fact]
        public void MinusOperator_SetVariable_ReturnsCorrect()
        {
            NumericVariable a = "x";
            NumericVariable b = "y";
            a.Set(4);
            b.Set(5);
            Assert.Equal(-1, a - 5);
            Assert.Equal(1, 5 - a);
            Assert.Equal(-1, a - b);
        }

        [Fact]
        public void DivisionOperator_UnknownVariable_Throws()
        {
            NumericVariable a = "x";
            NumericVariable b = "y";
            Assert.ThrowsAny<UnknownNumericVariableException>(() => a / 5);
            Assert.ThrowsAny<UnknownNumericVariableException>(() => 5 / a);
            Assert.ThrowsAny<UnknownNumericVariableException>(() => a / b);
        }

        [Fact]
        public void DivisionOperator_SetVariable_ReturnsCorrect()
        {
            NumericVariable a = "x";
            NumericVariable b = "y";
            a.Set(4);
            b.Set(5);
            Assert.Equal(4 / 5, a / 5);
            Assert.Equal(5 / 4, 5 / a);
            Assert.Equal(4 / 5, a / b);
        }

        [Fact]
        public void MultiplyOperator_UnknownVariable_Throws()
        {
            NumericVariable a = "x";
            NumericVariable b = "y";
            Assert.ThrowsAny<UnknownNumericVariableException>(() => a * 5);
            Assert.ThrowsAny<UnknownNumericVariableException>(() => 5 * a);
            Assert.ThrowsAny<UnknownNumericVariableException>(() => a * b);
        }

        [Fact]
        public void MultiplyOperator_SetVariable_ReturnsCorrect()
        {
            NumericVariable a = "x";
            NumericVariable b = "y";
            a.Set(4);
            b.Set(5);
            Assert.Equal(20, a * 5);
            Assert.Equal(20, 5 * a);
            Assert.Equal(20, a * b);
        }
    }
}
