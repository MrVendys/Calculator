using System.Text.RegularExpressions;

namespace Calculator.Core
{
    public class SpocitanyPriklad
    {
        public SpocitanyPriklad(string priklad, string vysledek)
        {
            Priklad = Regex.Replace(priklad, @"(\d+)", "\u200B$1\u200B");
            Vysledek = vysledek;
        }

        public string Priklad { get; }
        public string Vysledek { get; }
    }
}
