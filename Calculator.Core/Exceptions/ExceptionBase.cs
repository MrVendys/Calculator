namespace Calculator.Core.Exceptions
{
    public abstract class ExceptionBase : Exception
    {
        protected static readonly Dictionary<ChybovyKod, string> _chyboveHlasky = new Dictionary<ChybovyKod, string>()
        {
            { ChybovyKod.NeniCislo, "Operátor nemá číslo pro výpočet." },
            { ChybovyKod.ChybejiciCislo, "Znak vedle operátoru není číslo." },
            { ChybovyKod.PrazdnaZavorka, "V příkladu se vyskytla prázdná závorka." },
            { ChybovyKod.ChybiOteviraciZavorka, "Chybí otevírací závorka." },
            { ChybovyKod.ChybiZaviraciZavorka, "Chybí zavírací závorka." },
            { ChybovyKod.ChybaVeVypoctu, "Špatně zadaná čísla pro výpočet" },
            { ChybovyKod.DeleniNulou, "Dělení nulou." },
            { ChybovyKod.DuplicitniOperace, "Tento znak je již používán jinou operací." },
            { ChybovyKod.Neidentifikovano, "Nastala výjimka. Neznámá softwarová výjimka." },
        };

        protected ExceptionBase(ChybovyKod kod, string? message = null) : base(message ?? _chyboveHlasky[kod])
        {
            ChybovyKod = kod;
        }

        protected ExceptionBase(ChybovyKod kod, NeplatnyVstupException innerException, string? message = null) : base(message ?? _chyboveHlasky[kod], innerException)
        {
            ChybovyKod = kod;
        }

        public ChybovyKod ChybovyKod { get; }
    }
}
