using System.Windows;
using System.Windows.Controls;

using DBHandlers;

namespace DEX
{
    public partial class WD_Login : Window
    {
        public WD_Login()
        {
            InitializeComponent();
        }

        private void TB_Password_OnLostFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            if (string.IsNullOrWhiteSpace(textBox.Text))
            {
                textBox.Text = "insert password";
            }
        }

        private void TB_Password_OnGotFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            textBox.Text = "";
        }

        private void TB_Username_OnLostFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            if (string.IsNullOrWhiteSpace(textBox.Text))
            {
                textBox.Text = "insert username";
            }
        }

        private void TB_Username_OnGotFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            textBox.Text = "";
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var username = (string)TB_Username.Text;
            var password = (string)TB_Password.Text;
            if (UserDBHandler.CheckUsernameAndPassword(username, password))
                WrongEntry();
            CorrectEntry();
        }

        private void CorrectEntry()
        {
            Hide();
            //send signal to main window to chnage the  page to administration
        }

        private void WrongEntry()
        {
            TBL_WrongEntry.IsEnabled = true;
        }


    }
}
