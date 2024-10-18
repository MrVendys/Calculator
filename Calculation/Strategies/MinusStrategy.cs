namespace Calculation.Strategies
{
    internal class MinusStrategy : OperationStrategyBase
    {
        internal override char ZnakOperatoru => '-';

        internal override double Vypocitej(double cislo1, double? cislo2)
        {
            return cislo1 - cislo2.Value;
        }
    }
}
