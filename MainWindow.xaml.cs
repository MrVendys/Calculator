using System.Windows;
using System.Windows.Controls;
namespace Calculator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml.
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Charaktery pro generovani tlacitek.
        /// </summary>
        private List<string> _charakteryList = new List<string>() {
        "1",
        "2",
        "3",
        "4",
        "5",
        "6",
        "7",
        "8",
        "9",
        "0",
        ",",
        "+",
        "-",
        "*",
        "/",
        "^",
        "√",
        "!",
        "(",
        ")"
       };

        public MainWindow()
        {
            InitializeComponent();
            InitializeButtons();
        }

        /// <summary>
        /// Vytvoření tlačítek podle _characteryList a přidání do View na WrapPanel.
        /// </summary>
        private void InitializeButtons()
        {
            foreach (var charakter in _charakteryList)
            {
                Button b = new Button()
                {
                    Name = "ContentButton",
                    Height = 60,
                    Width = 60,
                    Content = charakter,
                    FontSize = 50,
                    VerticalContentAlignment = VerticalAlignment.Center,
                    HorizontalContentAlignment = HorizontalAlignment.Center,
                    Margin = new Thickness(1, 1, 1, 1),
                    Padding = new Thickness(-5, -5, -5, -5),
                };
                b.Click += CustomButton_Click;

                WrapPanel.Children.Add(b);
            }
        }

        /// <summary>
        /// Klik na vytvorene tlacitko.
        /// </summary>
        private void CustomButton_Click(object sender, RoutedEventArgs e)
        {
            Button senderButton = (Button)sender;
            InputTextbox.Text += senderButton.Content.ToString();
        }

        /// <summary>
        /// Klik na tlacitko "Vypocitat ( = )".
        /// </summary>
        private void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            string vyraz = InputTextbox.Text.ToString();
            if (!vyraz.Equals(""))
            {
                Counting c = new Counting();
                c.Chyba += OnChyba;

                double? result = c.Pocitej(vyraz);
                if (result.HasValue) 
                {
                    InputTextbox.Text = result.ToString();
                }
            }
            else
            {
                ZobrazHlasku("Nekompletní výraz. Zkontrolujte výraz a zadejte ho znovu.");
            }
        }

        /// <summary>
        /// Funkce, která se volá na Action: <see cref="Counting.Chyba"/>.
        /// Vyvolá funkci na zobratení chybové hlášky.
        /// </summary>
        /// <param name="chyba">Chybová hláška, která přijde skrz Action <see cref="Counting.Chyba"/></param>
        private void OnChyba(string chyba)
        {
            ZobrazHlasku(chyba);
        }

        /// <summary>
        /// Zobrazí MessageBox s chybovou hláškou.
        /// </summary>
        /// <param name="chyba"></param>
        private void ZobrazHlasku(string chyba)
        {
            string messageBoxText = chyba;
            string caption = "ERROR";
            MessageBoxButton button = MessageBoxButton.OK;
            MessageBoxImage icon = MessageBoxImage.Warning;
            MessageBox.Show(messageBoxText, caption, button, icon, MessageBoxResult.OK);
        }

        /// <summary>
        /// Klik na tlacitko "Odstranit ( ← )".
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (!InputTextbox.Text.Equals(""))
            {
                string text = InputTextbox.Text.Remove(InputTextbox.Text.Length - 1);
                InputTextbox.Text = text;
            }
        }

    }
}