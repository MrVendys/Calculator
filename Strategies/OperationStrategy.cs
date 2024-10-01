namespace Calculator.Strategies
{
    internal abstract class OperationStrategy
    {
        /// <summary>
        /// Priorita operatoru od 1,
        /// 1 == nejmensi -> resi se jako posledni (příklad: +,-),
        /// Použití v Counting.cs pro výpočet 
        /// </summary>
        public virtual int Priorita => 1;

        public virtual string ZnakOperatoru => "+";

        /// <summary>
        /// Počet čísel, se kterými operátor pracuje zleva
        /// </summary>
        public virtual int PocetCiselZleva => 1;

        /// <summary>
        /// Počet čísel, se kterými operátor pracuje zprava
        /// </summary>
        public virtual int PocetCiselZprava => 1;

        public enum VycetPozic
        {
            Vlevo,
            Vpravo,
            VlevoIVpravo
        }

        /// <summary>
        /// Kde se nacházejí čísla, se kterými operátor pracuje.: Před ním (Vlevo), za ním (Vpravo), nebo oboje (VlevoIVpravo)
        /// </summary>
        public virtual Enum PoziceCisel { get; }

        /// <summary>
        /// Výpočet konkrétní části příkladu
        /// </summary>
        /// <param name="tokeny">Tokeny počítaného příkladu</param>
        /// <returns>Vypočítaná hodnota v string poli</returns>
        public virtual string[] Vypocitej(double cislo1, double? cislo2)
        {
            return null;
        }


    }
}
