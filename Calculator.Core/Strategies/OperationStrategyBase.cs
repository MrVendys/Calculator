namespace Calculator.Core.Strategies
{
    internal abstract class OperationStrategyBase
    {
        /// <summary>
        /// Priorita operatoru od 1,
        /// 1 == nejmensi -> resi se jako posledni (příklad: +,-),
        /// Použití v Counting.cs pro výpočet,
        /// Default: 1
        /// </summary>
        internal virtual byte Priorita => 1;

        internal abstract char ZnakOperatoru { get; }

        /// <summary>
        /// Kde se nacházejí čísla, se kterými operátor pracuje.: Před ním (Vlevo), za ním (Vpravo), nebo oboje (VlevoIVpravo),
        /// Default: VlevoIVpravo
        /// </summary>
        internal virtual PoziceCisla Pozice => PoziceCisla.VlevoIVpravo;

        /// <summary>
        /// Výpočet konkrétní části příkladu
        /// </summary>
        /// <param name="tokeny">Tokeny počítaného příkladu</param>
        /// <returns>Vypočítaná hodnota v string poli</returns>
        internal abstract double Vypocitej(double cislo1, double? cislo2);
    }
}
