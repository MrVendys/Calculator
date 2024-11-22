using Calculator.UI.ViewModels;
using System.Windows;
using System.Windows.Input;

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
            CommandBinding historyPrikladClickHandler = new CommandBinding(CalculatorCommands.OnHistoryPrikladClickCommand, _viewModel.VratPriklad);
            CommandBinding pridejHandler = new CommandBinding(CalculatorCommands.PridejSymbolCommand,
                (s, e) => _viewModel.PridejSymbol((string)e.Parameter),
                (s, e) => e.CanExecute = PridejSymbolCanExecute((string)e.Parameter));

            CommandBindings.Add(vypocitejHandler);
            CommandBindings.Add(smazHandler);
            CommandBindings.Add(smazAllHandler);
            CommandBindings.Add(pridejHandler);
            CommandBindings.Add(historyPrikladClickHandler);
        }

        private void Window_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (_carkaHandled)
            {
                _carkaHandled = false;
            }
            else if (PridejSymbolCanExecute(e.Text))
            {
                _viewModel.PridejSymbol(e.Text);
            }
        }

        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Decimal)
            {
                _carkaHandled = true;

                if (PridejSymbolCanExecute(_viewModel.DesetinnyOddelovac))
                {
                    _viewModel.PridejSymbol(_viewModel.DesetinnyOddelovac);
                }
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