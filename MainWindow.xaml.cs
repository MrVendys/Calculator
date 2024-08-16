using Calculator.Strategies;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
namespace Calculator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<string> characters = new List<string>() {
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
        //Vytvoreni tlacitek do kalkulacky
        private void InitializeButtons()
        {
            foreach (var item in characters)
            {
                Button b = new Button()
                {
                    Name = "ContentButton",
                    Height = 60,
                    Width = 60,
                    Content = item,
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
        //Prepsani hodnoty tlacitka do textboxu
        private void CustomButton_Click(object sender, RoutedEventArgs e)
        {
            Button senderButton = (Button)sender;
            InputTextbox.Text += senderButton.Content.ToString();
        }
        //Vytvoreni instance Counting pro vypocet
        private void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            /*string expression = InputTextbox.Text.ToString();
            if (!expression.Equals(""))
            {
                Counting c = new Counting()
                float? result = c.Count(expression);
                if(result != null)    
                    InputTextbox.Text = c.Count(expression).ToString();
                else{
                    string messageBoxText = "Nekompletní výraz. Zkontrolujte výraz a zadejte ho znovu.";
                    string caption = "ERROR";
                    MessageBoxButton button = MessageBoxButton.Ok;
                    MessageBoxImage icon = MessageBoxImage.Warning;
                    MessageBoxResult result;
                    result = MessageBox.Show(messageBoxText, caption, button, icon, MessageBoxResult.Ok);
                    
                }
            }
            */
            List<(string item1, double item2)> mathExamplesWithResults = new List<(string, double)>
        {
            ("√(36 / 4) + 7", 10),    
            ("(2 + 5) ^ 2 / 7", 7), 
            ("3! + √9 * 4", 18), 
            ("8 * (5 - 3) + 6",22), 
            ("2 ^ √16 + 1", 17),
            ("√49 - 3 * 2", 1), 
            ("15 / (3 + 2) ^ 2", 0.6), 
            ("(4!) / 2 - 1", 11),
            ("7 * (2 + √9)", 35), 
            ("(2 + 2)! + 5 - 3", 26), 
            ("√81 / (3 + 6)", 1), 
            ("5 ^ 2 - √25", 20), 
            ("(2 + 6) * 3 - 5!", -96), 
            ("4! / (2! + 2!)",6) 
        };
            for (int i = 0; i < mathExamplesWithResults.Count(); i++)
            {
                Counting c = new Counting();
                Trace.WriteLine(i + " " + c.Count(mathExamplesWithResults[i].item1) + " = " + mathExamplesWithResults[i].item2);
            }
            
        }
        //Odstraneni posledniho symbolu
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