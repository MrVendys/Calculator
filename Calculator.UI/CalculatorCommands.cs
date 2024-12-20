﻿using System.Windows.Input;

namespace Calculator.UI
{
    public static class CalculatorCommands
    {
        public static readonly RoutedUICommand OdesliPrikladCommand = new RoutedUICommand("=", nameof(OdesliPrikladCommand), typeof(CalculatorCommands),
            new InputGestureCollection
            {
                new KeyGesture(Key.Enter)
            });
        public static readonly RoutedUICommand SmazSymbolCommand = new RoutedUICommand("⌫", nameof(SmazSymbolCommand), typeof(CalculatorCommands),
            new InputGestureCollection
            {
                new KeyGesture(Key.Back)
            });
        public static readonly RoutedUICommand SmazAllSymbolyCommand = new RoutedUICommand("C", nameof(SmazAllSymbolyCommand), typeof(CalculatorCommands),
            new InputGestureCollection
            {
                new KeyGesture(Key.Escape),
                new KeyGesture(Key.Delete)
            });
        public static readonly RoutedUICommand PridejSymbolCommand = new RoutedUICommand("", nameof(PridejSymbolCommand), typeof(CalculatorCommands));
        public static readonly RoutedCommand OnHistoryPrikladClickCommand = new RoutedCommand(nameof(OnHistoryPrikladClickCommand), typeof(CalculatorCommands));
        public static readonly RoutedCommand ClipboardCopy = new RoutedCommand(nameof(ClipboardCopy), typeof(CalculatorCommands), new InputGestureCollection
            {
                new KeyGesture(Key.C, ModifierKeys.Control),
            });
        public static readonly RoutedCommand ClipboardPaste = new RoutedCommand(nameof(ClipboardPaste), typeof(CalculatorCommands), new InputGestureCollection
            {
                new KeyGesture(Key.V, ModifierKeys.Control),
            });
    }
}
