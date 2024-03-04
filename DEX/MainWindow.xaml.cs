
using System.Windows;
using System.Windows.Controls;

using DBHandlers;

namespace DEX
{
    
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            ShowPage1();
        }

        private void ShowPage1()
        {
            Page1 page1 = new();
            contentFrame.Content = page1;
        }
    }
    

}