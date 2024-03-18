using DBHandlers;
using System.Windows;
using System.Windows.Controls;

namespace DEX
{
    public partial class Page1 : Page
    {
        private string currentCategory = "default";
        private WordsDBHandler wordsDBHandler;
        public Page1()
        {
            this.wordsDBHandler = new WordsDBHandler("../../../WordsDB.xml");
            InitializeComponent();
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

            var similarWords = currentCategory == "default" ? wordsDBHandler.GetSimilarWordsWithoutCategory(prefix) : wordsDBHandler.GetSimilarWordsWithCategory(prefix, currentCategory);
            if (LB_SearchBarResults is null) return;

            const int maxItemsShown = 30;
            const int textHeight = 20;
            LB_SearchBarResults.ItemsSource = similarWords;
            LB_SearchBarResults.Height = textHeight * Math.Min(maxItemsShown, similarWords.Count);
        }


        private void LB_SearchBarResults_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (LB_SearchBarResults.SelectedItem is null) return;
            Page_WordDescription pageWordDescription = new(wordsDBHandler.GetWord(LB_SearchBarResults.SelectedItem.ToString()));
            NavigationService?.Navigate(pageWordDescription);
        }

        private void BT_CategorySelector_Click(object sender, RoutedEventArgs e)
        {
            CategorySelector choiceWindow = new(wordsDBHandler.GetCategories());
            choiceWindow.ShowDialog();

            currentCategory = choiceWindow.SelectedCategory ?? "default";
        }

        private void BT_Login_OnClick(object sender, RoutedEventArgs e)
        {
            WD_Login loginWindow = new();
            loginWindow.LoginSuccess += LoginWindow_LoginSuccess;
            loginWindow.ShowDialog();


        }
        private void LoginWindow_LoginSuccess(object sender, EventArgs e)
        {
            ((WD_Login)sender).Close();
            Page_Admin pageAdmin = new(wordsDBHandler);
            NavigationService?.Navigate(pageAdmin);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string[][] gameWords = wordsDBHandler.GetWordsForGame();
            Page_Game pageGame = new(gameWords);
            NavigationService?.Navigate(pageGame);
        }
    }
}
