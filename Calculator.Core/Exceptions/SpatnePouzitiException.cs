namespace Calculator.Core.Exceptions
{
    /// <summary>
    /// Výjimka pro špatné používání knihovny.
    /// Např: Špatné přidání nové operace
    /// </summary>
    public class SpatnePouzitiException : Exception
    {
        private static readonly Dictionary<ChybovyKod, string> _chyboveHlasky = new Dictionary<ChybovyKod, string>()
        {
            { ChybovyKod.DuplicitniOperace, "Tento znak je již používán jinou operací." },
            { ChybovyKod.Neidentifikovano, "Nastala výjimka. Neznámá softwarová výjimka." },
        };

        public SpatnePouzitiException(ChybovyKod kod, string? message = null) : base(message ?? _chyboveHlasky[kod])
        {
            ChybovyKod = kod;
        }

        public SpatnePouzitiException(ChybovyKod kod, NeplatnyVstupException innerException, string? message = null) : base(message ?? _chyboveHlasky[kod], innerException)
        {
            ChybovyKod = kod;
        }

        public ChybovyKod ChybovyKod { get; }
    }
}
