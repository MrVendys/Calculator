namespace Calculator.Strategies
{
    internal class SquareRootStrategy : OperationStrategy
    {
        public override int Priorita => 4;

        public override string ZnakOperatoru => "√";

        public override int PocetCiselZleva => 0;

        public override int PocetCiselZprava => 1;

        public override Enum PoziceCisel => OperationStrategy.VycetPozic.Vpravo;

        public override string[] Vypocitej(double cislo1, double? cislo2)
        {
            try
            {
                double result = Math.Sqrt(cislo1);
                return new string[] { result.ToString() };
            
            }
            catch (Exception e) { return new string[] { }; }

        }
    }
}
