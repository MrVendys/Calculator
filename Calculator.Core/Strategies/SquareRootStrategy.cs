using Calculator.Core.Exceptions;

namespace Calculator.Core.Strategies
{
    internal class SquareRootStrategy : OperationStrategyBase
    {
        public override byte Priorita => 4;

        public override char ZnakOperatoru => '√';

        public override PoziceCisla Pozice => PoziceCisla.Vpravo;

        public override double Vypocitej(double cislo1)
        {
            if (cislo1 >= 0)
            {
                return Math.Sqrt(cislo1);
            }
            else
            {
                throw new InputValidationException("Pod odmocninou nemůže být záporné číslo");
            }
        }
    }
}
