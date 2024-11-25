using Calculator.Core.Exceptions;

namespace Calculator.Core.Strategies
{
    internal class OperaceModulo : OperaceBase
    {
        public override byte Priorita => 2;

        public override char ZnakOperatoru => '%';

        public override double Vypocitej(double cislo1, double cislo2)
        {
            if (cislo2 == 0)
                throw new NeplatnyVstupException(ChybovyKod.DeleniNulou, "Dělení nulou!");

            if (cislo1 % 1.0 != 0 || cislo2 % 1.0 != 0)
                throw new NeplatnyVstupException(ChybovyKod.ChybaVeVypoctu, "Modulo s desetinným číslem");

            return cislo1 % cislo2;
        }
    }
}