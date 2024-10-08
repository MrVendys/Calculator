namespace Calculator
{
    /// <summary>
    /// Vyjímka pro špatně zadaný příklad.</para>
    /// Např: Nekompletní závorka, chybějící číslo 
    /// </summary>
    internal class InputValidationException : Exception
    {
        public InputValidationException() { }

        public InputValidationException(string message) : base(message) { }

        public InputValidationException(string message, Exception innerException): base(message, innerException) { }
    }
}
