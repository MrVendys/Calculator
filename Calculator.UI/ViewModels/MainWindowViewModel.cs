using System.Collections.ObjectModel;
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
        private PrikladValidation _prikladValidation;
        public MainWindowViewModel()
        {
            _counting = new Counting();
            _prikladValidation = new PrikladValidation(_counting);
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
            if (_prikladValidation.TrySmazSymbol(Priklad))
                Priklad = Priklad.Remove(Priklad.Length - 1);
        }

        /// <summary>
        /// Handler <see cref="CalculatorCommands.PridejSymbolCommand"/>
        /// </summary>
        public void PridejSymbol(string parameter)
        {
            Priklad += _prikladValidation.TryPridejSymbol(parameter) ? parameter : null;
        }

        /// <summary>
        /// Handler <see cref="CalculatorCommands.OnHistoryPrikladClickCommand"/>
        /// </summary>
        public void VratPriklad(object sender, ExecutedRoutedEventArgs e)
        {
            SpocitanyPriklad sPriklad = (SpocitanyPriklad)e.Parameter;
            Priklad = sPriklad.Priklad;
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
