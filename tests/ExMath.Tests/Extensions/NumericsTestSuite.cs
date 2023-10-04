using ExMath.Extensions;

namespace TestSuite.Extensions
{
    public class NumericsTestSuite
    {
        [Fact]
        public void IsNumber_ValidNumber_ShouldReturnTrue()
        {
            Assert.True(Numerics.IsNumber<byte>());
            Assert.True(Numerics.IsNumber<sbyte>());
            Assert.True(Numerics.IsNumber<int>());
            Assert.True(Numerics.IsNumber<uint>());
            Assert.True(Numerics.IsNumber<long>());
            Assert.True(Numerics.IsNumber<ulong>());
            Assert.True(Numerics.IsNumber<double>());
            Assert.True(Numerics.IsNumber<float>());
            Assert.True(Numerics.IsNumber<decimal>());
            Assert.True(Numerics.IsNumber<short>());
            Assert.True(Numerics.IsNumber<ushort>());
        }

        [Fact]
        public void IsNumber_InvalidNumber_ShouldReturnFalse()
        {
            Assert.False(Numerics.IsNumber<string>());
            Assert.False(Numerics.IsNumber<Random>());
            Assert.False(Numerics.IsNumber<Exception>());
            Assert.False(Numerics.IsNumber<InvalidStruct>());
        }

        [Theory]
        [InlineData(5, 120)]
        [InlineData(0, 1)]
        public void Factorial_ValidData_ReturnsCorrect(int @base, ulong expected)
        {
            Assert.Equal(expected, @base.Factorial());
        }
    }
}
