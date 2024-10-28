using System.Windows;
using System.Windows.Input;
using Calculator.UI.ViewModels;

namespace Calculator.UI.Views
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
            CommandBinding smazAllHandler = new CommandBinding(CalculatorCommands.SmazAllSymbolyCommand, _viewModel.SmazAllSymboly);
            CommandBinding pridejHandler = new CommandBinding(CalculatorCommands.PridejSymbolCommand, (s,e) => _viewModel.PridejSymbol((string)e.Parameter));
            CommandBinding historyPrikladClickHandler = new CommandBinding(CalculatorCommands.OnHistoryPrikladClickCommand, _viewModel.VratPriklad);

            this.CommandBindings.Add(vypocitejHandler);
            this.CommandBindings.Add(smazHandler);
            this.CommandBindings.Add(smazAllHandler);
            this.CommandBindings.Add(pridejHandler);
            this.CommandBindings.Add(historyPrikladClickHandler);
        }

        private void Window_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (e.Text == "," || e.Text == ".")
            {
                _viewModel.PridejSymbol(System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator);
            }
            else
            {
                _viewModel.PridejSymbol(e.Text);
            }
        }
    }
}