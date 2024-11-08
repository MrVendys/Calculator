using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;

namespace Calculator.UI
{
    internal class FontSizeTextBox : TextBox
    {
        public FontSizeTextBox()
        {
            this.SizeChanged += (s, e) => ZmenFontSize();
            this.TextChanged += (s, e) => ZmenFontSize();
        }

        private void ZmenFontSize()
        {
            double dostupnyWidth = ActualWidth - (Padding.Left + Padding.Right) - (BorderThickness.Left + BorderThickness.Right);
            double fontSize = FontSize;

            FormattedText formattedText = new FormattedText(
                Text.ToString(),
                System.Globalization.CultureInfo.CurrentCulture,
                FlowDirection.LeftToRight,
                new Typeface(FontFamily, FontStyle, FontWeight, FontStretch),
                fontSize,
                Brushes.Black,
                VisualTreeHelper.GetDpi(this).PixelsPerDip);

            double formattedTextWidth = formattedText.Width + formattedText.OverhangTrailing + formattedText.OverhangLeading;

            if (FontSize == 30 && formattedTextWidth < dostupnyWidth)
                return;

            if (FontSize == 15 && formattedTextWidth > dostupnyWidth)
                return;

            bool potrebujeResize = true;
            bool prvniCyklus = true;
            bool zmensit = true;
            while (true)
            {
                formattedTextWidth = formattedText.Width + formattedText.OverhangTrailing + formattedText.OverhangLeading;
                potrebujeResize = formattedTextWidth > dostupnyWidth;

                if (prvniCyklus)
                {
                    zmensit = formattedTextWidth > dostupnyWidth;
                    prvniCyklus = false;
                }

                if (zmensit)
                {
                    if (potrebujeResize && fontSize > 15)
                    {
                        fontSize -= 1;
                        formattedText.SetFontSize(fontSize);
                        continue;
                    }
                    break;
                }
                else
                {
                    if (!potrebujeResize && fontSize < 30)
                    {
                        fontSize += 1;
                        formattedText.SetFontSize(fontSize);
                        continue;
                    }
                    fontSize -= 1;
                    break;
                }
            }
            FontSize = fontSize;
        }

        public bool TryZmenFontSize(string symbol)
        {
            double dostupnyWidth = RenderSize.Width - (Padding.Left + Padding.Right) - (BorderThickness.Left + BorderThickness.Right);

            FormattedText formattedText = new FormattedText(
                Text.ToString() + symbol,
                System.Globalization.CultureInfo.CurrentCulture,
                FlowDirection.LeftToRight,
                new Typeface(FontFamily, FontStyle, FontWeight, FontStretch),
                FontSize,
                Brushes.Black,
                VisualTreeHelper.GetDpi(this).PixelsPerDip);

            double formattedTextWidth = formattedText.Width + formattedText.OverhangLeading + formattedText.OverhangTrailing;

            if (FontSize <= 15 && formattedTextWidth > dostupnyWidth)
                return false;
            return true;
        }
    }
}
