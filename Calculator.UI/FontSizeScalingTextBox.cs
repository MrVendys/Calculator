using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Calculator.UI
{
    internal class FontSizeScalingTextBox : TextBox
    {
        private const int _MINFONTSIZE = 15;
        private const int _MAXFONTSIZE = 30;

        private FormattedText? _formattedText;

        public FontSizeScalingTextBox()
        {
            SizeChanged += (s, e) => ZmenFontSize();
            TextChanged += (s, e) => ZmenFontSize();
        }

        #region Validace

        public bool TryZmenFontSize(string priklad)
        {
            double textActualWidth = GetTextActualWidth(priklad);
            int textBoxActualWidth = GetTextBoxActualWidth();

            bool valid = TryZmensujFontSize(FontSize, textActualWidth, textBoxActualWidth, priklad);
            return valid;
        }

        private bool TryZmensujFontSize(double fontSize, double textActualWidth, double textBoxActualWidth, string priklad)
        {
            while (textActualWidth > textBoxActualWidth)
            {
                fontSize--;
                if (fontSize < _MINFONTSIZE)
                {
                    _formattedText = null;
                    return false;
                }

                _formattedText!.SetFontSize(fontSize);
                textActualWidth = GetTextActualWidth(priklad);
            }

            _formattedText = null;
            return true;
        }

        #endregion Validace

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
                if (fontSize < _MINFONTSIZE)
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
