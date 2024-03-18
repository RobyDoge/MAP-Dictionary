using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Path = System.IO.Path;

namespace DEX
{
    public partial class Page_Game : Page
    {
        private string[][] GameWord { set; get; }
        private int currentWordIndex = 0;
        private string[] GuessedWords { set; get; }

        public Page_Game(string[][] gameWords)
        {
            InitializeComponent();
            GameWord = gameWords;
            GuessedWords = new string[5];
            StartRound();
        }

        private void StartRound()
        {
            if (ChooseMeaningOrImage())
                ImageSelected();
            else MeaningSelected();
            TB_Guess.Text= "insert a guess";

        }

        private void MeaningSelected()
        {
            TB_Game.Opacity = 1;
            ImageGame.Opacity = 0;
            TB_Game.Text = GameWord[currentWordIndex][1];
        }

        private void ImageSelected()
        {
            TB_Game.Opacity = 0;
            ImageGame.Opacity = 1;
            var absolutePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, GameWord[currentWordIndex][2]);
            var bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri(absolutePath);
            bitmap.EndInit();
            ImageGame.Source = bitmap;

        }

        private bool ChooseMeaningOrImage()
        {
            if (GameWord[currentWordIndex].Length == 2)
            {
                return false;
            }

            var random = new Random();
            return random.Next(2) == 0;
        }

        private void BT_Previous_Click(object sender, RoutedEventArgs e)
        {
            currentWordIndex--;
            if(currentWordIndex==0)
                BT_Previous.IsEnabled = false;
            StartRound();
        }

        private void BT_Next_Click(object sender, RoutedEventArgs e)
        {
            currentWordIndex++;
            switch (currentWordIndex)
            {
                case 5:
                    MessageBox.Show($"Game Over\nYou have accumulated {FinalScore()} points");
                    NavigationService?.GoBack();
                    return;
                case 4:
                    BT_Next.Content="Finish";
                    break;
            }
            if(currentWordIndex>0)
                BT_Previous.IsEnabled = true;
            StartRound();
        }

        private int FinalScore()
        {
            var score = 0;
            for (var i = 0; i < 5; i++)
            {
                if (GameWord[i][0] == GuessedWords[i])
                    score++;
            }
            return score;
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (TB_Guess.Text is null or "insert a guess")
                return;
            if (GuessedWords is null)
                return;
            GuessedWords[currentWordIndex] = TB_Guess.Text;
        }

        private void TB_Guess_GotFocus(object sender, RoutedEventArgs e)
        {
            if("insert a guess"==TB_Guess.Text)
                TB_Guess.Text = "";
        }
    }
}

