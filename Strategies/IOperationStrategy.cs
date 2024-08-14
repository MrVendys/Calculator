using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Animation;

namespace Calculator.Strategies
{
    internal interface IOperationStrategy
    {
        //Priorita operatoru od 1
        //1 == nejmensi -> resi se jako posledni (+,-)
        const int PRIORITY = 0;
        //Počet charakteru, ktery jsou zapotrebi k vypoctu
        public int Priority { get; }
        string[] Count(string[] tokens);
    }
}
