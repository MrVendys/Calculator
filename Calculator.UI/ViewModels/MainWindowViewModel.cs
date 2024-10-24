using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using Calculator.Core;
using Calculator.Core.Exceptions;

namespace Calculator.UI.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private Counting _counting;
        public MainWindowViewModel()
        {
            _counting = new Counting();
        }

        /// <summary>
        /// Pro vizualizaci příkladu zadaným uživatelem, nebo spočítaného výsledku
        /// Bind k Textboxu <see cref="MainWindow.InputTextbox"/>
        /// </summary>
        public string Priklad
        {
            get
            {
                return _counting.Priklad; 
            }
            set
            { 
            } 
        }

        internal Counting Counting => _counting;

        public ObservableCollection<SpocitanyPriklad> HistoriePrikladu => _counting.HistoriePrikladu;

        /// <summary>
        /// Handler <see cref="CalculatorCommands.SmazSymbolCommand"/>. <br/>
        /// Použití výpočetního jádra <see cref="_counting"/> pro výpočet příkladu.
        /// Chytá výjimky: InputValidationException
        /// </summary>
        internal void Vypocitej(object sender, ExecutedRoutedEventArgs e)
        {
            try
            {
                _counting.Pocitej();
                OnPropertyChanged(nameof(Priklad));
            }
            catch (InputValidationException en)
            {
                ZobrazHlasku(en.Message);
            }
        }

        /// <summary>
        /// Handler <see cref="CalculatorCommands.SmazSymbolCommand"/>
        /// </summary>
        internal void SmazSymbol(object sender, ExecutedRoutedEventArgs e)
        {
            if (_counting.TryDeleteSymbol(Priklad))
            {
                OnPropertyChanged(nameof(Priklad));
            }
        }

        /// <summary>
        /// Handler <see cref="CalculatorCommands.PridejSymbolCommand"/>
        /// </summary>
        internal void PridejSymbol(string parameter)
        {
            if (_counting.TryAddSymbol(parameter))
            {
                OnPropertyChanged(nameof(Priklad));
            }
        }

        /// <summary>
        /// Handler <see cref="CalculatorCommands.OnHistoryPrikladClickCommand"/>
        /// </summary>
        internal void VratPriklad(object sender, ExecutedRoutedEventArgs e)
        {
            SpocitanyPriklad sPriklad = (SpocitanyPriklad)e.Parameter;
            if (_counting.TryVratPriklad(sPriklad))
            {
                OnPropertyChanged(nameof(Priklad));
            }
            
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
