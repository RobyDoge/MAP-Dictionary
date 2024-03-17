using System.Windows;
using System.Windows.Controls;

namespace DEX
{
    public partial class CategorySelector : Window
    {
        public string? SelectedCategory { get; private set; }

        public CategorySelector(IEnumerable<string> categories)
        {
            InitializeComponent();
            list.ItemsSource = categories;
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectedCategory = list.SelectedItem.ToString();
            Hide();
        }
    }
}
