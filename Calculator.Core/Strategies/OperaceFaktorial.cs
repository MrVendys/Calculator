using Calculator.Core.Exceptions;

namespace Calculator.Core.Strategies
{
    internal class OperaceFaktorial : OperaceBase
    {
        public override byte Priorita => 5;

        public override char ZnakOperatoru => '!';

        public override PoziceCisla Pozice => PoziceCisla.Vlevo;

        public override double Vypocitej(double cislo1)
        {
            if (cislo1 % 1.0 != 0)
                throw new InputValidationException(ChybovyKod.ChybaVeVypoctu, "Faktorial desetinného čísla není podporován");

            double result = 1;
            for (int i = (int)cislo1; i > 1; i--)
            {
                result = result * i;
            }

            return result;
        }
    }
}
