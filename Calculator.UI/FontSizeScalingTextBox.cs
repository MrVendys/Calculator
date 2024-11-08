using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;

namespace Calculator.UI
{
    internal class FontSizeScalingTextBox : TextBox
    {
        private const int _MINFONTSIZE = 15;
        private const int _MAXFONTSIZE = 30;

        private FormattedText formattedText;
        private int _textBoxActualWidth;
        private double _textActualWidth;

        public FontSizeScalingTextBox()
        {
            this.SizeChanged += (s, e) => ZmenFontSize();
            this.TextChanged += (s, e) => ZmenFontSize();
        }

        public bool TryZmenFontSize(string symbol)
        {
            SetPromenne(symbol);

            if (FontSize <= _MINFONTSIZE && _textActualWidth > _textBoxActualWidth)
                return false;

            return true;
        }

        private void ZmenFontSize()
        {
            SetPromenne();

            if (_textActualWidth > _textBoxActualWidth)
            {
                FontSize = ZmensujFontSize(FontSize);
            }
            else
            {
                FontSize = ZvetsujFontSize(FontSize);
            }
        }

        /// <summary>
        /// Postupné zmenšování <paramref name="fontSize"/>, dokud se text velikostně vejde do <see cref="_textBoxActualWidth"/>, nebo je dosažen <see cref="_MINFONTSIZE"/>
        /// </summary>
        private double ZmensujFontSize(double fontSize)
        {
            while (_textActualWidth > _textBoxActualWidth)
            {
                if (fontSize <= _MINFONTSIZE)
                {
                    return fontSize;
                }
                fontSize--;
                formattedText.SetFontSize(fontSize);
                VypocitejTextActualWidth();
            }
            return fontSize;
        }

        /// <summary>
        /// Postupné zvětšování <paramref name="fontSize"/>, dokud se text velikostně vejde do <see cref="_textBoxActualWidth"/>, nebo je dosažen <see cref="_MAXFONTSIZE"/> 
        /// </summary>
        private double ZvetsujFontSize(double fontSize)
        {
            while (_textActualWidth < _textBoxActualWidth)
            {
                if (fontSize >= _MAXFONTSIZE)
                {
                    return fontSize;
                }
                fontSize++;
                formattedText.SetFontSize(fontSize);
                VypocitejTextActualWidth();
            }
            //Vrácení poslední změny. While skončí, jakmile je Text VĚTŠÍ, než TextBox. Vrátí poslední změnu, aby se text vešel.
            fontSize--;
            return fontSize;
        }

        /// <summary>
        /// Nastaví proměnné <see cref="formattedText"/>, <see cref="_textBoxActualWidth"/> a <see cref="_textActualWidth"/>
        /// </summary>
        private void SetPromenne(string symbol = "")
        {
            formattedText = new FormattedText(
                Text.ToString() + symbol,
                System.Globalization.CultureInfo.CurrentCulture,
                FlowDirection.LeftToRight,
                new Typeface(FontFamily, FontStyle, FontWeight, FontStretch),
                FontSize,
                Brushes.Black,
                VisualTreeHelper.GetDpi(this).PixelsPerDip);

            _textBoxActualWidth = (int)(RenderSize.Width - (Padding.Left + Padding.Right) - (BorderThickness.Left + BorderThickness.Right));
            VypocitejTextActualWidth();
        }

        private void VypocitejTextActualWidth()
        {
            _textActualWidth = formattedText.Width + formattedText.OverhangTrailing + formattedText.OverhangLeading;
        }
    }
}
