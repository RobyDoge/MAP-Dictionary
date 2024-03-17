using DBHandlers;
using System.Windows;
using System.Windows.Controls;

namespace DEX
{
    public partial class WD_AddWord : Window
    {
        private WordsDBHandler DbHandler { set; get; }
        private string Category { set; get; }
        private string Word { set; get; }
        private string Definition { set; get; }
        private string ImaginePath { set; get; }

        public WD_AddWord(WordsDBHandler wordsDb)
        {
            this.DbHandler = wordsDb;
            InitializeComponent();
            LoadCategories();
            ImaginePath = "";
        }

        private void LoadCategories()
        {
            var categories = DbHandler.GetCategories();
            foreach (var category in categories)
            {
                CB_Category.Items.Add(category);
            }
        }

        private void TB_Word_TextChanged(object sender, TextChangedEventArgs e)
        {
            this.Word = TB_Word.Text;
        }

        private void TB_Category_TextChanged(object sender, TextChangedEventArgs e)
        {
            this.Category = TB_Category.Text;

        }
        private void CB_Category_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.Category = CB_Category.SelectedItem.ToString();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            this.Definition = TB_Meaning.Text;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            var openFileDialog = new Microsoft.Win32.OpenFileDialog
            {
                Filter = "Image Files (*.jpg;*.jpeg;*.png;*.gif)|*.jpg;*.jpeg;*.png;*.gif"
            };
            var result = openFileDialog.ShowDialog();

            if (result != true) return;

            var imagePath = openFileDialog.FileName;


            var imagesFolder = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Images");
            if (!System.IO.Directory.Exists(imagesFolder))
            {
                System.IO.Directory.CreateDirectory(imagesFolder);
            }

            var imageName = System.IO.Path.GetFileName(imagePath);
            var destinationPath = System.IO.Path.Combine(imagesFolder, imageName);
            System.IO.File.Copy(imagePath, destinationPath, true);

            var relativePath = System.IO.Path.GetRelativePath(AppDomain.CurrentDomain.BaseDirectory, destinationPath);
            ImaginePath = relativePath;
        }

        private void BT_AddWord_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(Word) || string.IsNullOrEmpty(Category) || string.IsNullOrEmpty(Definition))
            {
                MessageBox.Show("Please fill in all the fields");
                return;
            }

            if (DbHandler.AddWord(Word, Category, Definition, ImaginePath))
            {
                MessageBox.Show("Word added successfully");
                this.Close();
            }
            else
            {
                MessageBox.Show("An error occurred while adding the word");
            }

        }

        private void TB_Word_OnGotFocus(object sender, RoutedEventArgs e)
        {
            TB_Word.Text = "";
        }

        private void TB_Category_OnGotFocus(object sender, RoutedEventArgs e)
        {
            TB_Category.Text = "";
        }

        private void TB_Meaning_OnGotFocus(object sender, RoutedEventArgs e)
        {
            TB_Meaning.Text = "";
        }
    }
}
