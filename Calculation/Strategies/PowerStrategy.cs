namespace Calculator.Core.Strategies
{
    internal class PowerStrategy : OperationStrategyBase
    {
        internal override byte Priorita => 3;

        internal override char ZnakOperatoru => '^';

        internal override double Vypocitej(double cislo1, double? cislo2)
        {
            return Math.Pow(cislo1, cislo2.Value);
        }
    }
}
