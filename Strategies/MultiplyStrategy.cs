namespace Calculator.Strategies
{
    internal class MultiplyStrategy : OperationStrategy
    {
        public override int Priorita => 2;

        public override string ZnakOperatoru => "*";

        public override Enum PoziceCisel => OperationStrategy.VycetPozic.VlevoIVpravo;

        public override string[] Vypocitej(double cislo1, double? cislo2)
        {
            try
            {

            double result = cislo1 * cislo2.Value;
            return new string[] { result.ToString() };

            }
            catch (Exception e) { return new string[] { }; }
        }
    }
}
