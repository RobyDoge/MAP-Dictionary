using DBHandlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Printing;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace DEX
{
    /// <summary>
    /// Interaction logic for CategorySelector.xaml
    /// </summary>
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
