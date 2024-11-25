namespace Calculator.Core.Exceptions
{
    /// <summary>
    /// Výjimka pro špatné používání knihovny.
    /// Např: Špatné přidání nové operace
    /// </summary>
    public class SpatnePouzitiException : Exception
    {
        private static readonly Dictionary<ChybovyKodSpatnePouziti, string> _chyboveHlasky = new Dictionary<ChybovyKodSpatnePouziti, string>()
        {
            { ChybovyKodSpatnePouziti.ChybiZnak, "Operace nemá přiřazený znak" },
            { ChybovyKodSpatnePouziti.DuplicitniOperace, "Tento znak je již používán jinou operací." },
            { ChybovyKodSpatnePouziti.Neidentifikovano, "Nastala výjimka. Neznámá softwarová výjimka." },
        };

        public SpatnePouzitiException(ChybovyKodSpatnePouziti kod, string? message = null) : base(message ?? _chyboveHlasky[kod])
        {
            ChybovyKod = kod;
        }

        public SpatnePouzitiException(ChybovyKodSpatnePouziti kod, NeplatnyVstupException innerException, string? message = null) : base(message ?? _chyboveHlasky[kod], innerException)
        {
            ChybovyKod = kod;
        }

        public ChybovyKodSpatnePouziti ChybovyKod { get; }
    }

    public enum ChybovyKodSpatnePouziti
    {
        ChybiZnak,
        DuplicitniOperace,
        Neidentifikovano
    }
}
