namespace Calculator.Core.Exceptions
{
    /// <summary>
    /// Výjimka pro špatné používání knihovny.
    /// Např: Špatné přidání nové operace
    /// </summary>
    public class SpatnePouzitiException : ExceptionBase
    {
        public SpatnePouzitiException(ChybovyKod kod, string? message = null) : base(kod, message ?? _chyboveHlasky[kod]) { }

        public SpatnePouzitiException(ChybovyKod kod, NeplatnyVstupException innerException, string? message = null) : base(kod, innerException, message ?? _chyboveHlasky[kod]) { }
    }
}
