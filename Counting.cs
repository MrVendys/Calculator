using Calculator.Strategies;
using System.Linq;
using System.Xml.XPath;

namespace Calculator
{
    public class Counting
    {
        /// <summary>
        /// Výčet použitelných operací
        /// </summary>
        private List<(string Operace, OperationStrategyBase Strategie)> _operaceList = new List<(string Operace, OperationStrategyBase Strategie)>
            {
            ( "+",new PlusStrategy() ),
            ( "-",new MinusStrategy() ),
            ( "*",new MultiplyStrategy() ),
            ( "/",new DivideStrategy() ),
            ( "^",new PowerStrategy() ),
            ( "√",new SquareRootStrategy() ),
            ( "!", new FactorialStrategy() )
            };

        /// <summary>
        /// Kontrola chyby pro typ null
        /// </summary>
        private bool _chyba = false;

        public Counting() {
        }

        public double Pocitej(string priklad)
        {
            return Vyhodnot(DoTokenu(priklad));
        }

        /// <summary>
        /// Přepsání příkladu do pole stringů (
        /// Vyřešení problémů (mezera, prázdný string, desetiné číslo)
        /// </summary>
        /// <param name="priklad">Zadaný příklad</param>
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
        /// Výpočet celého/daného kusu příkladu
        /// </summary>
        /// <param name="tokeny">Část příkladu pro výpočet</param>
        /// <returns>Vypočítaná část příkladu</returns>
        private double Vyhodnot(string[] tokeny)
        {
            //První fáze = naleznutí závorek, rekurze na výpočet příkladu v ní, a nahrazení závorek mezivýsledkem do pole "tokeny"
            while (tokeny.Contains("("))
            {
                int oteviraciId = NajdiPosledni("(", tokeny);
                int uzaviraciId = NajdiPrvni(")", tokeny, oteviraciId);

                string[] zavorkyTkny = new string[uzaviraciId - oteviraciId - 1];
                Array.Copy(tokeny, oteviraciId + 1, zavorkyTkny, 0, (uzaviraciId - oteviraciId - 1));

                //Rekurze pro výpočet části příkladu v závorce
                var zavorkyVysl = Vyhodnot(zavorkyTkny);

                tokeny = tokeny.Take(oteviraciId).Concat(new string[] { zavorkyVysl.ToString() }).Concat(tokeny.Skip(uzaviraciId + 1)).ToArray();
            }

            //Druhá fáze funkce = projetí všech symbolů v poli "tokeny" a postupný výpočet.
            //Počítá se podle operací
            List<(string Operation, OperationStrategyBase Strategy)> pouziteOperace = NajdiOperatory(tokeny);
            int index = 0;
            while (tokeny.Length > 1)
            {
                if (!int.TryParse(tokeny[index], out _))
                {
                    if (tokeny[index] == pouziteOperace[0].Operation)
                    {
                        OperationStrategyBase pouzitaOperace = pouziteOperace[0].Strategy;
                        double meziVysl;
                        int takeIndex = 0;
                        int skipIndex = 0;
                        switch (pouzitaOperace.Pozice)
                        {
                            // Index: index operatoru
                            // TakeIndex: Kolik tokenů se má vzít z pole (všechny před mezipříkladem)
                            // SkipIndex: Kolik tokenů se má přeskočit v poli (všechny až za mezipříklad)
                            
                            // Pro operátory typu: !
                            case PoziceCisla.Vlevo:
                                meziVysl = pouzitaOperace.Vypocitej(double.Parse(tokeny[index - 1]),null);
                                takeIndex = index - 1;
                                skipIndex = index + 1;
                                break;

                            // Pro operátory typu: √
                            case PoziceCisla.Vpravo:
                                meziVysl = pouzitaOperace.Vypocitej(double.Parse(tokeny[index + 1]), null);
                                takeIndex = index;
                                skipIndex = index + 1 + 1;
                                break;

                            // Pro operátory typu: +,-,*,:
                            case PoziceCisla.VlevoIVpravo:
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
            return double.Parse(tokeny[0]);
        }

        /// <summary>
        /// Nalezení použitých operátorů v části příkladu
        /// </summary>
        /// <param name="tokeny">Část příkladu</param>
        /// <returns> List Touple(string: roperátor jako řetězec, IOperationStrategy: objekt operátoru) 
        /// naleznutých operací, uspořádaný sestupně podle vlastnosti Priorita</returns>
        private List<(string, OperationStrategyBase)> NajdiOperatory(string[] tokeny)
        {
            var pouziteOperace = new List<(string Operation, OperationStrategyBase Strategie)>();
            for (int i = 0; i < tokeny.Length; i++)
            {
                for (int j = 0; j < _operaceList.Count; j++)
                {
                    if (!int.TryParse(tokeny[i], out _))
                    {
                        if (tokeny[i].Equals(_operaceList[j].Operace))
                        {
                            pouziteOperace.Add((_operaceList[j].Operace, _operaceList[j].Strategie));
                        }
                    }
                }
            }

            return pouziteOperace.OrderByDescending(item => item.Strategie.Priorita).ToList();
        }

        /// <summary>
        /// Nalezení první instance symbolu v poli odzadu
        /// Používá se pro nalezení začátku závorky
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
            return 0;
        }

        /// <summary>
        /// Nalezení první instance symbolu v poli zepředu
        /// Používá se pro nalezení konce závorky
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
                    return i;
                }
            }
            return 0;
        }
    }
}
