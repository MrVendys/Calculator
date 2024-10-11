using System.Windows.Input;

namespace Calculator
{
    public class CalculatorCommands : RoutedUICommand
    {
        public static readonly RoutedUICommand OdesliPrikladCommand = new RoutedUICommand("=", nameof(OdesliPrikladCommand), typeof(CalculatorCommands));
        public static readonly RoutedUICommand SmazSymbolCommand = new RoutedUICommand("←", nameof(SmazSymbolCommand), typeof(CalculatorCommands));
        public static readonly RoutedUICommand PridejSymbolCommand = new RoutedUICommand("", nameof(PridejSymbolCommand), typeof(CalculatorCommands));
        public static readonly RoutedCommand OnHistoryPrikladClickCommand = new RoutedUICommand();
    }
}
