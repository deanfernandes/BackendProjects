using Xunit;
using Calculator.Library;
using FluentAssertions;

namespace Calculator.Tests
{
    public class MathHelperTests
    {
        private readonly MathHelper _mathHelper = new MathHelper();

        [Fact]
        public void GetFibonacciSequence_WithCount_ReturnsCorrectSequence()
        {
            int[] result = _mathHelper.GetFibonacciSequence(5);

            //Assert.Equal(new[] { 0, 1, 1, 2, 3 }, result);
            result.Should().Equal(new[] { 0, 1, 1, 2, 3 });
        }

        [Fact]
        public void GetFibonacciSequence_WithZeroCount_ReturnsEmpty()
        {
            int[] result = _mathHelper.GetFibonacciSequence(0);

            //Assert.Empty(result);
            result.Should().BeEmpty();
        }

        [Fact]
        public void GetEvenNumbers_WithNullInput_ReturnsNull()
        {
            var result = _mathHelper.GetEvenNumbers(null);

            //Assert.Null(result);
            result.Should().BeNull();
        }
    }
}