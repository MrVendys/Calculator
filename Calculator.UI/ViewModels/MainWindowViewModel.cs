using System.Collections.ObjectModel;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using Calculator.Core;
using Calculator.Core.Exceptions;

namespace Calculator.UI.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private string _priklad;
        private Counting _counting;

        public MainWindowViewModel()
        {
            _counting = new Counting();
        }

        /// <summary>
        /// Příklad zadaný uživatelem, nebo spočítaný výsledek
        /// Bind k Textboxu <see cref="MainWindow.InputTextbox"/>
        /// </summary>
        public string Priklad 
        {
            get
            {
                return _priklad;
            }
            set
            {
                _priklad = value;
                OnPropertyChanged(nameof(Priklad));
            }
        }

        internal Counting Counting => _counting;

        public ObservableCollection<SpocitanyPriklad> HistoriePrikladu => _counting.HistoriePrikladu;

        /// <summary>
        /// Handler <see cref="CalculatorCommands.SmazSymbolCommand"/>. <br/>
        /// Použití výpočetního jádra <see cref="_counting"/> pro výpočet příkladu.
        /// Chytá výjimky: InputValidationException
        /// </summary>
        public void Vypocitej(object sender, ExecutedRoutedEventArgs e)
        {
            try
            {
                var vysledek = _counting.Pocitej(Priklad);
                Priklad = vysledek;
            }
            catch (InputValidationException en)
            {
                ZobrazHlasku(en.Message);
            }
        }

        /// <summary>
        /// Handler <see cref="CalculatorCommands.SmazSymbolCommand"/>
        /// </summary>
        public void SmazSymbol(object sender, ExecutedRoutedEventArgs e)
        {
            if (Priklad != "")
                Priklad = Priklad.Remove(Priklad.Length - 1);
        }

        /// <summary>
        /// Handler <see cref="CalculatorCommands.PridejSymbolCommand"/>
        /// </summary>
        public void PridejSymbol(object sender, ExecutedRoutedEventArgs e)
        {
            UlozSymbol((string)e.Parameter);
        }

        /// <summary>
        /// Handler <see cref="CalculatorCommands.OnHistoryPrikladClickCommand"/>
        /// </summary>
        public void VratPriklad(object sender, ExecutedRoutedEventArgs e)
        {
            SpocitanyPriklad sPriklad = (SpocitanyPriklad)e.Parameter;
            Priklad = sPriklad.Priklad;
        }

        public void UlozSymbol(string symbol)
        {
            string pattern = "[-0-9()";
            string separator = System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;
            pattern += pattern.Contains(separator) ? null : separator;

            foreach (char znak in Counting.ZnakyOperaci)
            {
                if (znak == '-')
                    continue;
                else
                    pattern += znak.ToString();
            }
            pattern += "]";

            Regex regex = new Regex(pattern);
            if(regex.IsMatch(symbol))
                Priklad += symbol;
        }

        /// <summary>
        /// Po chycení vyjímky se uživateli zobrazí MessageBox s chybovou hláškou.
        /// </summary>
        private void ZobrazHlasku(string chyba = "Neidentifikovatelná chyba")
        {
            MessageBox.Show(chyba, "Error", MessageBoxButton.OK, MessageBoxImage.Warning, MessageBoxResult.OK);
        }
    }
}
