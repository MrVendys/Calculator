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

        #region Nastavení proměnných

        public PrikladValidator(Counting counting)
        {
            _counting = counting;
            _symbolValidator = InitializeRegex();
        }

        /// <summary>
        /// Kalkulačka umožňuje přidávat nové operace za chodu. <br/>
        /// Tato metoda se volá při vytvoření nové operace. Aktualizuje <see cref="_symbolValidator"/> pro přidání znaku nového operátoru.
        /// </summary>
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

        #endregion

        /// <summary>
        /// Zkontroluje, jestli je symbol povolený v <see cref="_symbolValidator"/>. <br/>
        /// Zároveň zkontroluje, zda lze symbol logicky zapsat do <see cref="Counting.Priklad"/>. (Víc desetinných čárek za sebou, zavírací závorka dřív, jak otevírací)
        /// </summary>
        /// <returns>Vrací, jestli může být <paramref name="symbol"/> zapsán</returns>
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
                if (priklad.Last() == _counting.DesetinnyOddelovac.First())
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Zkontroluje, jestli <paramref name="novyPriklad"/> obsahuje jen povolené znaky
        /// </summary>
        /// <returns>Vrací, jestli může být <paramref name="novyPriklad"/> zapsán</returns>
        public bool ValidatePridejPriklad(string novyPriklad)
        {
            foreach (char c in novyPriklad)
            {
                if (!_symbolValidator.IsMatch(c.ToString()))
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Zkontroluje, jestli lze smazat poslední znak z <see cref="Counting.Priklad"/>. 
        /// </summary>
        /// <returns>Vrací, jestli může být symbol odstraněn</returns>
        public bool ValidateSmazSymbol()
        {
            return _counting.Priklad != "";
        }

        public bool ValidateSmazPriklad()
        {
            return ValidateSmazSymbol();
        }

        /// <summary>
        /// Zkontroluje, zda se <paramref name="spocitanyPriklad"/> nachází v historii
        /// </summary>
        public bool ValidateVratPriklad(SpocitanyPriklad spocitanyPriklad)
        {
            return _counting.HistoriePrikladu.Contains(spocitanyPriklad);
        }
    }
}
