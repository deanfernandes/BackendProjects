using Xunit;
using Calculator.Library;

namespace Calculator.Tests
{
    public class CalculatorTests
    {
        [Xunit.FactAttribute]
        [Xunit.TraitAttribute("Category", "Unit")]
        [Xunit.TraitAttribute("Type", "Add")]
        public void Add_GivenTwoIntegers_ReturnsCorrectSum()
        {
            var c = new Calculator.Library.Calculator();

            int result = c.Add(10, 5);

            Xunit.Assert.Equal(15, result);
        }

        [Xunit.TheoryAttribute]
        [Xunit.InlineData(10, 5, 5)]
        [Xunit.InlineData(100, 50, 50)]
        [Xunit.TraitAttribute("Category", "Unit")]
        [Xunit.TraitAttribute("Type", "Subtract")]
        public void Subtract_GivenTwoIntegers_ReturnsCorrectSum(int a, int b, int expected)
        {
            var c = new Calculator.Library.Calculator();

            int result = c.Subtract(a, b);

            Xunit.Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(10, 5, 50)]
        [InlineData(100, 50, 5000)]
        [Xunit.TraitAttribute("Category", "Unit")]
        [Xunit.TraitAttribute("Type", "Multiply")]
        public void Multiply_ReturnsCorrectProduct(int a, int b, int expected)
        {
            var c = new Calculator.Library.Calculator();

            int result = c.Multiply(a, b);

            Assert.Equal(expected, result);
        }

        [Fact]
        [Xunit.TraitAttribute("Category", "Unit")]
        [Xunit.TraitAttribute("Type", "Divide")]
        public void Divide_ByNonZero_ReturnsQuotient()
        {
            var c = new Calculator.Library.Calculator();

            int result = c.Divide(10, 5);

            Assert.Equal(2, result);
        }

        [Fact]
        [Xunit.TraitAttribute("Category", "Unit")]
        [Xunit.TraitAttribute("Type", "Divide")]
        public void Divide_ByZero_ThrowsDivideByZeroException()
        {
            var c = new Calculator.Library.Calculator();

            Assert.Throws<DivideByZeroException>(() => c.Divide(10, 0));
        }
    }
}