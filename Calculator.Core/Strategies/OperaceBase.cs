namespace Calculator.Core.Strategies
{
    public abstract class OperaceBase
    {
        /// <summary>
        /// Priorita operatoru od 1 (default). <br/>
        /// 1 == nejmensi -> resi se jako posledni (příklad: +,-).
        /// </summary>
        public virtual byte Priorita => 1;

        public abstract char ZnakOperatoru { get; }

        /// <summary>
        /// Kde se nacházejí čísla, se kterými operátor pracuje.: Před ním (Vlevo), za ním (Vpravo), nebo oboje (VlevoIVpravo). <br/>
        /// Default: VlevoIVpravo
        /// </summary>
        public virtual PoziceCisla Pozice => PoziceCisla.VlevoIVpravo;

        public virtual double Vypocitej(double cislo1)
        {
            if (Pozice == PoziceCisla.VlevoIVpravo)
            {
                return Vypocitej(cislo1, 0);
            }
            else
            {
                throw new InvalidOperationException();
            }
        }

        public virtual double Vypocitej(double cislo1, double cislo2)
        {
            if (Pozice != PoziceCisla.VlevoIVpravo)
            {
                return Vypocitej(cislo1);
            }
            else
            {
                throw new InvalidOperationException();
            }
        }
    }
}
