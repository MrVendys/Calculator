namespace Calculator.Core.Exceptions
{
    /// <summary>
    /// Výjimka pro špatně zadaný příklad.
    /// Např: Nekompletní závorka, chybějící číslo 
    /// </summary>
    public class NeplatnyVstupException : ExceptionBase
    {
        public NeplatnyVstupException(ChybovyKod kod, string? message = null) : base(kod, message ?? _chyboveHlasky[kod]) { }

        public NeplatnyVstupException(ChybovyKod kod, NeplatnyVstupException innerException, string? message = null) : base(kod, innerException, message ?? _chyboveHlasky[kod]) { }
    }
}
