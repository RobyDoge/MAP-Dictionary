using DBHandlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DEX
{
    /// <summary>
    /// Interaction logic for Page1.xaml
    /// </summary>
    public partial class Page1 : Page
    {
        private WordsDBHandler wordsDBHandler;
        public Page1()
        {
            this.wordsDBHandler = new WordsDBHandler("../../../WordsDB.xml");
            InitializeComponent();
            WD_Login test = new();
            test.Show();
        }
        private void TB_SearchBar_OnLostFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            if (string.IsNullOrWhiteSpace(textBox.Text))
            {
                textBox.Text = "insert word";
            }
        }

        private void TB_SearchBar_OnGotFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            textBox.Text = "";
        }

        private void TB_SearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            var prefix = TB_SearchBar.Text.ToLower();
            if (prefix == "") return;

            var similarWords = wordsDBHandler.GetSimilarWords(prefix);
            if (LB_SearchBarResults is null) return;

            const int maxItemsShown = 30;
            const int textHeight = 20;
            LB_SearchBarResults.ItemsSource = similarWords;
            LB_SearchBarResults.Height = textHeight * Math.Min(maxItemsShown, similarWords.Count);
        }


        private void LB_SearchBarResults_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
