using Calculator.Commands;
using Calculator.Exceptions;
using System.ComponentModel;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Calculator.ViewModels
{
    internal class MainWindowViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// Příklad zadaný uživatelem, nebo spočítaný výsledek
        /// </summary>
        private string _priklad;

        public string Priklad { 
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

        private void OnPropertyChanged(string jmenoPromenne)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(jmenoPromenne));
        }

        public MainWindowViewModel() { }

        /// <summary>
        /// Výpočet příkladu, volaný z View pomocí RoutedCommand: <see cref="MainWindow.MainWindow"/>
        /// </summary>
        public void Vypocitej(object target, ExecutedRoutedEventArgs e)
        {
            try
            {
                Counting c = new Counting();
                Priklad = c.Pocitej(Priklad);
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
