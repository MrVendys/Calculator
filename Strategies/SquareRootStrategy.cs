﻿namespace Calculator.Strategies
{
    internal class SquareRootStrategy : IOperationStrategy
    {
        const int PRIORITY = 4;
        public int Priority => PRIORITY; 

        public string[] Count(string[] tokens)
        {
            try
            {

            if(tokens[1].Equals("√"))
            {
                double result = Math.Sqrt(double.Parse(tokens[2]));
                return new string[] { tokens[0], result.ToString() };
            }
            else
            {
                double result = Math.Sqrt(double.Parse(tokens[1]));
                return new string[] { result.ToString() };
            }
            }
            catch (Exception e) { return new string[] { }; }

        }
    }
}