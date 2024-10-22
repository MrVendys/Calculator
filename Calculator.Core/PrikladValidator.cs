using System.Text.RegularExpressions;

namespace Calculator.Core
{
    internal class PrikladValidator
    {
        private Regex _regex;
        private List<char> _znakyOperaci;
        public PrikladValidator(List<char> znakyOperaci)
        {
            _znakyOperaci = znakyOperaci;
            _regex = InicializeRegex();
        }

        public bool ValidateAddSymbol(string symbol)
        {
            return _regex.IsMatch(symbol) ? true : false;
        }

        public bool ValidateDeleteSymbol(string symbol)
        {
            return string.IsNullOrWhiteSpace(symbol) ? false : true;
        }

        private Regex InicializeRegex()
        {
            string pattern = "[-0-9()";
            string separator = System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;

            foreach (char znak in _znakyOperaci)
            {
                if (znak == '-')
                {
                    continue;
                }
                else
                {
                    pattern += znak.ToString();
                }
            }
            pattern += "]";

            return new Regex(pattern);
        }
    }
}
