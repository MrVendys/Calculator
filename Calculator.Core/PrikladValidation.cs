using System.Text.RegularExpressions;

namespace Calculator.Core
{
    //Udělat internal, používat z Counting.cs
    public class PrikladValidation
    {
        Counting _counting;
        Regex _regex;
        public PrikladValidation(Counting counting)
        {
            _counting = counting;
            _regex = InicializeRegex();
        }

        private Regex InicializeRegex()
        {
            string pattern = "[-0-9()";
            string separator = System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;
            pattern += pattern.Contains(separator) ? null : separator;

            foreach (char znak in _counting.ZnakyOperaci)
            {
                if (znak == '-')
                    continue;
                else
                    pattern += znak.ToString();
            }
            pattern += "]";

            return new Regex(pattern);
        }

        public bool TryPridejSymbol(string symbol)
        {
            return _regex.IsMatch(symbol) ? true : false;
        }
        public bool TrySmazSymbol(string symbol) 
        {
            return string.IsNullOrWhiteSpace(symbol) ? false : true;
        }
    }
}
