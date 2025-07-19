namespace FizzBuzzGame
{
    public static class Game
    {
        public static string Play(int number)
        {
            return number switch
            {
                _ when number > 100 => number.ToString(),
                _ when number.IsDivisibleBy(15) => "FizzBuzz",
                _ when number.IsDivisibleBy(3) => "Fizz",
                _ when number.IsDivisibleBy(5) => "Buzz",
                _ => number.ToString()
            };
        }

        private static bool IsDivisibleBy(this int i, int number)
        {
            return i % number == 0;
        }
    }
}