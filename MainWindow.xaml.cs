﻿using Calculator.Strategies;
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
            string expression = InputTextbox.Text.ToString();
            if (!expression.Equals(""))
            {
                Counting c = new Counting();
                InputTextbox.Text = c.Count(expression).ToString();
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