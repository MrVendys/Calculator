using Calculator.Exceptions;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Calculator.ViewModels
{
    internal class MainWindowViewModel : ViewModelBase
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

        /// <summary>
        /// Výpočet příkladu, volaný z View pomocí RoutedCommand: <see cref="MainWindow.MainWindow"/>
        /// Chytá vyjíkmy: InputValidationException
        /// </summary>
        public void Vypocitej(object target, ExecutedRoutedEventArgs e)
        {
            try
            {
                if(Priklad != "")
                {
                    Priklad = _counting.Pocitej(Priklad);
                }
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
        /// Zobrazení MessageBox s chybovou hláškou.
        /// </summary>
        private void ZobrazHlasku(string chyba)
        {
            MessageBox.Show(chyba ?? "Neidentifikovatelná chyba", "Error", MessageBoxButton.OK, MessageBoxImage.Warning, MessageBoxResult.OK);
        }
    }
}
