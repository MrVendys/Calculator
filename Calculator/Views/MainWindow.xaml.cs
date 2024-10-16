﻿using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using Calculator.ViewModels;

namespace Calculator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml.
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly MainWindowViewModel _viewModel;

        public MainWindow()
        {
            InitializeComponent();
            _viewModel = (MainWindowViewModel)DataContext;
            InitializeCommands();
        }

        private void InitializeCommands()
        {
            CommandBinding VypocitejHandler = new CommandBinding(CalculatorCommands.OdesliPrikladCommand, _viewModel.Vypocitej);
            CommandBinding SmazHandler = new CommandBinding(CalculatorCommands.SmazSymbolCommand, _viewModel.SmazSymbol);
            CommandBinding PridejHandler = new CommandBinding(CalculatorCommands.PridejSymbolCommand, _viewModel.PridejSymbol);
            CommandBinding HistoryPrikladClickHandler = new CommandBinding(CalculatorCommands.OnHistoryPrikladClickCommand, _viewModel.VratPriklad);

            this.CommandBindings.Add(VypocitejHandler);
            this.CommandBindings.Add(SmazHandler);
            this.CommandBindings.Add(PridejHandler);
            this.CommandBindings.Add(HistoryPrikladClickHandler);
        }

        private void InputTextbox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9+\\-*/.!^√]");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}