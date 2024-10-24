namespace Calculator.Core.Strategies
{
    internal class MultiplyStrategy : OperationStrategyBase
    {
        internal override byte Priorita => 2;

        internal override char ZnakOperatoru => '*';

        internal override double Vypocitej(double cislo1, double cislo2)
        {
            return cislo1 * cislo2;
        }
    }
}
