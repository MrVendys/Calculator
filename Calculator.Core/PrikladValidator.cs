using System.Text.RegularExpressions;

namespace Calculator.Core
{
    /// <summary>
    /// Validace úpravy příkladu. Přidání, odebrání symbolu, vrácení příkladu z historie
    /// </summary>
    internal class PrikladValidator
    {
        private readonly Regex _regex;
        private readonly Counting _counting;

        public PrikladValidator(Counting counting)
        {
            _counting = counting;
            _regex = InitializeRegex();
        }

        /// <summary>
        /// Zkontroluje, jestli je symbol povolený v <see cref="_regex"/>.
        /// </summary>
        /// <returns>Vrací, jestli může být symbol zapsán</returns>
        public bool ValidatePridejSymbol(string symbol)
        {
            return _regex.IsMatch(symbol);
        }

        /// <summary>
        /// Zkontroluje, jestli je symbol nějaký znak.
        /// </summary>
        /// <returns>Vrací, jestli může být symbol odstraněn</returns>
        public bool ValidateSmazSymbol(string symbol)
        {
            return !string.IsNullOrWhiteSpace(symbol);
        }

        /// <summary>
        /// Zkontroluje, zda se <paramref name="spocitanyPriklad"/> nachází v historii
        /// </summary>
        public bool ValidateVratPriklad(SpocitanyPriklad spocitanyPriklad)
        {
            return _counting.HistoriePrikladu.Contains(spocitanyPriklad);
        }

        public void Refresh()
        {
            InitializeRegex();
        }

        private Regex InitializeRegex()
        {
            string separator = System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;
            string pattern = "[-0-9()" + Regex.Escape(separator);

            foreach (char znak in _counting.ZnakyOperaci)
            {
                if (znak == '-')
                {
                    continue;
                }
                else
                {
                    pattern += Regex.Escape(znak.ToString());
                }
            }
            pattern += "]";

            return new Regex(pattern);
        }
    }
}
