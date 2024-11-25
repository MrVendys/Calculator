﻿using Calculator.Core;
using Calculator.Core.Exceptions;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

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
        /// Použití výpočetního jádra <see cref="Counting"/> pro výpočet příkladu.
        /// Odchytávání výjimek vzniklých při výpočtu. <br/>
        /// Aktualizování vlastnosti <see cref="Priklad"/> při úspěšném vypočítání příkladu.
        /// </summary>
        public void Vypocitej()
        {
            try
            {
                _counting.Vypocitej();
                OnPropertyChanged(nameof(Priklad));
            }
            catch (NeplatnyVstupException e)
            {
                ZobrazHlasku(e.ChybovyKod, e.Message);
            }
            catch (SpatnePouzitiException e)
            {
                ZobrazHlasku(e.ChybovyKod, e.Message);
            }
        }

        /// <summary>
        /// Handler <see cref="CalculatorCommands.SmazSymbolCommand"/>. <br/>
        /// Aktualizování vlastnosti <see cref="Priklad"/> při úspěšném smazání symbolu.
        /// </summary>
        public void SmazSymbol()
        {
            if (_counting.TrySmazSymbol())
            {
                OnPropertyChanged(nameof(Priklad));
            }
        }

        /// <summary>
        /// Handler <see cref="CalculatorCommands.SmazAllSymbolyCommand"/>. <br/>
        /// Aktualizování vlastnosti <see cref="Priklad"/> při úspěšném smazání všech symbolů.
        /// </summary>
        public void SmazPriklad()
        {
            if (_counting.TrySmazPriklad())
            {
                OnPropertyChanged(nameof(Priklad));
            }
        }

        /// <summary>
        /// Handler <see cref="CalculatorCommands.PridejSymbolCommand"/>
        /// Aktualizování vlastnosti <see cref="Priklad"/> při úspěšném přidání symbolu.
        /// </summary>
        public void PridejSymbol(string symbol)
        {
            if (_counting.TryPridejSymbol(symbol))
            {
                OnPropertyChanged(nameof(Priklad));
            }
        }

        /// Aktualizování vlastnosti <see cref="Priklad"/> při úspěšném vrácení příkladu z historie.
        /// <summary>
        public void VratPriklad(object sender, ExecutedRoutedEventArgs e)
        {
            SpocitanyPriklad spocitanyPriklad = (SpocitanyPriklad)e.Parameter;
            if (_counting.TryVratPriklad(spocitanyPriklad))
            {
                OnPropertyChanged(nameof(Priklad));
            }
            else
            {
                ZobrazHlasku(ChybovyKod.Neidentifikovano, "Chyba v programu. Nelze načíst příklad z historie");
            }
        }

        /// <summary>
        /// Zobrazení uživateli MessageBox s <paramref name="chyba"/>.
        /// </summary>
        private void ZobrazHlasku(ChybovyKod kod = ChybovyKod.Neidentifikovano, string chyba = "")
        {
            MessageBox.Show($"Nastala chyba (Kod {(int)kod}:{kod}). {chyba}", "Error", MessageBoxButton.OK, MessageBoxImage.Warning, MessageBoxResult.OK);
        }
    }
}
