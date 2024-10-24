using System.Text.RegularExpressions;

namespace Calculator.Core
{
    /// <summary>
    /// Validace úpravy příkladu. Přidání, odebrání symbolu
    /// </summary>
    internal class PrikladValidator
    {
        private Regex _regex;
        private Counting _counting;

        /// <param name="counting">Výpočetní jádro pro získání znaků používaných operátorů</param>
        internal PrikladValidator(Counting counting)
        {
            _counting = counting;
            _regex = InicializeRegex();
        }

        internal bool ValidateAddSymbol(string symbol)
        {
            return _regex.IsMatch(symbol);
        }

        internal bool ValidateDeleteSymbol(string symbol)
        {
            return !string.IsNullOrWhiteSpace(symbol);
        }

        internal bool ValidateVratPriklad(SpocitanyPriklad sPriklad)
        {
            return _counting.HistoriePrikladu.Contains(sPriklad);
        }

        private Regex InicializeRegex()
        {
            string pattern = "[-0-9()";
            string separator = System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;

            foreach (char znak in _counting.ZnakyOperaci)
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
