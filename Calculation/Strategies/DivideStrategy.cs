namespace Calculation.Strategies
{
    internal class DivideStrategy : OperationStrategyBase
    {
        internal override byte Priorita => 2;

        internal override char ZnakOperatoru => '/';

        internal override double Vypocitej(double cislo1, double? cislo2)
        {
            return cislo1 / cislo2.Value;
        }
    }
}
