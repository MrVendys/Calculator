using Calculator.Enums;

namespace Calculator.Strategies
{
    internal class SquareRootStrategy : OperationStrategyBase
    {
        public override byte Priorita => 4;

        public override char ZnakOperatoru => '√';

        public override PoziceCisla Pozice => PoziceCisla.Vpravo;

        public override double Vypocitej(double cislo1, double? cislo2)
        {
            return Math.Sqrt(cislo1);
        }
    }
}
