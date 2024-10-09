using System.Windows.Input;
namespace Calculator.Commands
{
    public class CalculatorCommands : RoutedUICommand
    {
        public static readonly RoutedUICommand OdesliPrikladCommand = new RoutedUICommand();
        public static readonly RoutedUICommand SmazSymbolCommand = new RoutedUICommand();
        public static readonly RoutedUICommand PridejSymbolCommand = new RoutedUICommand();
    }
}
