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
    /// Interaction logic for Page_WordDescription.xaml
    /// </summary>
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
            if (ImagePath == "tba")
            {
                IM_Image.Source = new BitmapImage(new Uri("https://via.placeholder.com/150"));
            }
            else
            {
                string absolutePath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ImagePath);
                var bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(absolutePath);
                bitmap.EndInit();
                IM_Image.Source = bitmap;
            }


        }
    }
}
