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
            if (cislo1 < 0)
                throw new InputValidationException("Pod odmocninou nemůže být záporné číslo");

            return Math.Sqrt(cislo1);
        }
    }
}
