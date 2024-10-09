using Calculator.Enums;

namespace Calculator.Strategies
{
    internal class FactorialStrategy : OperationStrategyBase
    {
        public override byte Priorita => 5;

        public override char ZnakOperatoru => '!';

        public override PoziceCisla Pozice => PoziceCisla.Vlevo;

        public override double Vypocitej(double cislo1, double? cislo2)
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
