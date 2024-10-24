using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using Calculator.Core;
using Calculator.Core.Exceptions;

namespace Calculator.UI.ViewModels
{
    internal class MainWindowViewModel : ViewModelBase
    {
        /// <summary>
        /// Výpočetní jádro
        /// </summary>
        private Counting _counting;

        internal MainWindowViewModel()
        {
            _counting = new Counting();
        }

        internal Counting Counting => _counting;

        /// <summary>
        /// Pro vizualizaci příkladu zadaným uživatelem, nebo spočítaného výsledku
        /// </summary>
        public string Priklad
        {
            get
            {
                return _counting.Priklad; 
            }
            set {}
        }

        /// <summary>
        /// Pro vizualizaci Historie počítání
        /// </summary>
        public ObservableCollection<SpocitanyPriklad> HistoriePrikladu => _counting.HistoriePrikladu;

        /// <summary>
        /// Handler <see cref="CalculatorCommands.SmazSymbolCommand"/>. <br/>
        /// Použití výpočetního jádra <see cref="_counting"/> pro výpočet příkladu.
        /// Odchytávání vyjímek vzniklých při výpočtu
        /// </summary>
        internal void Vypocitej(object sender, ExecutedRoutedEventArgs e)
        {
            try
            {
                _counting.Vypocitej();
                OnPropertyChanged(nameof(Priklad));
            }
            catch (InputValidationException en) //Exeption en
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
            else
            {
                ZobrazHlasku("Nelze smazat poslední symbol");
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
            else
            {
                ZobrazHlasku("Nelze přidat symbol");
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
            else
            {
                ZobrazHlasku("Nelze načíst příklad z historie");
            }
        }

        /// <summary>
        /// Zobrazení uživateli MessageBox s chybovou hláškou.
        /// </summary>
        private void ZobrazHlasku(string chyba = "Neidentifikovatelná chyba")
        {
            MessageBox.Show(chyba, "Error", MessageBoxButton.OK, MessageBoxImage.Warning, MessageBoxResult.OK);
        }
    }
}
