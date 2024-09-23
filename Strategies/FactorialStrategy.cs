﻿using System;

namespace Calculator.Strategies
{
    internal class FactorialStrategy : IOperationStrategy
    {
        private readonly int _priorita = 5;
        public int Priorita { get { return _priorita; } }

        public string[] Vypocitej(string[] tokeny)
        {
            double result = 1;
            try
            {
                for (int i = 1; i <= double.Parse(tokeny[0]); i++)
                {
                    result = result * i;
                }

            } catch (Exception e) { return new string[] { }; }

            if (tokeny.Length == 3)
            {
                return new string[] { result.ToString(), tokeny[2] };
            }
            else
            {
                return new string[] { result.ToString() };
            }
            
        }
    }
}
