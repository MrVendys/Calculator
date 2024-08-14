using System;

namespace Calculator.Strategies
{
    internal class FactorialStrategy : IOperationStrategy
    {
        const int PRIORITY = 4;
        public int Priority => PRIORITY;

        public string[] Count(string[] tokens)
        {
            double result = 1;
            for (int i = 1; i <= double.Parse(tokens[0]); i++)
            {
                result = result * i;
            }
            if (tokens.Length == 3)
            {
                return new string[] { result.ToString(), tokens[2] };
            }
            else
            {
                return new string[] { result.ToString() };
            }
            
        }
    }
}
