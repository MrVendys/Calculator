namespace Calculator.Strategies
{
    internal class FactorialStrategy : OperationStrategy
    {
        public override int Priorita => 5;

        public override string ZnakOperatoru => "!";

        public override int PocetCiselZleva => 1;

        public override int PocetCiselZprava => 0;

        public override Enum PoziceCisel => OperationStrategy.VycetPozic.Vlevo;

        public override string[] Vypocitej(double cislo1, double? cislo2)
        {
            double result = 1;
            try
            {
                for (int i = 1; i <= cislo1; i++)
                {
                    result = result * i;
                }
                return new string[] { result.ToString()};

            } catch (Exception e) { return new string[] { }; }
            
        }
    }
}
