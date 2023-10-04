namespace TestSuite
{
    /// <summary>
    /// Unit tests for base class
    /// </summary>
    public class ExMathTestSuite
    {
        [Theory]
        [InlineData(5, 120)]
        [InlineData(0, 1)]
        [InlineData(1, 1)]
        [InlineData(12, 479001600)]
        [InlineData(120, 6.689502913E+198)]
        public void Factorial_ReturnsCorrect(uint @base, double expected)
        {
            Assert.Equal(expected, ExMathLib.Factorial(@base).Truncate(10));
        }

        [Theory]
        [InlineData(3, 2, 3)]
        [InlineData(3, 0, 1)]
        [InlineData(12, 12, 1)]
        [InlineData(43, 2, 903)]
        [InlineData(112, 49, 1.63725E+32)]
        public void Combination_ReturnsCorrect(uint c, uint r, double expected)
        {
            Assert.Equal(expected, ExMathLib.Combination(c, r).Truncate(6));
        }
    }
}
