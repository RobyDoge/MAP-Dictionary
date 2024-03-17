using DBHandlers;
using System.Windows.Controls;

namespace DEX
{
    /// <summary>
    /// Interaction logic for Page_Admin.xaml
    /// </summary>
    public partial class Page_Admin : Page
    {
        private WordsDBHandler DbHandler { set; get; }
        public Page_Admin(WordsDBHandler dbHandler)
        {
            InitializeComponent();
            this.DbHandler = dbHandler;
        }

        private void BT_AddWord_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            WD_AddWord wdAddWord = new(DbHandler);
            wdAddWord.Show();
        }

        private void BT_EditWord_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Window_ModifyWord wdModifyWord = new(DbHandler);
            wdModifyWord.Show();
        }

        private void BT_DeleteWord_Click(object sender, System.Windows.RoutedEventArgs e)
        {

        }
    }
}
