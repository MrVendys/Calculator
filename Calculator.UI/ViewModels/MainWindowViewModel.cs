﻿using Calculator.Core;
using Calculator.Core.Exceptions;
using Calculator.UI.Views;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace Calculator.UI.ViewModels
{
    internal class MainWindowViewModel : ViewModelBase
    {
        private readonly Counting _counting;

        public MainWindowViewModel()
        {
            _counting = new Counting();
        }

        public string DesetinnyOddelovac => System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;

        /// <summary>
        /// Pro vizualizaci příkladu zadaným uživatelem, nebo spočítaného výsledku
        /// </summary>
        public string Priklad => _counting.Priklad;

        /// <summary>
        /// Pro vizualizaci Historie počítání
        /// </summary>
        public ObservableCollection<SpocitanyPriklad> HistoriePrikladu => _counting.HistoriePrikladu;

        /// <summary>
        /// Handler <see cref="CalculatorCommands.OdesliPrikladCommand"/>. <br/>
        /// Použití výpočetního jádra <see cref="_counting"/> pro výpočet příkladu.
        /// Odchytávání vyjímek vzniklých při výpočtu. <br/>
        /// Aktualizování vlastnosti <see cref="Priklad"/> při úspěšném smazání symbolu.
        /// </summary>
        public void Vypocitej(object sender, ExecutedRoutedEventArgs e)
        {
            try
            {
                _counting.Vypocitej();
                OnPropertyChanged(nameof(Priklad));
            }
            catch (InputValidationException en)
            {
                ZobrazHlasku(en.Message);
            }
        }

        /// <summary>
        /// Handler <see cref="CalculatorCommands.SmazSymbolCommand"/>. <br/>
        /// Aktualizování vlastnosti <see cref="Priklad"/> při úspěšném smazání symbolu.
        /// </summary>
        public void SmazSymbol(object sender, ExecutedRoutedEventArgs e)
        {
            if (_counting.TrySmazSymbol(Priklad))
            {
                OnPropertyChanged(nameof(Priklad));
                MainWindow mw = (MainWindow)sender;
                mw.IncreaseFontSize();
            }
        }

        /// <summary>
        /// Handler <see cref="CalculatorCommands.SmazAllSymbolyCommand"/>. <br/>
        /// Aktualizování vlastnosti <see cref="Priklad"/> při úspěšném smazání všech symbolů.
        /// </summary>
        public void SmazAllSymboly(object sender, ExecutedRoutedEventArgs e)
        {
            if (_counting.TrySmazAllSymboly(Priklad))
            {
                OnPropertyChanged(nameof(Priklad));
                MainWindow mw = (MainWindow)sender;
                mw.InputLabel.FontSize = 30;
            }
        }

        /// <summary>
        /// Handler <see cref="CalculatorCommands.PridejSymbolCommand"/>
        /// Aktualizování vlastnosti <see cref="Priklad"/> při úspěšném přidání symbolu.
        /// </summary>
        public void PridejSymbol(string parameter)
        {
            if (_counting.TryPridejSymbol(parameter))
            {
                OnPropertyChanged(nameof(Priklad));
            }
        }
        public void PridejSymbol(object sender, ExecutedRoutedEventArgs e)
        {
            MainWindow mw = (MainWindow)sender;
            if (mw.DecreaseFontSize((string)e.Parameter))
            {
                if (_counting.TryPridejSymbol((string)e.Parameter))
                {
                    OnPropertyChanged(nameof(Priklad));
                }
            }
        }

        /// <summary>
        /// Handler <see cref="CalculatorCommands.OnHistoryPrikladClickCommand"/>
        /// Aktualizování vlastnosti <see cref="Priklad"/> při úspěšném vrácení příkladu z historie.
        /// </summary>
        public void VratPriklad(object sender, ExecutedRoutedEventArgs e)
        {
            SpocitanyPriklad sPriklad = (SpocitanyPriklad)e.Parameter;
            if (_counting.TryVratPriklad(sPriklad))
            {
                OnPropertyChanged(nameof(Priklad));
                MainWindow mw = (MainWindow)sender;
                mw.DecreaseFontSize();
            }
            else
            {
                ZobrazHlasku("Chyba v programu. Nelze načíst příklad z historie");
            }
        }

        /// <summary>
        /// Zobrazení uživateli MessageBox s <paramref name="chyba"/>.
        /// </summary>
        private void ZobrazHlasku(string chyba = "Neidentifikovatelná chyba")
        {
            MessageBox.Show(chyba, "Error", MessageBoxButton.OK, MessageBoxImage.Warning, MessageBoxResult.OK);
        }
    }
}
