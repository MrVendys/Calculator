using System.Windows;
using System.Windows.Input;
using Calculator.UI.ViewModels;

namespace Calculator.UI.Views
{
    public partial class MainWindow : Window
    {
        private readonly MainWindowViewModel _viewModel;

        /// <summary>
        /// Pro správné zobrazování desetinného oddělovače u numpadu
        /// </summary>
        private bool _carkaHandled = false;

        public MainWindow()
        {
            InitializeComponent();
            _viewModel = (MainWindowViewModel)DataContext;
            InitializeCommands();
        }

        private void InitializeCommands()
        {
            CommandBinding vypocitejHandler = new CommandBinding(CalculatorCommands.OdesliPrikladCommand, (s, e) => _viewModel.Vypocitej());
            CommandBinding smazHandler = new CommandBinding(CalculatorCommands.SmazSymbolCommand, (s, e) => _viewModel.SmazSymbol());
            CommandBinding smazAllHandler = new CommandBinding(CalculatorCommands.SmazAllSymbolyCommand, (s, e) => _viewModel.SmazPriklad());
            CommandBinding pridejHandler = new CommandBinding(CalculatorCommands.PridejSymbolCommand, (s, e) => _viewModel.PridejSymbol((string)e.Parameter, PridejSymbolCanExecute));
            CommandBinding historyPrikladClickHandler = new CommandBinding(CalculatorCommands.OnHistoryPrikladClickCommand, _viewModel.VratPriklad);

            this.CommandBindings.Add(vypocitejHandler);
            this.CommandBindings.Add(smazHandler);
            this.CommandBindings.Add(smazAllHandler);
            this.CommandBindings.Add(pridejHandler);
            this.CommandBindings.Add(historyPrikladClickHandler);
        }

        private void Window_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (_carkaHandled)
            {
                _carkaHandled = false;
            }
            else
            {
                _viewModel.PridejSymbol(e.Text, PridejSymbolCanExecute);
            }
        }

        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Decimal)
            {
                _carkaHandled = true;
                _viewModel.PridejSymbol(_viewModel.DesetinnyOddelovac, PridejSymbolCanExecute);
            }
        }

        /// <summary>
        /// Zkusí zjistit, jestli má přidaný znak místo na vykreslení
        /// </summary>
        private bool PridejSymbolCanExecute(string symbol)
        {
            return InputTextBox.TryZmenFontSize(symbol);
        }
    }
}