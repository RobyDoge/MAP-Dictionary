using DBHandlers;
using System.IO;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace DEX
{
    public partial class Page_WordDescription : Page
    {
        private string Word { get; set; }
        private string Meaning { get; set; }
        private string ImagePath { get; set; }

        public Page_WordDescription(KeyValuePair<string, MeaningAndImage> wordPair)
        {
            InitializeComponent();
            Word = wordPair.Key;
            Meaning = wordPair.Value.Meaning;
            ImagePath = wordPair.Value.ImagePath;
            TB_Word.Text = Word;
            TB_WordDefinition.Text = Meaning;
            var absolutePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ImagePath);
            var bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri(absolutePath);
            bitmap.EndInit();
            IM_Image.Source = bitmap;



        }
    }
}
