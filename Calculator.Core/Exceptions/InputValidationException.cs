namespace Calculator.Core.Exceptions
{
    /// <summary>
    /// Výjimka pro špatně zadaný příklad.
    /// Např: Nekompletní závorka, chybějící číslo 
    /// </summary>
    public class InputValidationException : Exception
    {
        public ChybovyKod ChybovyKod { get; }

        public static Dictionary<ChybovyKod, string> ChyboveHlasky = new Dictionary<ChybovyKod, string>()
        {
            { ChybovyKod.NeniCislo, "Operátor nemá číslo pro výpočet." },
            { ChybovyKod.ChybejiciCislo, "Znak vedle operátoru není číslo." },
            { ChybovyKod.PrazdnaZavorka, "V příkladu se vyskytla prázdná závorka." },
            { ChybovyKod.OteviraciZavorka, "Chybí otevírací závorka." },
            { ChybovyKod.ZaviraciZavorka, "Chybí zavírací závorka." },
            { ChybovyKod.ChybaVeVypoctu, "Špatně zadaná čísla pro výpočet" },
            { ChybovyKod.Neidentifikovano, "Nastala výjimka. Neznámá softwarová výjimka." },
        };

        public InputValidationException(ChybovyKod kod, string message = "") : base(message)
        {
            ChybovyKod = kod;
            message = message == "" ? "" : ChyboveHlasky[kod];
        }

        public InputValidationException(Exception innerException, ChybovyKod kod, string message = "") : base(message, innerException)
        {
            ChybovyKod = kod;
            message = message == "" ? "" : ChyboveHlasky[kod];
        }
    }
}
