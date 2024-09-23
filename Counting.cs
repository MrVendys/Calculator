using Calculator.Strategies;

namespace Calculator
{
    public class Counting
    {

        /// <summary>
        /// Dostupné operace
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
        /// Kontrola chyby typu null
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
        /// </summary>
        /// <param name="priklad">Zadaný příklad</param>
        /// <returns>Pole charakterů</returns>
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
                    cislo += tokeny;
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
        /// Výpočet celého příkladu
        /// </summary>
        /// <param name="tokeny">Zadaný příklad</param>
        /// <returns>Vypočítaný příklad</returns>
        private float? Vyhodnot(string[] tokeny)
        {
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
                var zavorkyVysl = Vyhodnot(zavorkyTkny);
                if (zavorkyVysl == null)
                    return null;
                tokeny = tokeny.Take(oteviraciId).Concat(new string[] { zavorkyVysl.ToString() }).Concat(tokeny.Skip(uzaviraciId + 1)).ToArray();

            }

            List<(string Operation, IOperationStrategy Strategy)> pouziteOperace = NajdiOperatory(tokeny);
            pouziteOperace = pouziteOperace.OrderByDescending(item => item.Strategy.Priorita).ToList();

            
            int index = 0;
            while (tokeny.Length > 1)
            {
                if (!int.TryParse(tokeny[index], out _))
                {
                    if (tokeny[index] == pouziteOperace[0].Operation)
                    {
                        IOperationStrategy pouzitaOperace = pouziteOperace[0].Strategy;
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
        /// Nalezení použitých operátorů
        /// </summary>
        /// <param name="tokeny">Pole, ve kterém se bude hledat</param>
        /// <returns>List<string, IOperationStrategy> použitých operátorů</returns>
        private List<(string, IOperationStrategy)> NajdiOperatory(string[] tokeny)
        {
            var pouziteOperace = new List<(string Operation, IOperationStrategy Strategy)>();

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
            return pouziteOperace;
        }
        /// <summary>
        /// Nalezení poslední instance symbolu
        /// </summary>
        /// <param name="symbol">Charakter, který se bude hledat</param>
        /// <param name="tokeny">Pole, ve kterém se bude vyhledávat</param>
        /// <returns>List<string, IOperationStrategy> použitých operátorů</returns>
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
        /// Nalezení první instance symbolu
        /// </summary>
        /// <param name="symbol">Charakter, který se bude hledat</param>
        /// <param name="tokeny">Pole, ve kterém se bude vyhledávat</param>
        /// <param name="startovaciId">Index, na kterém se začne vyhledávat</param>
        /// <returns>List<string, IOperationStrategy> použitých operátorů</returns>
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
