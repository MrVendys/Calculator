using System.Windows;
using System.Windows.Controls;
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
            CommandBinding vypocitejHandler = new CommandBinding(CalculatorCommands.OdesliPrikladCommand, _viewModel.Vypocitej);
            CommandBinding smazHandler = new CommandBinding(CalculatorCommands.SmazSymbolCommand, _viewModel.SmazSymbol);
            CommandBinding smazAllHandler = new CommandBinding(CalculatorCommands.SmazAllSymbolyCommand, _viewModel.SmazAllSymboly);
            CommandBinding pridejHandler = new CommandBinding(CalculatorCommands.PridejSymbolCommand, _viewModel.PridejSymbol);
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

        private void InputTextBlock_TextChanged(object sender, TextChangedEventArgs e)
        {
            var textBlock = InputTextBlock;
            double dostupnyWidth = textBlock.ActualWidth - (textBlock.Padding.Left + textBlock.Padding.Right) - 10;
            double fontSize = textBlock.FontSize;


            FormattedText formattedText = new FormattedText(
                textBlock.Text.ToString(),
                System.Globalization.CultureInfo.CurrentCulture,
                FlowDirection.LeftToRight,
                new Typeface(textBlock.FontFamily, textBlock.FontStyle, textBlock.FontWeight, textBlock.FontStretch),
                fontSize,
                Brushes.Black,
                VisualTreeHelper.GetDpi(textBlock).PixelsPerDip);

            if (textBlock.FontSize == 30 && formattedText.Width < dostupnyWidth)
            {
                return;
            }
            //Zavolá se znova, protože textChanged on Text.Remove
            if (textBlock.FontSize == 15 && formattedText.Width > dostupnyWidth)
            {
                textBlock.Text = textBlock.Text.Remove(textBlock.Text.Length - 1);
                return;
            }


            bool potrebujeResize = true;
            bool zmensit = formattedText.Width > dostupnyWidth;
            while (potrebujeResize)
            {
                if (zmensit)
                {
                    potrebujeResize = formattedText.Width > dostupnyWidth;
                    if (potrebujeResize && fontSize > 15)
                    {
                        fontSize -= 0.5;
                        formattedText.SetFontSize(fontSize);
                        continue;
                    }
                    if (potrebujeResize)
                        textBlock.Text = textBlock.Text.Remove(textBlock.Text.Length - 1);
                    break;
                }
                else
                {
                    potrebujeResize = formattedText.Width < dostupnyWidth;
                    if (potrebujeResize && fontSize < 30)
                    {
                        fontSize += 0.5;
                        formattedText.SetFontSize(fontSize);
                        continue;
                    }
                    break;
                }
            }

            textBlock.FontSize = fontSize;
        }
    }
}