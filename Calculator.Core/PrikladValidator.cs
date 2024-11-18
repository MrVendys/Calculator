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
            string separator = _counting.DesetinnyOddelovac;
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
        /// Zároveň zkusí zjistit, zda lze symbol logicky zapsat do <see cref="Counting.Priklad"/>. (Víc desetinných čárek za sebou, zavírací závorka dřív, jak otevírací)
        /// </summary>
        /// <returns>Vrací, jestli může být <paramref name="symbol"/> zapsán</returns>
        public bool ValidatePridejSymbol(char symbol)
        {
            if (_symbolValidator.IsMatch(symbol.ToString()))
                return ValidateSymbol(symbol);

            return false;
        }

        /// <summary>
        /// Zkontroluje, jestli <paramref name="novyPriklad"/> obsahuje jen povolené znaky.
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
        /// Zkontroluje, zda se <paramref name="spocitanyPriklad"/> nachází v historii.
        /// </summary>
        public bool ValidateVratPriklad(SpocitanyPriklad spocitanyPriklad)
        {
            return _counting.HistoriePrikladu.Contains(spocitanyPriklad);
        }

        private bool CheckZavorky()
        {
            int pocetOtevrenychZavorek = 0;
            foreach (char s in _counting.Priklad)
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

            return pocetOtevrenychZavorek > 0;
        }

        /// <summary>
        /// Zkontroluje, zda lze <paramref name="symbol"/> zapsat do aktuálního <see cref="Counting.Priklad"/>.
        /// </summary>
        private bool ValidateSymbol(char symbol)
        {
            string priklad = _counting.Priklad;
            char oddelovac = _counting.DesetinnyOddelovac.First();
            IEnumerable<char> operace = _counting.ZnakyOperaci;
            MatchCollection matches = Regex.Matches(priklad, @$"\d+(\{oddelovac}\d+)?");
            string posledniCislo = matches.Count == 0 ? "" : matches.Last().Value;
            char posledniSymbol = string.IsNullOrEmpty(priklad) ? ' ' : priklad.Last();

            // Přidání čísla
            if (char.IsDigit(symbol))
            {
                if (posledniSymbol != ')' && posledniSymbol != '!')
                {
                    return true;
                }

                return false;
            }

            // Přidání desetinné čárky/tečky
            if (symbol == oddelovac)
            {
                if (posledniCislo != "" && !posledniCislo.Contains(oddelovac))
                {
                    return true;
                }

                return false;
            }

            // Přidání symbolu do prázdného příkladu 
            if (string.IsNullOrEmpty(priklad))
            {
                if (symbol == '(' || symbol == '√')
                {
                    return true;
                }

                return false;
            }

            // Přidání závorek
            if (symbol == ')')
            {
                if (posledniSymbol != '(' && !operace.Contains(posledniSymbol))
                {
                    return CheckZavorky();
                }

                return false;
            }

            if (symbol == '(')
            {
                if (!char.IsDigit(posledniSymbol) && posledniSymbol != ')')
                {
                    return true;
                }

                return false;
            }

            // Přidání operátoru
            if (symbol == '√')
            {
                if (!char.IsDigit(posledniSymbol) && posledniSymbol != ')')
                {
                    return true;
                }

                return false;
            }

            // Přidání operátoru
            if (operace.Contains(symbol))
            {
                if (!operace.Contains(posledniSymbol) && posledniSymbol != '(' && symbol != oddelovac)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
