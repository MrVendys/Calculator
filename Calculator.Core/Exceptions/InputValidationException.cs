namespace Calculator.Core.Exceptions
{
    /// <summary>
    /// Výjimka pro špatně zadaný příklad.
    /// Např: Nekompletní závorka, chybějící číslo 
    /// </summary>
    public class InputValidationException : Exception
    {
        public InputValidationException() { }

        public InputValidationException(string message) : base(message) { }

        public InputValidationException(string message, Exception innerException) : base(message, innerException) { }
    }
}
