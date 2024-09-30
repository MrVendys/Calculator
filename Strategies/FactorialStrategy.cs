namespace Calculator.Strategies
{
    internal class FactorialStrategy : OperationStrategy
    {
        public int Priorita => 5;

        public string ZnakOperatoru => "!";

        public int PocetCisel => 1;

        public Enum Pozice = OperationStrategy.VycetPozic.Zleva;

        public string[] Vypocitej(double? cislo1, double? cislo2)
        {
            double result = 1;
            try
            {
                for (int i = 1; i <= cislo1.Value; i++)
                {
                    result = result * i;
                }
                return new string[] { result.ToString()};

            } catch (Exception e) { return new string[] { }; }
            
        }
    }
}
