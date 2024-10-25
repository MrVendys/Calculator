namespace Calculator.Core.Strategies
{
    internal class PowerStrategy : OperationStrategyBase
    {
        public override byte Priorita => 3;

        public override char ZnakOperatoru => '^';

        public override double Vypocitej(double cislo1, double cislo2)
        {
            return Math.Pow(cislo1, cislo2);
        }
    }
}
