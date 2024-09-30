namespace Calculator.Strategies
{
    internal class MultiplyStrategy : OperationStrategy
    {
        public int Priorita => 2;

        public string ZnakOperatoru => "*";

        public int PocetCisel => 2;

        public Enum Pozice = OperationStrategy.VycetPozic.ZlevaIZprava;

        public string[] Vypocitej(double? cislo1, double? cislo2)
        {
            try
            {

            double result = cislo1.Value * cislo2.Value;
            return new string[] { result.ToString() };

            }
            catch (Exception e) { return new string[] { }; }
        }
    }
}
