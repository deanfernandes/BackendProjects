namespace Calculator.Library
{
    public class MathHelper
    {
        public int[] GetFibonacciSequence(int count)
        {
            if (count <= 0)
                return Array.Empty<int>();

            var sequence = new int[count];

            for (int i = 0; i < count; i++)
            {
                if (i == 0) sequence[i] = 0;
                else if (i == 1) sequence[i] = 1;
                else sequence[i] = sequence[i - 1] + sequence[i - 2];
            }

            return sequence;
        }

        public int[]? GetEvenNumbers(int[]? numbers)
        {
            if (numbers == null)
                return null;

            var evens = numbers.Where(n => n % 2 == 0).ToArray();

            return evens.Length == 0 ? null : evens;
        }
    }
}
