using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
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
            CommandBinding pridejHandler = new CommandBinding(CalculatorCommands.PridejSymbolCommand, _viewModel.PridejSymbol);
            CommandBinding historyPrikladClickHandler = new CommandBinding(CalculatorCommands.OnHistoryPrikladClickCommand, _viewModel.VratPriklad);

            this.CommandBindings.Add(vypocitejHandler);
            this.CommandBindings.Add(smazHandler);
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
                _viewModel.PridejSymbol(e.Text);
            }
        }

        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Decimal)
            {
                _carkaHandled = true;
                _viewModel.PridejSymbol(_viewModel.DesetinnyOddelovac);
            }
        }

        internal bool DecreaseFontSize(string symbol = null)
        {
            var label = InputLabel;
            double dostupnyWidth = label.ActualWidth - (label.Padding.Left + label.Padding.Right) - 10;
            double fontSize = label.FontSize;

            FormattedText formattedText = new FormattedText(
                label.Content.ToString() + symbol,
                System.Globalization.CultureInfo.CurrentCulture,
                FlowDirection.LeftToRight,
                new Typeface(label.FontFamily, label.FontStyle, label.FontWeight, label.FontStretch),
                fontSize,
                Brushes.Black,
                VisualTreeHelper.GetDpi(label).PixelsPerDip);

            bool isShrinking = formattedText.Width > dostupnyWidth;
            bool withinLimit = false;

            while (!withinLimit && fontSize >= 15)
            {
                if (isShrinking && fontSize > 15)
                {
                    fontSize -= 0.5;
                    formattedText.SetFontSize(fontSize);
                    isShrinking = formattedText.Width > dostupnyWidth;
                }
                else if(fontSize <= 15)
                {
                    withinLimit = true;
                    return false; //Nejde přidat symbol
                }
                else
                {
                    withinLimit = true; //Nepotřebuje zmenšit
                }
            }

            label.FontSize = fontSize;
            return true;
        }

        internal void IncreaseFontSize()
        {
            var label = InputLabel;
            double dostupnyWidth = label.ActualWidth - (label.Padding.Left + label.Padding.Right) - 10;
            double fontSize = label.FontSize;

            FormattedText formattedText = new FormattedText(
                label.Content.ToString(),
                System.Globalization.CultureInfo.CurrentCulture,
                FlowDirection.LeftToRight,
                new Typeface(label.FontFamily, label.FontStyle, label.FontWeight, label.FontStretch),
                fontSize,
                Brushes.Black,
                VisualTreeHelper.GetDpi(label).PixelsPerDip);

            bool isShrinking = formattedText.Width > dostupnyWidth;
            bool withinLimit = false;

            while (!withinLimit && fontSize <= 30)
            {
                if (!isShrinking && fontSize < 30)
                {
                    fontSize += 0.5;
                    formattedText.SetFontSize(fontSize);
                    isShrinking = formattedText.Width < dostupnyWidth;
                }
                else
                {
                    withinLimit = true;
                }

                formattedText.SetFontSize(fontSize);
            }

            label.FontSize = fontSize;
        }
    }
}