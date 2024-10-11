using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Calculator.ViewModels;

namespace Calculator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml.
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Charaktery pro generovani tlacitek.
        /// </summary>
        private List<string> _charakteryList = new List<string>() {
        "1",
        "2",
        "3",
        "4",
        "5",
        "6",
        "7",
        "8",
        "9",
        "0",
        ",",
        "+",
        "-",
        "*",
        "/",
        "^",
        "√",
        "!",
        "(",
        ")"
        };

        private readonly MainWindowViewModel _viewModel;

        public MainWindow()
        {
            InitializeComponent();
            InitializeButtons();
            _viewModel = (MainWindowViewModel)DataContext;
            InitializeCommands();
        }

        /// <summary>
        /// Vytvoření tlačítek podle _characteryList a přidání do View na WrapPanel.
        /// </summary>
        private void InitializeButtons()
        {
            foreach (var charakter in _charakteryList)
            {
                Button b = new Button()
                {
                    Name = "ContentButton",
                    Height = 60,
                    Width = 60,
                    Content = charakter,
                    FontSize = 50,
                    VerticalContentAlignment = VerticalAlignment.Center,
                    HorizontalContentAlignment = HorizontalAlignment.Center,
                    Margin = new Thickness(1, 1, 1, 1),
                    Padding = new Thickness(-5, -5, -5, -5),
                    Command = CalculatorCommands.PridejSymbolCommand
                };

                WrapPanel.Children.Add(b);
            }
        }

        private void InitializeCommands()
        {
            CommandBinding VypocitejHandler = new CommandBinding(CalculatorCommands.OdesliPrikladCommand, _viewModel.Vypocitej);
            CommandBinding SmazHandler = new CommandBinding(CalculatorCommands.SmazSymbolCommand, _viewModel.SmazSymbol);
            CommandBinding PridejHandler = new CommandBinding(CalculatorCommands.PridejSymbolCommand, _viewModel.PridejSymbol);
            CommandBinding HistoryPrikladClickHandler = new CommandBinding(CalculatorCommands.OnHistoryPrikladClickCommand, _viewModel.VratPriklad);

            this.CommandBindings.Add(VypocitejHandler);
            this.CommandBindings.Add(SmazHandler);
            this.CommandBindings.Add(PridejHandler);
            this.CommandBindings.Add(HistoryPrikladClickHandler);
        }
    }
}