using Calculator.Core.Exceptions;
using Calculator.Core.Strategies;
using System.Collections.ObjectModel;

namespace Calculator.Core
{
    public class Counting
    {
        /// <summary>
        /// Výčet použitelných operací. <br/>
        /// Key: char, znak operátoru. <br/>
        /// Value: OperationStrategy, instance OperationStrategy odpovídajícího znaku.
        /// </summary>
        internal readonly Dictionary<char, OperaceBase> _operace = new Dictionary<char, OperaceBase>();

        private readonly PrikladValidator _prikladValidator;

        #region Inicializace

        public Counting()
        {
            InitializeOperace();
            _prikladValidator = new PrikladValidator(this);
        }

        private void InitializeOperace()
        {
            AddOperace(new OperaceScitani());
            AddOperace(new OperaceOdcitani());
            AddOperace(new OperaceNasobeni());
            AddOperace(new OperaceDeleni());
            AddOperace(new OperaceMocnina());
            AddOperace(new OperaceOdmocnina());
            AddOperace(new OperaceFaktorial());
            AddOperace(new OperaceModulo());
        }

        public void AddOperace(OperaceBase operace)
        {
            if (_operace.TryGetValue(operace.ZnakOperatoru, out _))
                throw new SpatnePouzitiException(
                    ChybovyKodSpatnePouziti.DuplicitniOperace,
                    $"Operace se znakem: {operace.ZnakOperatoru} je již definována. \n" +
                    $"(Již použité znaky: {string.Join(',', _operace.Keys)})"
                    );

            if (char.IsWhiteSpace(operace.ZnakOperatoru))
                throw new SpatnePouzitiException(ChybovyKodSpatnePouziti.ChybiZnak);

            _operace.Add(operace.ZnakOperatoru, operace);
            _prikladValidator?.Refresh();
        }

        #endregion

        public ObservableCollection<SpocitanyPriklad> HistoriePrikladu { get; } = new ObservableCollection<SpocitanyPriklad>();

        public string DesetinnyOddelovac => System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;

        public string Priklad { get; private set; } = "";

        public IEnumerable<char> ZnakyOperaci => _operace.Keys;

        #region Validace

        /// <summary>
        /// Pokusí se přidat symbol do <see cref="Priklad"/>
        /// </summary>
        /// <returns>Vrací bool, jestli byl příklad změněn</returns>
        public bool TryPridejSymbol(string symbol)
        {
            if (symbol.Equals("mod", StringComparison.CurrentCultureIgnoreCase))
                symbol = "%";

            bool valid = _prikladValidator.ValidatePridejSymbol(symbol.First());
            if (valid)
                Priklad += symbol;

            return valid;
        }

        public bool TryPridejPriklad(string novyPriklad)
        {
            bool valid = _prikladValidator.ValidatePridejPriklad(novyPriklad);
            if (valid)
                Priklad = novyPriklad;

            return valid;
        }

        /// <summary>
        /// Pokusí se odebrat poslední symbol z <see cref="Priklad"/>
        /// </summary>
        /// <returns>Vrací bool, jestli byl příklad změněn</returns>
        public bool TrySmazSymbol()
        {
            bool valid = _prikladValidator.ValidateSmazSymbol();
            if (valid)
                Priklad = Priklad.Remove(Priklad.Length - 1);

            return valid;
        }

        /// <summary>
        /// Pokusí se smazat celý <see cref="Priklad"/>
        /// </summary>
        /// <returns>Vrací bool, jestli byl příklad změněn</returns>
        public bool TrySmazPriklad()
        {
            bool valid = _prikladValidator.ValidateSmazPriklad();
            if (valid)
                Priklad = "";

            return valid;
        }

        /// <summary>
        /// Pokusí se uložit <paramref name="spocitanyPriklad"/> do <see cref="Priklad"/>
        /// </summary>
        /// <returns>Vrací bool, jestli byl příklad změněn</returns>
        public bool TryVratPriklad(SpocitanyPriklad spocitanyPriklad)
        {
            bool valid = _prikladValidator.ValidateVratPriklad(spocitanyPriklad);
            if (valid)
                Priklad = spocitanyPriklad.Priklad;

            return valid;
        }

        #endregion

        /// <summary>
        /// Vypočítá <see cref="Priklad"/> a výsledek uloží zpět do <see cref="Priklad"/>
        /// </summary>
        /// <remarks>
        /// Zároveň uloží počítaný příklad s výsledkem do <see cref="HistoriePrikladu"/>
        /// </remarks>
        /// <exception cref="NeplatnyVstupException">Neplatně zadaný příklad</exception>
        public void Vypocitej()
        {
            if (string.IsNullOrEmpty(Priklad))
                return;

            string vysl = Pocitej(DoTokenu(Priklad));
            HistoriePrikladu.Add(new SpocitanyPriklad(Priklad, vysl));
            Priklad = vysl;
        }

        /// <summary>
        /// Přepíše příklad do string pole.
        /// </summary>
        /// <remarks>
        /// Uložení kompletního čísla (vč. negace, desetinných míst)
        /// </remarks>
        private string[] DoTokenu(string priklad)
        {
            List<string> tokeny = new List<string>();
            string cislo = "";
            for (int index = 0; index < priklad.Length; index++)
            {
                char token = priklad[index];
                if (char.IsNumber(token)
                    || token == DesetinnyOddelovac[0]
                    || (token == '-' && !char.IsNumber(priklad[index - 1 < 0 ? 0 : index - 1])))
                {
                    cislo += token;

                    continue;
                }

                if (cislo != "")
                {
                    tokeny.Add(cislo);
                    cislo = "";
                }

                tokeny.Add(token.ToString());
            }

            if (cislo != "")
            {
                tokeny.Add(cislo);
            }

            return tokeny.ToArray();
        }

        /// <summary>
        /// Nalezne závorky, pošle příklad v nich dál na vypočítání a přepíše závorky na jejich výsledek. <br/>
        /// Závorky se řeší postupně zevnitř.
        /// </summary>
        /// <param name="tokeny">Příklad, rozložený funkcí <see cref="DoTokenu(string)"/></param>
        /// <returns>Pole <paramref name="tokeny"/> s vypočítanými závorky</returns>
        private string[] VyresZavorky(string[] tokeny)
        {
            while (tokeny.Contains("(") || tokeny.Contains(")"))
            {
                int oteviraciId = NajdiPosledniZavorku(tokeny);
                int uzaviraciId = NajdiPrvniZavorku(tokeny, oteviraciId);

                string[] zavorkyPriklad = new string[uzaviraciId - oteviraciId - 1];
                Array.Copy(tokeny, oteviraciId + 1, zavorkyPriklad, 0, (uzaviraciId - oteviraciId - 1));

                string zavorkyVysl = Pocitej(zavorkyPriklad);

                tokeny = tokeny.Take(oteviraciId).Concat([zavorkyVysl]).Concat(tokeny.Skip(uzaviraciId + 1)).ToArray();
            }

            return tokeny;
        }

        private string Pocitej(string[] tokeny)
        {
            //Rekurzí se postupně posílají závorky zpět pro výpočet.
            tokeny = VyresZavorky(tokeny);

            List<OperaceBase> pouziteOperace = NajdiOperatory(tokeny);
            int index = 0;
            while (tokeny.Length > 1 && pouziteOperace.Count > 0)
            {
                if (!int.TryParse(tokeny[index], out _))
                {
                    if (tokeny[index] == pouziteOperace[0].ZnakOperatoru.ToString())
                    {
                        OperaceBase pouzitaOperace = pouziteOperace[0];
                        double meziVysl;
                        int takeIndex;
                        int skipIndex;
                        switch (pouzitaOperace.Pozice)
                        {
                            // Index: index operatoru
                            // TakeIndex: Kolik tokenů se má vzít z pole (všechny před mezipříkladem)
                            // SkipIndex: Kolik tokenů se má přeskočit v poli (všechny až za mezipříklad)

                            // Pro operátory typu: !
                            case PoziceCisla.Vlevo:
                                ZkontrolujCisla(tokeny.Take(index + 1).ToArray(), index, pouzitaOperace);
                                meziVysl = pouzitaOperace.Vypocitej(double.Parse(tokeny[index - 1]));
                                takeIndex = index - 1;
                                skipIndex = index + 1;
                                break;

                            // Pro operátory typu: √
                            case PoziceCisla.Vpravo:
                                ZkontrolujCisla(tokeny.Take(index + 2).ToArray(), index, pouzitaOperace);
                                meziVysl = pouzitaOperace.Vypocitej(double.Parse(tokeny[index + 1]));
                                takeIndex = index;
                                skipIndex = index + 1 + 1;
                                break;

                            // Pro operátory typu: +,-,*,/
                            case PoziceCisla.VlevoIVpravo:
                                ZkontrolujCisla(tokeny.Take(index + 2).ToArray(), index, pouzitaOperace);
                                meziVysl = pouzitaOperace.Vypocitej(double.Parse(tokeny[index - 1]), double.Parse(tokeny[index + 1]));
                                takeIndex = index - 1;
                                skipIndex = index + 1 + 1;
                                break;

                            default:
                                throw new NotImplementedException("Tato operace není implementovaná");
                        }

                        // Přepsání pole "tokeny" novými hodnoty. Přepsání mezipříkladu na mezivýsledek. 
                        // Začátek nechá stejný (do takeIndexu). 
                        // Vloží mezivýsledek.
                        // Vloží původní pole. Přitom přeskočí prvních (skipIndex) hodnot.
                        tokeny = tokeny.Take(takeIndex).Concat([meziVysl.ToString()]).Concat(tokeny.Skip(skipIndex)).ToArray();
                        pouziteOperace.RemoveAt(0);
                        index = -1;
                    }
                }

                index++;
            }

            return tokeny[0];
        }

        /// <summary>
        /// Kontrola, zda má operátor vedle sebe číslo, se kterým může počítat.
        /// </summary>
        /// <param name="tokeny">Příklad, rozložený funkcí <see cref="DoTokenu(string)"/></param>
        /// <param name="index">Index řešeného operátoru v <paramref name="tokeny"/></param>
        /// <param name="pouzitaOperace">Řešený operátor jako objekt OperaceStrategy pro získání vlastnosti <see cref="PoziceCisla"/></param>
        private void ZkontrolujCisla(string[] tokeny, int index, OperaceBase pouzitaOperace)
        {
            int indexCisla1;
            int? indexCisla2 = null;
            if (pouzitaOperace.Pozice == PoziceCisla.Vlevo && index - 1 >= 0)
            {
                indexCisla1 = index - 1;
            }
            else if (pouzitaOperace.Pozice == PoziceCisla.Vpravo && index + 1 < tokeny.Length)
            {
                indexCisla1 = index + 1;
            }
            else if (pouzitaOperace.Pozice == PoziceCisla.VlevoIVpravo && index - 1 >= 0 && index + 1 < tokeny.Length)
            {
                indexCisla1 = index - 1;
                indexCisla2 = index + 1;
            }
            else
            {
                throw new NeplatnyVstupException(ChybovyKodNeplatnyVstup.ChybejiciCislo); ;
            }

            if (!Zkontroluj(tokeny[indexCisla1], indexCisla2 == null ? "0" : tokeny[indexCisla2.Value]))
            {
                throw new NeplatnyVstupException(ChybovyKodNeplatnyVstup.NeniCislo, $"Operator: {pouzitaOperace.ZnakOperatoru} nemá číslo pro výpočet");
            }
        }

        private bool Zkontroluj(string symbol1, string symbol2)
        {
            string[] symboly = { symbol1, symbol2 };
            foreach (var symbol in symboly)
            {
                if (!double.TryParse(symbol, out _))
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Nalezení použitých operátorů v příkladu.
        /// </summary>
        /// <param name="tokeny">Příklad, rozložený funkcí <see cref="DoTokenu(string)"/></param>
        /// <returns>List nalezených operací. Uspořádaný sestupně podle vlastnosti <see cref="OperaceBase.Priorita"/></returns>
        private List<OperaceBase> NajdiOperatory(string[] tokeny)
        {
            List<OperaceBase> pouziteOperace = new List<OperaceBase>();
            for (int i = 0; i < tokeny.Length; i++)
            {
                if (!int.TryParse(tokeny[i], out _))
                {
                    if (_operace.TryGetValue(tokeny[i][0], out var operace))
                    {
                        pouziteOperace.Add(operace);
                    }
                }
            }

            return pouziteOperace.OrderByDescending(item => item.Priorita).ToList();
        }

        /// <summary>
        /// Nalezení poslední instance symbolu "(" v poli. <br/>
        /// </summary>
        /// <param name="tokeny">Pole, ve kterém se bude vyhledávat</param>
        /// <returns>Index hledaného symbolu</returns>
        private int NajdiPosledniZavorku(string[] tokeny)
        {
            for (int i = tokeny.Length - 1; i >= 0; i--)
            {
                if (tokeny[i] == "(")
                {
                    return i;
                }
            }

            throw new NeplatnyVstupException(ChybovyKodNeplatnyVstup.ChybiOteviraciZavorka);
        }

        /// <summary>
        /// Nalezení první instance symbolu ")" v poli. <br/>
        /// </summary>
        /// <param name="tokeny">Pole, ve kterém se bude vyhledávat</param>
        /// <param name="startovaciId">Index (otevřené závorky), od kterého se začne vyhledávat</param>
        /// <returns>Index hledaného symbolu</returns>
        private int NajdiPrvniZavorku(string[] tokeny, int startovaciId)
        {
            for (int i = startovaciId; i < tokeny.Length; i++)
            {
                if (tokeny[i] == ")")
                {
                    if (i - startovaciId <= 1)
                    {
                        throw new NeplatnyVstupException(ChybovyKodNeplatnyVstup.PrazdnaZavorka);
                    }

                    return i;
                }
            }

            throw new NeplatnyVstupException(ChybovyKodNeplatnyVstup.ChybiZaviraciZavorka);
        }
    }
}
