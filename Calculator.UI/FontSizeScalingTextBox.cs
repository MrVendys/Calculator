using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;

namespace Calculator.UI
{
    internal class FontSizeScalingTextBox : TextBox
    {
        private const int _MINFONTSIZE = 15;
        private const int _MAXFONTSIZE = 30;

        private FormattedText? _formattedText;

        public FontSizeScalingTextBox()
        {
            this.SizeChanged += (s, e) => ZmenFontSize();
            this.TextChanged += (s, e) => ZmenFontSize();
        }

        public bool TryZmenFontSize(string symbol)
        {
            double textActualWidth = GetTextActualWidth(symbol);
            int textBoxActualWidth = GetTextBoxActualWidth();

            if (FontSize <= _MINFONTSIZE && textActualWidth > textBoxActualWidth)
                return false;

            return true;
        }


        private void ZmenFontSize()
        {
            double textActualWidth = GetTextActualWidth();
            int textBoxActualWidth = GetTextBoxActualWidth();

            if (textActualWidth > textBoxActualWidth)
            {
                FontSize = ZmensujFontSize(FontSize, textActualWidth, textBoxActualWidth);
            }
            else
            {
                FontSize = ZvetsujFontSize(FontSize, textActualWidth, textBoxActualWidth);
            }
        }

        /// <summary>
        /// Postupné zmenšování <paramref name="fontSize"/>, dokud se text velikostně vejde do <paramref name="textBoxActualWidth"/>, nebo je dosažen <see cref="_MINFONTSIZE"/>
        /// </summary>
        /// <param name="textActualWidth">Počítán metodou <see cref="GetTextActualWidth(string)"/></param>
        /// <param name="textBoxActualWidth">Počítán metodou <see cref="GetTextBoxActualWidth()"/></param>
        private double ZmensujFontSize(double fontSize, double textActualWidth, int textBoxActualWidth)
        {
            while (textActualWidth > textBoxActualWidth)
            {
                if (fontSize <= _MINFONTSIZE)
                {
                    return fontSize;
                }

                fontSize--;
                _formattedText!.SetFontSize(fontSize);
                textActualWidth = GetTextActualWidth();
            }

            return fontSize;
        }

        /// <summary>
        /// Postupné zvětšování <paramref name="fontSize"/>, dokud se text velikostně vejde do <paramref name="textBoxActualWidth"/>, nebo je dosažen <see cref="_MAXFONTSIZE"/> 
        /// </summary>
        /// <param name="textActualWidth">Počítán metodou <see cref="GetTextActualWidth(string)"/></param>
        /// <param name="textBoxActualWidth">Počítán metodou <see cref="GetTextBoxActualWidth()"/></param>
        private double ZvetsujFontSize(double fontSize, double textActualWidth, int textBoxActualWidth)
        {
            while (textActualWidth < textBoxActualWidth)
            {
                if (fontSize >= _MAXFONTSIZE)
                {
                    return fontSize;
                }

                fontSize++;
                _formattedText!.SetFontSize(fontSize);
                textActualWidth = GetTextActualWidth();
            }

            //While skončí, jakmile je Text VĚTŠÍ, než TextBox. Vrátí poslední změnu, aby se text vešel.
            return --fontSize;
        }

        #region Helper

        private double GetTextActualWidth(string symbol = "")
        {
            if (_formattedText == null || _formattedText.Text != Text + symbol)
                SetFormattedText(symbol);

            return _formattedText!.Width + _formattedText.OverhangTrailing + _formattedText.OverhangLeading;
        }

        private int GetTextBoxActualWidth()
        {
            return (int)(RenderSize.Width - (Padding.Left + Padding.Right) - (BorderThickness.Left + BorderThickness.Right));
        }

        private void SetFormattedText(string symbol)
        {
            _formattedText = new FormattedText(
                Text + symbol,
                System.Globalization.CultureInfo.CurrentCulture,
                FlowDirection.LeftToRight,
                new Typeface(FontFamily, FontStyle, FontWeight, FontStretch),
                FontSize,
                Brushes.Black,
                VisualTreeHelper.GetDpi(this).PixelsPerDip
            );
        }

        #endregion

    }
}
