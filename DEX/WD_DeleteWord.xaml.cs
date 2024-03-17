using System.Windows;
using System.Windows.Controls;
using DBHandlers;

namespace DEX
{
    /// <summary>
    /// Interaction logic for WD_DeleteWord.xaml
    /// </summary>
    public partial class WD_DeleteWord : Window
    {
        WordsDBHandler  DbHandler { get; set; }
        private string Word { get; set; }
        private string Category { get; set; }
        public WD_DeleteWord(WordsDBHandler dbHandler)
        {
            this.DbHandler = dbHandler;
            Category= "";
            InitializeComponent();
            LoadCategories();
        }

        private void LoadCategories()
        {
            var categories = DbHandler.GetCategories();
            foreach (var category in categories)
            {
                CB_Category.Items.Add(category);
            }
        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            TB_Word.Text= "";
        }

        private void TB_Word_TextChanged(object sender, TextChangedEventArgs e)
        {
            var prefix = TB_Word.Text.ToLower();
            if (prefix == "") return;

            var similarWords = Category == ""
                ? DbHandler.GetSimilarWordsWithoutCategory(prefix)
                : DbHandler.GetSimilarWordsWithCategory(prefix, Category);
            if (LB_Search is null) return;
            LB_Search.IsEnabled = true;
            LB_Search.ItemsSource = similarWords;
        }

        private void CB_Category_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Category= CB_Category.SelectedItem.ToString();
        }

        private void LB_Search_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Word = LB_Search.SelectedItem.ToString();
            TB_WordDelete.Text=Word;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (DbHandler.DeleteWord(Word))
            {
                MessageBox.Show("Word deleted successfully");
                Close();
            }
            else
            {
                MessageBox.Show("Word not found");
            }
          
        }
    }
}
