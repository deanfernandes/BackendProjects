using Calculator.Library;

namespace Calculator.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var c = new Calculator.Library.Calculator();
            Console.WriteLine(c.Add(10, 5));
            Console.WriteLine(c.Subtract(10, 5));
            Console.WriteLine(c.Multiply(10, 5));
            Console.WriteLine(c.Divide(10, 5));
        }
    }
}