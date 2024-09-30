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
        /// <summary>
        /// Priority výpočtu
        /// </summary>
        public int Priorita { get; }

        /// <summary>
        /// Výpočet konkrétní části příkladu
        /// </summary>
        /// <param name="tokeny">Počítaný příklad</param>
        /// <returns>Pole s vypočítanou hodnotou</returns>
        string[] Vypocitej(string[] tokeny);
    }
}
