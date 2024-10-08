using Calculator.Strategies;

namespace Calculator
{
    public class Counting
    {
        /// <summary>
        /// Výčet použitelných operací.
        /// Key: string, znak operátoru.
        /// Value: OperationStrategy, instance OperationStrategy odpovídajícího znaku.
        /// </summary>
        private Dictionary<string, OperationStrategyBase> _operace = new Dictionary<string, OperationStrategyBase>();

        public Counting() 
        { 
            AddOperace(new PlusStrategy());
            AddOperace(new MinusStrategy());
            AddOperace(new MultiplyStrategy());
            AddOperace(new DivideStrategy());
            AddOperace(new PowerStrategy());
            AddOperace(new SquareRootStrategy());
            AddOperace(new FactorialStrategy());
        }

        private void AddOperace(OperationStrategyBase operace)
        {
            _operace.Add(operace.ZnakOperatoru.ToString(), operace);
        }
            
        /// <summary>
        /// Volaná funkce z <see cref="MainWindow.SubmitButton_Click(object, System.Windows.RoutedEventArgs)"/>
        /// </summary>
        /// <param name="priklad"></param>
        /// <exception cref="InputValidationException">Nerozumě zadaný příklad</exception>
        /// Ukázat funkce, kde je volám?
        /// <returns></returns>
        public string? Pocitej(string priklad)
        {
            return Vyhodnot(DoTokenu(priklad));
        }

        /// <summary>
        /// Přepsání příkladu do pole stringů.
        /// Vyřešení problémů (mezera, prázdný string, desetiné číslo).
        /// </summary>
        /// <param name="priklad"></param>
        /// <returns>Rozsekaný příklad do pole</returns>
        private string[] DoTokenu(string priklad)
        {
            List<string> tokeny = new List<string>();
            int index = 0;
            string cislo = "";
            while (index < priklad.Length)
            {
                string token = priklad.Substring(index, 1);
                if (int.TryParse(token, out _) || token == ",")
                {
                    cislo += token;
                }
                else if (token != " ")
                {
                    if (cislo != "")
                    {
                        tokeny.Add(cislo);
                        cislo = "";
                    }
                    tokeny.Add(token);
                }
                index++;
            }

            if(cislo != "")
                tokeny.Add(cislo);

            return tokeny.ToArray();
        }

        /// <summary>
        /// Vypočítání závorky a uložení mezivýsledku do pole.
        /// </summary>
        /// <param name="tokeny"></param>
        /// <returns>Upravené pole s vypočítanými závorky</returns>
        private string[] NajdiZavorky(string[] tokeny)
        {
            while (tokeny.Contains("(") || tokeny.Contains(")"))
            {
                int oteviraciId = NajdiPosledni("(", tokeny);

                int uzaviraciId = NajdiPrvni(")", tokeny, oteviraciId);

                string[] zavorkyTkny = new string[uzaviraciId - oteviraciId - 1];
                Array.Copy(tokeny, oteviraciId + 1, zavorkyTkny, 0, (uzaviraciId - oteviraciId - 1));

                //Rekurze pro výpočet části příkladu v závorce
                var zavorkyVysl = Vyhodnot(zavorkyTkny);

                tokeny = tokeny.Take(oteviraciId).Concat(new string[] { zavorkyVysl.ToString() }).Concat(tokeny.Skip(uzaviraciId + 1)).ToArray();
            }

            return tokeny;
        }

        /// <summary>
        /// Výpočet celého/daného kusu příkladu.
        /// </summary>
        /// <param name="tokeny">Část příkladu pro výpočet</param>
        /// <returns>Vypočítaná část příkladu</returns>
        private string Vyhodnot(string[] tokeny)
        {
            //První fáze = vypočítání závorek
            tokeny = NajdiZavorky(tokeny); ;

            //Druhá fáze funkce = Postupný výpočet všech tokenů v poli "tokeny".
            //Počítá se podle operací
            List<OperationStrategyBase> pouziteOperace = NajdiOperatory(tokeny);
            int index = 0;
            while (tokeny.Length > 1 && pouziteOperace.Count > 0)
            {
                if (!int.TryParse(tokeny[index], out _))
                {
                    if (tokeny[index] == pouziteOperace[0].ZnakOperatoru.ToString())
                    {
                        OperationStrategyBase pouzitaOperace = pouziteOperace[0];
                        double meziVysl;
                        int takeIndex = 0;
                        int skipIndex = 0;
                        switch (pouzitaOperace.Pozice)
                        {
                            // Index: index operatoru
                            // TakeIndex: Kolik tokenů se má vzít z pole (všechny před mezipříkladem)
                            // SkipIndex: Kolik tokenů se má přeskočit v poli (všechny až za mezipříklad)
                            
                            //TODO: Duplicitní kod.. nějak upravit

                            // Pro operátory typu: !
                            case PoziceCisla.Vlevo:
                                ZkontrolujCisla(tokeny.Take(index + 1).ToArray(), index, pouzitaOperace);
                                meziVysl = pouzitaOperace.Vypocitej(double.Parse(tokeny[index - 1]), null);
                                takeIndex = index - 1;
                                skipIndex = index + 1;
                                break;

                            // Pro operátory typu: √
                            case PoziceCisla.Vpravo:
                                ZkontrolujCisla(tokeny.Take(index + 2).ToArray(), index, pouzitaOperace);
                                meziVysl = pouzitaOperace.Vypocitej(double.Parse(tokeny[index + 1]), null);
                                takeIndex = index;
                                skipIndex = index + 1 + 1;
                                break;

                            // Pro operátory typu: +,-,*,:
                            case PoziceCisla.VlevoIVpravo:
                                ZkontrolujCisla(tokeny.Take(index + 2).ToArray(), index, pouzitaOperace);
                                meziVysl = pouzitaOperace.Vypocitej(double.Parse(tokeny[index - 1]), double.Parse(tokeny[index + 1]));
                                takeIndex = index - 1;
                                skipIndex = index + 1 + 1;
                                break;

                            default:
                                meziVysl = 0;
                                break;
                        }

                        // Přepsání pole "tokeny" novými hodnotami. Přepsání mezipříkladu na mezivýsledek. 
                        // Začátek nechá stejný (do takeIndexu). 
                        // Vloží mezivýsledek.
                        // Vloží konec z původního pole. Přitom přeskočí prvních (skipIndex) hodnot.
                        tokeny = tokeny.Take(takeIndex).Concat(new string[] { meziVysl.ToString() }).Concat(tokeny.Skip(skipIndex)).ToArray();
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
        /// <param name="tokeny"></param>
        /// <param name="index">Index z <paramref name="tokeny"/> řešeného operátoru</param>
        /// <param name="pouzitaOperace">Objekt operace řešeného operátoru</param>
        private void ZkontrolujCisla(string[] tokeny, int index, OperationStrategyBase pouzitaOperace)
        {
            int indexCisla1;
            int indexCisla2;
            if (pouzitaOperace.Pozice == PoziceCisla.Vlevo && index - 1 >= 0)
            {
                indexCisla1 = index - 1;
            }
            else if (pouzitaOperace.Pozice == PoziceCisla.Vpravo && index + 1 < tokeny.Length)
            {
                indexCisla1 = index + 1;
            }
            else if (pouzitaOperace.Pozice == PoziceCisla.VlevoIVpravo && tokeny.Length > 2)
            {
                indexCisla1 = index - 1;
                indexCisla2 = index + 1;
            }
            else
            {
                throw new InputValidationException("Moc krátký příklad pro výpočet.");
            }

            if (!Zkontroluj(tokeny[indexCisla1]))
            {
                throw new InputValidationException($"Chybí číslo pro výpočet u operátoru: {pouzitaOperace.ZnakOperatoru}");
            }
        }

        /// <summary>
        /// Kontrola znaku vedle operátoru, zda je to číslo
        /// </summary>
        /// <param name="tokeny"></param>
        /// <returns>True: Všechno je v pořádku. False: Našla se chyba</returns>
        private bool Zkontroluj(string cislo1, string cislo2 = "0")
        {
            string[] cisla = { cislo1, cislo2 };
            foreach (var cislo in cisla) 
            {
                if (!double.TryParse(cislo, out _))
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Nalezení použitých operátorů v části příkladu.
        /// </summary>
        /// <param name="tokeny">Část příkladu</param>
        /// <returns> List <OperationStrategy>: objekt naleznuté operace.
        /// Uspořádaný sestupně podle vlastnosti Priorita</returns>
        private List<OperationStrategyBase> NajdiOperatory(string[] tokeny)
        {
            List<OperationStrategyBase> pouziteOperace = new List<OperationStrategyBase>();
            for (int i = 0; i < tokeny.Length; i++)
            {
                if (!int.TryParse(tokeny[i], out _))
                {
                    if (_operace.Keys.Contains(tokeny[i]))
                    {
                        pouziteOperace.Add(_operace[tokeny[i]]);
                    }
                }
            }

            return pouziteOperace.OrderByDescending(item => item.Priorita).ToList();
        }

        /// <summary>
        /// Nalezení první instance symbolu v poli odzadu.
        /// Používá se pro nalezení začátku závorky.
        /// </summary>
        /// <param name="symbol">Charakter, který se bude hledat</param>
        /// <param name="tokeny">Pole, ve kterém se bude vyhledávat</param>
        /// <returns>Index hledaného symbolu</returns>
        private int NajdiPosledni(string symbol, string[] tokeny)
        {
            for (int i = tokeny.Length - 1; i >= 0; i--)
            {
                if (tokeny[i] == symbol)
                {
                    return i;
                }
            }

            throw new InputValidationException("Neplatná závorka");
        }

        /// <summary>
        /// Nalezení první instance symbolu v poli zepředu.
        /// Používá se pro nalezení konce závorky.
        /// </summary>
        /// <param name="symbol">Charakter, který se bude hledat</param>
        /// <param name="tokeny">Pole, ve kterém se bude vyhledávat</param>
        /// <param name="startovaciId">Index otevřené závorky, od kterého se začne vyhledávat</param>
        /// <returns>Index hledaného symbolu</returns>
        private int NajdiPrvni(string symbol, string[] tokeny, int startovaciId)
        {
            for (int i = startovaciId; i < tokeny.Length; i++)
            {
                if (tokeny[i] == symbol)
                {
                    if (startovaciId - i < 1)
                    {
                        throw new InputValidationException("Prázdná závorka");
                    }
                    return i;
                }
            }
            
            throw new InputValidationException("Neplatná závorka");
        }
    }
}
