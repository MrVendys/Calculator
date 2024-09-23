using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator.Strategies
{
    internal class PowerStrategy : IOperationStrategy
    {
        private readonly int _priorita = 3;
        public int Priorita { get { return _priorita; } }

        public string[] Vypocitej(string[] tokeny)
        {
            try
            {
            double result = Math.Pow(double.Parse(tokeny[0]), double.Parse(tokeny[2]));
            return new string[] { result.ToString() };

            }
            catch (Exception e) { return new string[] { }; }
        }
    }
}
