using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Calculation;
using Calculation.Exceptions;

namespace Calculator.ViewModels
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

        public ObservableCollection<SpocitanyPriklad> HistoriePrikladu => _counting.HistoriePrikladu;

        /// <summary>
        /// Po chycení vyjímky se uživateli zobrazí MessageBox s chybovou hláškou.
        /// </summary>
        private void ZobrazHlasku(string chyba)
        {
            MessageBox.Show(chyba ?? "Neidentifikovatelná chyba", "Error", MessageBoxButton.OK, MessageBoxImage.Warning, MessageBoxResult.OK);
        }

        /// <summary>
        /// Použití výpočetního jádra <see cref="_counting"/> pro výpočet příkladu
        /// Chytá výjimky: InputValidationException
        /// </summary>
        public void Vypocitej(object target, ExecutedRoutedEventArgs e)
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

        public void SmazSymbol(object target, ExecutedRoutedEventArgs e)
        {
            if(Priklad != "")
                Priklad = Priklad.Remove(Priklad.Length - 1);
        }

        public void PridejSymbol(object target, ExecutedRoutedEventArgs e)
        {
            Button b = (Button)e.Source;
            Priklad += b.Content.ToString();
        }

        /// <summary>
        /// Handler vracející příklad po kliknutí na něj v historii.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void VratPriklad(object sender, ExecutedRoutedEventArgs e)
        {
            SpocitanyPriklad sPriklad = (SpocitanyPriklad)e.Parameter;
            Priklad = sPriklad.Priklad;
        }
    }
}
