using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator.Strategies
{
    internal class DivideStrategy : IOperationStrategy
    {
        public int Priorita { get { return 2; } }

        public string[] Vypocitej(string[] tokeny)
        {
            try{
                double result = double.Parse(tokeny[0]) / double.Parse(tokeny[2]);
                return new string[] { result.ToString() };
            }catch(Exception e) { return new string[] { }; }
           
        }
    }
}
