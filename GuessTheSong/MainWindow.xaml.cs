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
using GuessTheSongServer.DB;
//using DateTime;
//using GuessTheSongServer.DB.DataBaseHandler;

namespace GuessTheSong
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DateTime? selectedDate = DateOfBirth.SelectedDate;
            string firstName = FirstNameUI.Text;
            string lastName = LastNameUI.Text;
            string genre = GenreUI.Text;
            string artist = ArtistUI.Text; ;
            DBActions.SaveUserData(firstName, lastName, selectedDate, genre, artist);  
 
            GameWindow multiPlayer = new GameWindow();
            this.Visibility = Visibility.Hidden;
            multiPlayer.Owner = this;
            multiPlayer.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            multiPlayer.ShowDialog();
        }

    }
}
