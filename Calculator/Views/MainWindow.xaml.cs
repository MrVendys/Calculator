using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using Calculator.ViewModels;

namespace Calculator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml.
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly MainWindowViewModel _viewModel;

        public MainWindow()
        {
            InitializeComponent();
            _viewModel = (MainWindowViewModel)DataContext;
            InitializeCommands();
        }

        private void InitializeCommands()
        {
            CommandBinding vypocitejHandler = new CommandBinding(CalculatorCommands.OdesliPrikladCommand, _viewModel.Vypocitej);
            CommandBinding smazHandler = new CommandBinding(CalculatorCommands.SmazSymbolCommand, _viewModel.SmazSymbol);
            CommandBinding pridejHandler = new CommandBinding(CalculatorCommands.PridejSymbolCommand, _viewModel.PridejSymbol);
            CommandBinding historyPrikladClickHandler = new CommandBinding(CalculatorCommands.OnHistoryPrikladClickCommand, _viewModel.VratPriklad);

            this.CommandBindings.Add(vypocitejHandler);
            this.CommandBindings.Add(smazHandler);
            this.CommandBindings.Add(pridejHandler);
            this.CommandBindings.Add(historyPrikladClickHandler);
        }

        private void InputTextbox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            // chyba s "-", Regex pořád bere jako interval a špatně Escapuje
            string pattern = "[-0-9,.()";
            
            foreach (char znak in _viewModel.Counting.ZnakyOperaci)
            {
                if (znak == '-')
                    continue;
                else
                    pattern += znak.ToString();
            }
            pattern += "]";
            
            Regex regex = new Regex(pattern);
            e.Handled = !regex.IsMatch(e.Text);
        }
    }
}