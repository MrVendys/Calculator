using System.Text.RegularExpressions;

namespace Calculator.Core
{
    /// <summary>
    /// Validace úprav příkladu. Přidání, odebrání symbolu, vrácení příkladu z historie
    /// </summary>
    internal class PrikladValidator
    {
        private Regex _symbolValidator;
        private readonly Counting _counting;

        #region Inicializace

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
            _symbolValidator = InitializeRegex();
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
        /// <returns>Vrací true, jestli může být <paramref name="novyPriklad"/> zapsán. False, pokud nastala v nějakém symbolu chyba.</returns>
        public bool ValidatePridejPriklad(string novyPriklad)
        {
            for (int i = 0; i < novyPriklad.Length; i++)
            {
                if (_symbolValidator.IsMatch(novyPriklad[i].ToString()))
                {
                    if (ValidateSymbol(novyPriklad[i], novyPriklad.Substring(0, i)))
                    {
                        continue;
                    }
                }

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

        private bool GetPocetOtevrenychZavorek(string priklad)
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

            return pocetOtevrenychZavorek > 0;
        }

        /// <summary>
        /// Zkontroluje, zda lze <paramref name="symbol"/> zapsat do aktuálního <see cref="Counting.Priklad"/>.
        /// </summary>
        private bool ValidateSymbol(char symbol, string? novyPriklad = null)
        {
            string priklad = novyPriklad ?? _counting.Priklad;
            char oddelovac = _counting.DesetinnyOddelovac[0];
            char posledniSymbol = string.IsNullOrEmpty(priklad) ? ' ' : priklad.Last();

            MatchCollection celaCisla = Regex.Matches(priklad, @$"\d+(\{oddelovac}\d+)?");
            string posledniCislo = celaCisla.Count == 0 ? "" : celaCisla.Last().Value;

            IEnumerable<char> operace = _counting.ZnakyOperaci;
            PoziceCisla? symbolPoziceCisla = GetPoziceCisla(symbol);
            PoziceCisla? posledniSymbolPoziceCisla = GetPoziceCisla(posledniSymbol);

            // Přidání čísla
            if (char.IsDigit(symbol))
            {
                if (posledniSymbol != ')' && posledniSymbolPoziceCisla != PoziceCisla.Vlevo)
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
                if (symbol == '(' || symbolPoziceCisla == PoziceCisla.Vpravo || symbol == '-')
                {
                    return true;
                }

                return false;
            }

            // Přidání závorek
            if (symbol == ')')
            {
                if (posledniSymbol != '(' || posledniSymbolPoziceCisla == PoziceCisla.Vlevo)
                {
                    return GetPocetOtevrenychZavorek(priklad);
                }

                return false;
            }

            if (symbol == '(')
            {
                if (!char.IsDigit(posledniSymbol) && posledniSymbol != ')' && posledniSymbolPoziceCisla != PoziceCisla.Vlevo)
                {
                    return true;
                }

                return false;
            }

            // Přidávání stejného operátoru
            if (symbol == posledniSymbol)
            {
                return false;
            }

            // Přidání operátoru typu √
            if (symbolPoziceCisla == PoziceCisla.Vpravo)
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
                if ((!operace.Contains(posledniSymbol) && posledniSymbol != '(' && symbol != oddelovac) || posledniSymbolPoziceCisla == PoziceCisla.Vlevo)
                {
                    return true;
                }

                if (symbol == '-' && (posledniSymbol == '(' || posledniSymbolPoziceCisla == PoziceCisla.Vpravo))
                {
                    return true;
                }

                return false;
            }

            return false;
        }

        private PoziceCisla? GetPoziceCisla(char symbol)
        {
            if (_counting._operace.TryGetValue(symbol, out var symbolOperace))
                return symbolOperace.Pozice;
            return null;
        }
    }
}
