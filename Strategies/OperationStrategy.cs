namespace Calculator.Strategies
{
    internal abstract class OperationStrategy
    {
        /// <summary>
        /// Priorita operatoru od 1,
        /// 1 == nejmensi -> resi se jako posledni (příklad: +,-),
        /// Použití v Counting.cs pro výpočet 
        /// </summary>
        public int Priorita => 1;

        public string ZnakOperatoru { get; }

        public int PocetCisel => 2;

        public enum VycetPozic
        {
            Zleva,
            Zprava,
            ZlevaIZprava
        }

        public virtual Enum Pozice 
        { 
            get 
            { 
                return Pozice; 
            } 
            set 
            { 
                Pozice = VycetPozic.ZlevaIZprava; 
            } 
        }



        /// <summary>
        /// Výpočet konkrétní části příkladu
        /// </summary>
        /// <param name="tokeny">Tokeny počítaného příkladu</param>
        /// <returns>Vypočítaná hodnota v string poli</returns>
        public virtual string[] Vypocitej(double? cislo1, double? cislo2)
        {
            return null;
        }


    }
}
