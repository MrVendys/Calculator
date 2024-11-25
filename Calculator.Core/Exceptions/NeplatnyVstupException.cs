namespace Calculator.Core.Exceptions
{
    /// <summary>
    /// Výjimka pro špatně zadaný příklad.
    /// Např: Nekompletní závorka, chybějící číslo 
    /// </summary>
    public class NeplatnyVstupException : Exception
    {
        private static readonly Dictionary<ChybovyKodNeplatnyVstup, string> _chyboveHlasky = new Dictionary<ChybovyKodNeplatnyVstup, string>()
        {
            { ChybovyKodNeplatnyVstup.NeniCislo, "Operátor nemá číslo pro výpočet." },
            { ChybovyKodNeplatnyVstup.ChybejiciCislo, "Znak vedle operátoru není číslo." },
            { ChybovyKodNeplatnyVstup.PrazdnaZavorka, "V příkladu se vyskytla prázdná závorka." },
            { ChybovyKodNeplatnyVstup.ChybiOteviraciZavorka, "Chybí otevírací závorka." },
            { ChybovyKodNeplatnyVstup.ChybiZaviraciZavorka, "Chybí zavírací závorka." },
            { ChybovyKodNeplatnyVstup.ChybaVeVypoctu, "Špatně zadaná čísla pro výpočet" },
            { ChybovyKodNeplatnyVstup.DeleniNulou, "Dělení nulou." },
        };

        public NeplatnyVstupException(ChybovyKodNeplatnyVstup kod, string? message = null) : base(message ?? _chyboveHlasky[kod])
        {
            ChybovyKod = kod;
        }

        public NeplatnyVstupException(ChybovyKodNeplatnyVstup kod, NeplatnyVstupException innerException, string? message = null) : base(message ?? _chyboveHlasky[kod], innerException)
        {
            ChybovyKod = kod;
        }

        public ChybovyKodNeplatnyVstup ChybovyKod { get; }
    }

    public enum ChybovyKodNeplatnyVstup
    {
        NeniCislo,
        ChybejiciCislo,
        PrazdnaZavorka,
        ChybiOteviraciZavorka,
        ChybiZaviraciZavorka,
        ChybaVeVypoctu,
        DeleniNulou,
    }
}
