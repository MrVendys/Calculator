using System.Text.RegularExpressions;

namespace Calculator.Core
{
    /// <summary>
    /// Validace úpravy příkladu. Přidání, odebrání symbolu, vrácení příkladu z historie
    /// </summary>
    internal class PrikladValidator
    {
        private readonly Regex _symbolValidator;
        private readonly Counting _counting;

        public PrikladValidator(Counting counting)
        {
            _counting = counting;
            _symbolValidator = InitializeRegex();
        }

        /// <summary>
        /// Zkontroluje, jestli je symbol povolený v <see cref="_symbolValidator"/>. <br/>
        /// Zároveň zkontroluje, zda lze symbol logicky zapsat do <see cref="Counting.Priklad"/>. (Víc desetinných čárek za sebou, zavírací závorka dřív, jak otevírací)
        /// </summary>
        /// <returns>Vrací, jestli může být symbol zapsán</returns>
        public bool ValidatePridejSymbol(string symbol)
        {
            if (!_symbolValidator.IsMatch(symbol))
                return false;

            string priklad = _counting.Priklad;
            if (symbol == ")")
            {
                int pocetOtevrenychZavorek = 0;
                foreach (char s in priklad)
                {
                    if (s == '(')
                    {
                        pocetOtevrenychZavorek++;
                    }
                    if (s == ')')
                    {
                        pocetOtevrenychZavorek--;
                    }
                }

                if (pocetOtevrenychZavorek <= 0)
                {
                    return false;
                }
            }
            else if (symbol == _counting.DesetinnyOddelovac)
            {
                if (priklad[priklad.Length - 1] == _counting.DesetinnyOddelovac.First())
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Zkontroluje, jestli je symbol nějaký znak.
        /// </summary>
        /// <returns>Vrací, jestli může být symbol odstraněn</returns>
        public bool ValidateSmazSymbol(string? symbol)
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
