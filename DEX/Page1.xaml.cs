﻿using DBHandlers;
using System.Windows;
using System.Windows.Controls;

namespace DEX
{
    /// <summary>
    /// Interaction logic for Page1.xaml
    /// </summary>
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
            throw new NotImplementedException();
            ;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            const int maxItemsShown = 30;
            const int textHeight = 20;
            LB_CategorySelector.ItemsSource = wordsDBHandler.GetCategories();
            LB_CategorySelector.Height = textHeight * Math.Min(maxItemsShown, wordsDBHandler.GetCategories().Count);
        }

        private void LB_CategorySelector_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            currentCategory = (string)LB_CategorySelector.SelectedItem;
            LB_CategorySelector.ItemsSource = null;
        }
    }
}
