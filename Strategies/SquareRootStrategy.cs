namespace Calculator.Strategies
{
    internal class SquareRootStrategy : IOperationStrategy
    {
        private readonly int _priorita = 4;
        public int Priorita { get { return _priorita; } }

        public string[] Vypocitej(string[] tokens)
        {
            try
            {

            if(tokens[1].Equals("√"))
            {
                double result = Math.Sqrt(double.Parse(tokens[2]));
                return new string[] { tokens[0], result.ToString() };
            }
            else
            {
                double result = Math.Sqrt(double.Parse(tokens[1]));
                return new string[] { result.ToString() };
            }
            }
            catch (Exception e) { return new string[] { }; }

        }
    }
}
