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
        /// Priorita operatoru od 1
        /// 1 == nejmensi -> resi se jako posledni (příklad: +,-)
        /// Použití v Counting.cs pro výpočet 
        /// </summary>
        public int Priorita { get; }

        /// <summary>
        /// Výpočet konkrétní části příkladu
        /// </summary>
        /// <param name="tokeny">Tokeny počítaného příkladu</param>
        /// <returns>Vypočítaná hodnota v string poli</returns>
        string[] Vypocitej(string[] tokeny);
    }
}
