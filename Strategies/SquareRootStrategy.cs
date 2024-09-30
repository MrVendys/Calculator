namespace Calculator.Strategies
{
    internal class SquareRootStrategy : OperationStrategy
    {
        public int Priorita => 4;

        public string ZnakOperatoru => "√";

        public int PocetCisel => 1;

        public Enum Pozice = OperationStrategy.VycetPozic.Zprava;

        public string[] Vypocitej(double? cislo1, double? cislo2)
        {
            try
            {
                double result = Math.Sqrt(cislo2.Value);
                return new string[] { result.ToString() };
            
            }
            catch (Exception e) { return new string[] { }; }

        }
    }
}
