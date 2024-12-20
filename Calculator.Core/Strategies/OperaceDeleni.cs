﻿using Calculator.Core.Exceptions;

namespace Calculator.Core.Strategies
{
    internal class OperaceDeleni : OperaceBase
    {
        public override byte Priorita => 2;

        public override char ZnakOperatoru => '/';

        public override double Vypocitej(double cislo1, double cislo2)
        {
            if (cislo2 != 0)
            {
                return cislo1 / cislo2;
            }
            else
            {
                throw new NeplatnyVstupException(ChybovyKodNeplatnyVstup.DeleniNulou, "Dělení nulou!");
            }
        }
    }
}