namespace Calculator.Core.Strategies
{
    internal class OperaceNasobeni : OperaceBase
    {
        public override byte Priorita => 2;

        public override char ZnakOperatoru => '*';

        public override double Vypocitej(double cislo1, double cislo2)
        {
            return cislo1 * cislo2;
        }
    }
}
