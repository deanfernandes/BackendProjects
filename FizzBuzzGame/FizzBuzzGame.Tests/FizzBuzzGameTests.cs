using Xunit;
using FluentAssertions;
using FizzBuzzGame;

namespace FizzBuzzGame.Tests;

public class FizzBuzzGameTests
{
    /*
    1. Print numbers from 1 up to a given limit (often 100).
    2. For multiples of 3, print "Fizz" instead of the number.
    3. For multiples of 5, print "Buzz" instead of the number.
    4. For numbers that are multiples of both 3 and 5 (i.e., multiples of 15), print "FizzBuzz" instead of the number.
    5. For all other numbers, just print the number itself.
    */

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(4)]
    [InlineData(7)]
    public void Play_GivenNumberNotMultipleOf3Or5_ReturnsNumberAsString(int number)
    {
        string result = Game.Play(number);

        //Assert.Equal(number.ToString(), result);
        result.Should().Be(number.ToString());
    }

    [Theory]
    [InlineData(3)]
    [InlineData(6)]
    [InlineData(9)]
    public void Play_GivenNumberMultipleOf3Only_ReturnWordFizz(int number)
    {
        string result = Game.Play(number);

        //Assert.Equal("Fizz", result);
        result.Should().Be("Fizz");
    }

    [Theory]
    [InlineData(5)]
    [InlineData(10)]
    [InlineData(20)]
    public void Play_GivenNumberMultipleOf5Only_ReturnWordBuzz(int number)
    {
        string result = Game.Play(number);

        //Assert.Equal("Buzz", result);
        result.Should().Be("Buzz");
    }

    [Theory]
    [InlineData(15)]
    [InlineData(30)]
    [InlineData(45)]
    public void Play_GivenNumberMultipleOf3And5_ReturnWordFizzBuzz(int number)
    {
        string result = Game.Play(number);

        //Assert.Equal("FizzBuzz", result);
        result.Should().Be("FizzBuzz");
    }

    [Theory]
    [InlineData(102)]
    [InlineData(110)]
    [InlineData(105)]
    public void Play_GivenNumberAboveLimit100_ReturnsNumberAsString(int number)
    {
        string result = Game.Play(number);

        //Assert.Equal(number.ToString(), result);
        result.Should().Be(number.ToString());
    }
}