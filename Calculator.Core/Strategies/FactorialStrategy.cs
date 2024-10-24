namespace Calculator.Core.Strategies
{
    internal class FactorialStrategy : OperationStrategyBase
    {
        internal override byte Priorita => 5;

        internal override char ZnakOperatoru => '!';

        internal override PoziceCisla Pozice => PoziceCisla.Vlevo;

        internal override double Vypocitej(double cislo1, double cislo2)
        {
            double result = 1;
            for (int i = (int)cislo1; i > 1; i--)
            {
                result = result * i;
            }

            return result;
        }
    }
}
