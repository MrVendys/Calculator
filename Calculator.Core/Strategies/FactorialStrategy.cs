using Calculator.Core.Exceptions;

namespace Calculator.Core.Strategies
{
    internal class FactorialStrategy : OperationStrategyBase
    {
        public override byte Priorita => 5;

        public override char ZnakOperatoru => '!';

        public override PoziceCisla Pozice => PoziceCisla.Vlevo;

        public override double Vypocitej(double cislo1)
        {
            if (cislo1 % 1.0 == 0)
            {
                double result = 1;
                for (int i = (int)cislo1; i > 1; i--)
                {
                    result = result * i;
                }

                return result;
            }
            else
            {
                throw new InputValidationException("Faktorial desetinného čísla momentálně nelze vypočítat");
            }
        }
    }
}
