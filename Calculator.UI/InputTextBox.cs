using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;
namespace Calculator.UI
{
    internal class InputTextBox : TextBox
    {
        public InputTextBox()
        {
            this.SizeChanged += (s, e) => ZmenFontSize();
            this.TextChanged += (s, e) => ZmenFontSize();
        }

        private void ZmenFontSize()
        {
            double dostupnyWidth = ActualWidth - (Padding.Left + Padding.Right);
            double fontSize = FontSize;

            FormattedText formattedText = new FormattedText(
                Text.ToString(),
                System.Globalization.CultureInfo.CurrentCulture,
                FlowDirection.LeftToRight,
                new Typeface(FontFamily, FontStyle, FontWeight, FontStretch),
                fontSize,
                Brushes.Black,
                VisualTreeHelper.GetDpi(this).PixelsPerDip);

            if (FontSize == 30 && formattedText.Width < dostupnyWidth)
                return;

            if (FontSize == 15 && formattedText.Width > dostupnyWidth)
                return;

            bool potrebujeResize = true;
            bool zmensit = formattedText.Width + 2 > dostupnyWidth;
            while (potrebujeResize)
            {
                if (zmensit)
                {
                    potrebujeResize = formattedText.Width + 2 > dostupnyWidth;
                    if (potrebujeResize && fontSize > 15)
                    {
                        fontSize -= 0.5;
                        formattedText.SetFontSize(fontSize);
                        continue;
                    }
                    break;
                }
                else
                {
                    fontSize += 0.5;
                    formattedText.SetFontSize(fontSize);
                    potrebujeResize = Math.Round(formattedText.Width, 0) < Math.Round(dostupnyWidth, 0);
                    if (potrebujeResize && fontSize < 30)
                    {
                        continue;
                    }

                    fontSize -= 0.5;
                    break;
                }
            }
            FontSize = fontSize;
        }

        public bool TryZmenFontSize()
        {
            double dostupnyWidth = ActualWidth - (Padding.Left + Padding.Right);

            FormattedText formattedText = new FormattedText(
                Text.ToString(),
                System.Globalization.CultureInfo.CurrentCulture,
                FlowDirection.LeftToRight,
                new Typeface(FontFamily, FontStyle, FontWeight, FontStretch),
                FontSize,
                Brushes.Black,
                VisualTreeHelper.GetDpi(this).PixelsPerDip);

            if (FontSize <= 15 && formattedText.Width + 2 > dostupnyWidth)
                return false;
            return true;
        }
    }
}
