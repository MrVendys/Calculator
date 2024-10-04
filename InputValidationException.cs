using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator
{
    internal class InputValidationException : Exception
    {
        /// <summary>
        /// Vrací vyjímku pro špatně zadaný příklad. </para>
        /// Např: Nekompletní závorka, chybějící číslo 
        /// </summary>
        public InputValidationException() { }

        public InputValidationException(string message) : base(message) { }

        public InputValidationException(string message, Exception innerException): base(message, innerException) { }
    }
}
