﻿namespace Calculator.Core.Strategies
{
    internal class OperaceOdcitani : OperaceBase
    {
        public override char ZnakOperatoru => '-';

        public override double Vypocitej(double cislo1, double cislo2)
        {
            return cislo1 - cislo2;
        }
    }
}
