using Calculator.Strategies;

namespace Calculator
{
    public class Counting
    {
        /// <summary>
        /// Výčet použitelných operací
        /// </summary>
        private List<(string Operace, IOperationStrategy Strategie)> _operaceList = new List<(string Operace, IOperationStrategy Strategie)>
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

        public float? Pocitej(string priklad)
        {
            return Vyhodnot(DoTokenu(priklad));
        }

        /// <summary>
        /// Přepsání příkladu do pole stringů
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
        private float? Vyhodnot(string[] tokeny)
        {
            //První fáze = naleznutí závorek, rekurze na výpočet příkladu v ní, a nahrazení závorek mezivýsledkem do pole "tokeny"
            while (tokeny.Contains("("))
            {
                int oteviraciId = NajdiPosledni("(", tokeny);
                int uzaviraciId = NajdiPrvni(")", tokeny, oteviraciId);

                if(_chyba)
                {
                    return null;
                } 

                string[] zavorkyTkny = new string[uzaviraciId - oteviraciId - 1];
                Array.Copy(tokeny, oteviraciId + 1, zavorkyTkny, 0, (uzaviraciId - oteviraciId - 1));

                //Rekurze pro výpočet části příkladu v závorce
                var zavorkyVysl = Vyhodnot(zavorkyTkny);
                if (zavorkyVysl == null)
                    return null;

                tokeny = tokeny.Take(oteviraciId).Concat(new string[] { zavorkyVysl.ToString() }).Concat(tokeny.Skip(uzaviraciId + 1)).ToArray();
            }

            //Druhá fáze funkce = projetí všech symbolů v poli "tokeny" a postupný výpočet.
            //Počítá se podle operací
            List<(string Operation, IOperationStrategy Strategy)> pouziteOperace = NajdiOperatory(tokeny);
            int index = 0;
            while (tokeny.Length > 1)
            {
                if (!int.TryParse(tokeny[index], out _))
                {
                    if (tokeny[index] == pouziteOperace[0].Operation)
                    {
                        IOperationStrategy pouzitaOperace = pouziteOperace[0].Strategy;

                        //Kazda operace prijima pole string[] s operatorem a znakem pred nim a za nim.
                        //Jednotlive operatory si s tim poradi a vrati pole string[], kterym se nahradi cast zadavaciho pole "tokeny[]"
                        //Duvodem je nejednoznacny pocet a usporadani znaku pro vypocet urciteho operatoru: 
                        //1 + 2 = 3 znaky
                        //3! = 2 znaky.. jeden pred operatorem
                        //√4 = 2 znaky.. jeden za operatorem
                        //Např. ["1","+","2"]
                        string[] result = pouzitaOperace.Vypocitej(tokeny.Skip(index - 1).Take(3).ToArray());
                        if (result.Length == 0)
                             return null;

                        tokeny = tokeny.Take(index - 1).Concat(result).Concat(tokeny.Skip(index + 2)).ToArray();
                        pouziteOperace.RemoveAt(0);
                        index = -1;
                    }
                }
                index++;
            }
            return float.Parse(tokeny[0]);
        }

        /// <summary>
        /// Nalezení použitých operátorů v části příkladu
        /// </summary>
        /// <param name="tokeny">Část příkladu</param>
        /// <returns> List Touple(string: roperátor jako řetězec, IOperationStrategy: objekt operátoru) 
        /// naleznutých operací, uspořádaný sestupně podle vlastnosti Priorita</returns>
        private List<(string, IOperationStrategy)> NajdiOperatory(string[] tokeny)
        {
            var pouziteOperace = new List<(string Operation, IOperationStrategy Strategie)>();
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

            _chyba = true;
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

            _chyba = true;
            return 0;
        }
    }
}
