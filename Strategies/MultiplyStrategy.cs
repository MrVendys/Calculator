using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator.Strategies
{
    internal class MultiplyStrategy : IOperationStrategy
    {
        const int PRIORITY = 2;
        public int Priority  => PRIORITY; 

        public string[] Count(string[] tokens)
        {
            try
            {

            double result = double.Parse(tokens[0]) * double.Parse(tokens[2]);
            return new string[] { result.ToString() };
            }
            catch (Exception e) { return new string[] { }; }
        }
    }
}
