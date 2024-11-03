using System.Diagnostics;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;

namespace Calculator.UI
{
    internal class TextBlockHelper
    {
        public static readonly DependencyProperty MonitorOverflowProperty =
        DependencyProperty.RegisterAttached(
            "MonitorOverflow",
            typeof(bool),
            typeof(TextBlockHelper),
            new PropertyMetadata(false, OnMonitorOverflowChanged));

        public static bool GetMonitorOverflow(DependencyObject obj) => (bool)obj.GetValue(MonitorOverflowProperty);
        public static void SetMonitorOverflow(DependencyObject obj, bool value) => obj.SetValue(MonitorOverflowProperty, value);

        private static void OnMonitorOverflowChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is TextBlock textBlock && e.NewValue is true)
            {
                textBlock.SizeChanged += (sender, args) => CheckOverflow(textBlock);
            }
        }

        private static void CheckOverflow(TextBlock textBlock)
        {
            double dostupnyWidth = textBlock.ActualWidth - (textBlock.Padding.Left + textBlock.Padding.Right) - 10;
            double fontSize = textBlock.FontSize;


            FormattedText formattedText = new FormattedText(
                textBlock.Text.ToString(),
                System.Globalization.CultureInfo.CurrentCulture,
                FlowDirection.LeftToRight,
                new Typeface(textBlock.FontFamily, textBlock.FontStyle, textBlock.FontWeight, textBlock.FontStretch),
                fontSize,
                Brushes.Black,
                VisualTreeHelper.GetDpi(textBlock).PixelsPerDip);

            if (textBlock.FontSize == 30 && formattedText.Width < dostupnyWidth)
                return;
            if (textBlock.FontSize == 15 && formattedText.Width > dostupnyWidth)
                return;


            bool potrebujeResize = true;
            bool zmensit = formattedText.Width > dostupnyWidth;
            while (potrebujeResize)
            {
                if (zmensit)
                {
                    potrebujeResize = formattedText.Width > dostupnyWidth;
                    if (potrebujeResize && fontSize > 15)
                    {
                        fontSize -= 0.5;
                        formattedText.SetFontSize(fontSize);
                        continue;
                    }
                    if (potrebujeResize && textBlock.Text != "")
                        textBlock.Text = textBlock.Text.Remove(textBlock.Text.Length - 1);
                    break;
                }
                else
                {
                    potrebujeResize = formattedText.Width < dostupnyWidth;
                    if (potrebujeResize && fontSize < 30)
                    {
                        fontSize += 0.5;
                        formattedText.SetFontSize(fontSize);
                        continue;
                    }
                    break;
                }
            }

            textBlock.FontSize = fontSize;
        }
    }
}

