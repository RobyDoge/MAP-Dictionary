using System.Windows;
using System.Windows.Controls;
using DBHandlers;

namespace DEX
{
    /// <summary>
    /// Interaction logic for Window_ModifyWord.xaml
    /// </summary>
    public partial class Window_ModifyWord : Window
    {
        private string InitialWord { get; set; }
        private List<string> wordInfo = ["","","",""];
        private WordsDBHandler DbHandler { get; set; }
        public Window_ModifyWord(WordsDBHandler dbHandler)
        {
            this.DbHandler = dbHandler;
            InitializeComponent();
            AddCategories();
        }

        private void AddCategories()
        {
            var categories = DbHandler.GetCategories();
            foreach (var category in categories)
            {
                CB_Category.Items.Add(category);
            }
        }

        private void TB_OnGotFocus(object sender, RoutedEventArgs e)
        {
            var aux = (TextBox)sender;
            aux.Text = "";
        }

        private void TB_Word_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TB_Word.Text))
            {
            }

            if (!DbHandler.WordExists(TB_Word.Text)) return;

            CB_IsFound.IsChecked = true; 
            wordInfo = DbHandler.GetWordInfo(TB_Word.Text);
            TB_Meaning.Text = wordInfo[2];
            TB_NewWord.Text = wordInfo[0];
            InitialWord = wordInfo[0];
            TB_Category.Text   = wordInfo[1];
            
        }

        private void TB_NewWord_TextChanged(object sender, TextChangedEventArgs e)
        {
            wordInfo[0] = TB_NewWord.Text.ToLower();
        }

        private void TB_Category_TextChanged(object sender, TextChangedEventArgs e)
        {
            wordInfo[1]= TB_Category.Text.ToLower();
        }

        private void CB_Category_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            wordInfo[1] = CB_Category.SelectedItem.ToString();
            TB_Category.Text= wordInfo[1].ToLower();
        }

        private void TB_Meaning_TextChanged(object sender, TextChangedEventArgs e)
        {
            wordInfo[2] = TB_Meaning.Text.ToLower();
        }

        private void BT_ModifyImage_Click(object sender, RoutedEventArgs e)
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
            wordInfo[3] = relativePath;
        }

        private void BT_Save_Click(object sender, RoutedEventArgs e)
        {
            if (DbHandler.ModifyWord(InitialWord, wordInfo))
                MessageBox.Show("Word modified successfully");
            else
                MessageBox.Show("Word not found");
        }
    }
}
