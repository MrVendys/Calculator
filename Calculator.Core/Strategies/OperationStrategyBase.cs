namespace Calculator.Core.Strategies
{
    internal abstract class OperationStrategyBase
    {
        /// <summary>
        /// Priorita operatoru od 1 (default). <br/>
        /// 1 == nejmensi -> resi se jako posledni (příklad: +,-).
        /// </summary>
        internal virtual byte Priorita => 1;

        internal abstract char ZnakOperatoru { get; }

        /// <summary>
        /// Kde se nacházejí čísla, se kterými operátor pracuje.: Před ním (Vlevo), za ním (Vpravo), nebo oboje (VlevoIVpravo). <br/>
        /// Default: VlevoIVpravo
        /// </summary>
        internal virtual PoziceCisla Pozice => PoziceCisla.VlevoIVpravo;

        internal abstract double Vypocitej(double cislo1, double cislo2 = 0);
    }
}
