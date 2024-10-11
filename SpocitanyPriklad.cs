namespace Calculator
{
    public class SpocitanyPriklad
    {
        private string[] _priklad = new string[2];

        public SpocitanyPriklad(string priklad, string vysledek)
        {
            _priklad.Append(priklad);
            _priklad.Append(vysledek);
        }

        public string[] Priklad => _priklad;
    }
}
