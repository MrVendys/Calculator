namespace Calculator.Core.Exceptions
{
    /// <summary>
    /// Výjimka pro špatně zadaný příklad.
    /// Např: Nekompletní závorka, chybějící číslo 
    /// </summary>
    public class NeplatnyVstupException : Exception
    {
        private static readonly Dictionary<ChybovyKod, string> _chyboveHlasky = new Dictionary<ChybovyKod, string>()
        {
            { ChybovyKod.NeniCislo, "Operátor nemá číslo pro výpočet." },
            { ChybovyKod.ChybejiciCislo, "Znak vedle operátoru není číslo." },
            { ChybovyKod.PrazdnaZavorka, "V příkladu se vyskytla prázdná závorka." },
            { ChybovyKod.ChybiOteviraciZavorka, "Chybí otevírací závorka." },
            { ChybovyKod.ChybiZaviraciZavorka, "Chybí zavírací závorka." },
            { ChybovyKod.ChybaVeVypoctu, "Špatně zadaná čísla pro výpočet" },
            { ChybovyKod.DeleniNulou, "Dělení nulou." },
        };

        public NeplatnyVstupException(ChybovyKod kod, string? message = null) : base(message ?? _chyboveHlasky[kod])
        {
            ChybovyKod = kod;
        }

        public NeplatnyVstupException(ChybovyKod kod, NeplatnyVstupException innerException, string? message = null) : base(message ?? _chyboveHlasky[kod], innerException)
        {
            ChybovyKod = kod;
        }

        public ChybovyKod ChybovyKod { get; }
    }
}
