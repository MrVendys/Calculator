﻿using Calculator.Strategies;
using System.Linq;
using System.Windows.Navigation;
using System.Xml.XPath;

namespace Calculator
{
    public class Counting
    {
        /// <summary>
        /// Dá vědět MainWindow.cs, že se někde vyskytla chyba s konkrétní chybovou hláškou.
        /// </summary>
        /// <param name="chyba">Chybový text, který chceme ukázat uživateli</param>
        public delegate void ChybaHandler(string chyba);
        public event ChybaHandler Chyba;

        /// <summary>
        /// Výčet použitelných operací.
        /// Key: string, znak operátoru.
        /// Value: OperationStrategy, instance OperationStrategy odpovídajícího znaku.
        /// </summary>
        private Dictionary<string, OperationStrategyBase> _operace = new Dictionary<string, OperationStrategyBase>()
        {
            { "+",new PlusStrategy() },
            { "-",new MinusStrategy() },
            { "*",new MultiplyStrategy() },
            { "/",new DivideStrategy() },
            { "^",new PowerStrategy() },
            { "√",new SquareRootStrategy() },
            { "!", new FactorialStrategy() }
            };

        public Counting() {
        }

        /// <summary>
        /// Volaná funkce z <see cref="MainWindow.SubmitButton_Click(object, System.Windows.RoutedEventArgs)"/>
        /// </summary>
        /// <param name="priklad"></param>
        /// <returns></returns>
        public double? Pocitej(string priklad)
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
                int? oteviraciId = NajdiPosledni("(", tokeny);
                if (!oteviraciId.HasValue)
                    return null;

                int? uzaviraciId = NajdiPrvni(")", tokeny, oteviraciId.Value);
                if (!uzaviraciId.HasValue)
                    return null;

                string[] zavorkyTkny = new string[uzaviraciId.Value - oteviraciId.Value - 1];
                Array.Copy(tokeny, oteviraciId.Value + 1, zavorkyTkny, 0, (uzaviraciId.Value - oteviraciId.Value - 1));

                if (zavorkyTkny.Length < 1)
                {
                    Chyba?.Invoke("Chyba: Prázdná závorka");
                    return null;
                }

                //Rekurze pro výpočet části příkladu v závorce
                var zavorkyVysl = Vyhodnot(zavorkyTkny);

                tokeny = tokeny.Take(oteviraciId.Value).Concat(new string[] { zavorkyVysl.ToString() }).Concat(tokeny.Skip(uzaviraciId.Value + 1)).ToArray();
            }
            return tokeny;
        }

        /// <summary>
        /// Výpočet celého/daného kusu příkladu.
        /// </summary>
        /// <param name="tokeny">Část příkladu pro výpočet</param>
        /// <returns>Vypočítaná část příkladu</returns>
        private double? Vyhodnot(string[] tokeny)
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
                                if (tokeny.Length >= 2)
                                {
                                    bool vporadku = Zkontroluj(new string[] {tokeny[index - 1]});
                                    if (!vporadku)
                                    {
                                        Chyba?.Invoke($"Chybí číslo pro výpočet u operátoru: {pouzitaOperace.ZnakOperatoru}");
                                        return null;
                                    }
                                }
                                else
                                {
                                    Chyba?.Invoke("Moc krátký příklad pro výpočet");
                                    return null;
                                }
                                meziVysl = pouzitaOperace.Vypocitej(double.Parse(tokeny[index - 1]),null);
                                takeIndex = index - 1;
                                skipIndex = index + 1;
                                break;

                            // Pro operátory typu: √
                            case PoziceCisla.Vpravo:
                                if (tokeny.Length >= 2)
                                {
                                    bool vporadku = Zkontroluj(new string[] { tokeny[index + 1] });
                                    if (!vporadku)
                                    {
                                        Chyba?.Invoke($"Chybí číslo pro výpočet u operátoru: {pouzitaOperace.ZnakOperatoru}");
                                        return null;
                                    }
                                }
                                else
                                {
                                    Chyba?.Invoke("Moc krátký příklad pro výpočet");
                                    return null;
                                }
                                meziVysl = pouzitaOperace.Vypocitej(double.Parse(tokeny[index + 1]), null);
                                takeIndex = index;
                                skipIndex = index + 1 + 1;
                                break;

                            // Pro operátory typu: +,-,*,:
                            case PoziceCisla.VlevoIVpravo:
                                if (tokeny.Length >= 3)
                                {
                                    bool vporadku = Zkontroluj(new string[] { tokeny[index - 1], tokeny[index + 1] });
                                    if (!vporadku)
                                    {
                                        Chyba?.Invoke($"Chybí číslo pro výpočet u operátoru: {pouzitaOperace.ZnakOperatoru}");
                                        return null;
                                    }
                                }
                                else
                                {
                                    Chyba?.Invoke("Moc krátký příklad pro výpočet");
                                    return null;
                                }
                                    
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
        /// Zkontroluje, zda má operátor vedle sebe číslo, se kterým může počítat.
        /// </summary>
        /// <param name="tokeny"></param>
        /// <returns>True: Všechno je v pořádku. False: Našla se chyba</returns>
        private bool Zkontroluj(string[] tokeny)
        {
            bool vporadku = true;
            foreach (var token in tokeny) 
            {
                if (double.TryParse(token, out _))
                    vporadku = vporadku && true;
                else
                    vporadku = vporadku && false;
            }

            return vporadku;
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
                for (int j = 0; j < _operace.Count; j++)
                {
                    if (!int.TryParse(tokeny[i], out _))
                    {
                        if (_operace.Keys.Contains(tokeny[i]))
                        {
                            pouziteOperace.Add(_operace[tokeny[i]]);
                            break;
                        }
                    }
                    else
                    {
                        break;
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
        private int? NajdiPosledni(string symbol, string[] tokeny)
        {
            for (int i = tokeny.Length - 1; i >= 0; i--)
            {
                if (tokeny[i] == symbol)
                {
                    return i;
                }
            }

            Chyba?.Invoke("Nekompletní závorka");
            return null;
        }

        /// <summary>
        /// Nalezení první instance symbolu v poli zepředu.
        /// Používá se pro nalezení konce závorky.
        /// </summary>
        /// <param name="symbol">Charakter, který se bude hledat</param>
        /// <param name="tokeny">Pole, ve kterém se bude vyhledávat</param>
        /// <param name="startovaciId">Index otevřené závorky, od kterého se začne vyhledávat</param>
        /// <returns>Index hledaného symbolu</returns>
        private int? NajdiPrvni(string symbol, string[] tokeny, int startovaciId)
        {
            for (int i = startovaciId; i < tokeny.Length; i++)
            {
                if (tokeny[i] == symbol)
                {
                    return i;
                }
            }

            Chyba?.Invoke("Nekompletní závorka");
            return null;
        }
    }
}
