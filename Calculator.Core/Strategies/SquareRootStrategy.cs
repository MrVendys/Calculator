﻿using Calculator.Core.Exceptions;

namespace Calculator.Core.Strategies
{
    internal class SquareRootStrategy : OperationStrategyBase
    {
        internal override byte Priorita => 4;

        internal override char ZnakOperatoru => '√';

        internal override PoziceCisla Pozice => PoziceCisla.Vpravo;

        internal override double Vypocitej(double cislo1, double cislo2)
        {
            if (cislo1 >= 0)
                return Math.Sqrt(cislo1);
            else
                throw new InputValidationException("Pod odmocninou nemůže být záporné číslo");
        }
    }
}
