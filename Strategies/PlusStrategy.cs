using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator.Strategies
{
    internal class PlusStrategy : IOperationStrategy
    {
        const int PRIORITY = 1;
        public int Priority => PRIORITY;

        public string[] Count(string[] tokens)
        {
            double result = double.Parse(tokens[0]) + double.Parse(tokens[2]);
            return new string[] { result.ToString() };
        }
    }
}
