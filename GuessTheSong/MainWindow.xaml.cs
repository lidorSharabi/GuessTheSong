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
using GuessTheSongServer.DM;
using System.Configuration;

namespace GuessTheSong
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DataBaseHandler dbHandler;
        List<Artist> artistsList;
        List<Genre> genresList;

        public MainWindow()
        {
            InitializeComponent();

            string DatabaseName = ConfigurationManager.AppSettings["DatabaseName"];
            string Password = ConfigurationManager.AppSettings["Password"];
            string Server = ConfigurationManager.AppSettings["Server"];
            string User = ConfigurationManager.AppSettings["User"];

            dbHandler = new DataBaseHandler(Server, DatabaseName, Password, User);

            artistsList = dbHandler.GetArtists();

            genresList = dbHandler.GetGenres();

            autoComGenre.LostFocus += (sender, e) =>
            {
                var border = (autoComGenreResultStack.Parent as ScrollViewer).Parent as Border;
                border.Visibility = System.Windows.Visibility.Collapsed;
            };
            autoComArtist.LostFocus += (sender, e) =>
            {
                var border = (autoComArtistResultStack.Parent as ScrollViewer).Parent as Border;
                border.Visibility = System.Windows.Visibility.Collapsed;

            };
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DateTime? selectedDate = DateOfBirth.SelectedDate;
            string firstName = FirstNameUI.Text;
            string lastName = LastNameUI.Text;
            //string genre = GenreUI.Text;
            int genreID = 5;
            //string artist = ArtistUI.Text; ;
            int artistID = 6;

            dbHandler.SaveUserData(firstName, lastName, selectedDate, genreID, artistID);
            string genre = autoComGenre.Text;
            string artist = autoComArtist.Text; ;
            //DBActions.SaveUserData(firstName, lastName, selectedDate, genre, artist);

            GameWindow multiPlayer = new GameWindow();
            this.Visibility = Visibility.Hidden;
            multiPlayer.Owner = this;
            multiPlayer.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            multiPlayer.ShowDialog();
        }

        private void TextBox_KeyUp(object sender, KeyEventArgs e)
        {
            bool found = false;
            var data = new List<string>();
            string autoComName = (sender as TextBox).Name;
            StackPanel resultStack;
            resultStack = autoComArtistResultStack;
            if (autoComName.Equals("autoComArtist"))
            {
                resultStack = autoComArtistResultStack;
                artistsList.ForEach((item) => data.Add(item.Desc));
            }
            else if (autoComName.Equals("autoComGenre"))
            {
                resultStack = autoComGenreResultStack;
                genresList.ForEach((item) => data.Add(item.Desc));
            }
            var border = (resultStack.Parent as ScrollViewer).Parent as Border;

            //var data = Model.GetData();

            string query = (sender as TextBox).Text;

            if (query.Length == 0)
            {
                // Clear   
                resultStack.Children.Clear();
                border.Visibility = System.Windows.Visibility.Collapsed;
            }
            else
            {
                border.Visibility = System.Windows.Visibility.Visible;
            }

            // Clear the list   
            resultStack.Children.Clear();

            // Add the result   
            foreach (var obj in data)
            {
                if (obj.ToLower().StartsWith(query.ToLower()))
                {
                    // The word starts with this... Autocomplete must work   
                    addItem(obj, autoComName, border, resultStack);
                    found = true;
                }
            }

            if (!found)
            {
                resultStack.Children.Add(new TextBlock() { Text = "No results found." });
            }
        }

        private void addItem(string text, string autoComName, Border border, StackPanel resultStack)
        {
            TextBlock block = new TextBlock();

            // Add the text   
            block.Text = text;

            // A little style...   
            block.Margin = new Thickness(2, 3, 2, 3);
            block.Cursor = Cursors.Hand;

            // Mouse events   
            block.MouseLeftButtonUp += (sender, e) =>
            {
                if (autoComName.Equals("autoComArtist"))
                {
                    autoComArtist.Text = (sender as TextBlock).Text;
                }
                else if (autoComName.Equals("autoComGenre"))
                {
                    autoComGenre.Text = (sender as TextBlock).Text;
                }
                border.Visibility = System.Windows.Visibility.Collapsed;
            };

            block.MouseEnter += (sender, e) =>
            {
                TextBlock b = sender as TextBlock;
                b.Background = Brushes.PeachPuff;
            };

            block.MouseLeave += (sender, e) =>
            {
                TextBlock b = sender as TextBlock;
                b.Background = Brushes.Transparent;
            };


            // Add to the panel   
            resultStack.Children.Add(block);
        }

        class Model
        {
            static public List<string> GetData()
            {
                List<string> data = new List<string>();

                data.Add("Afzaal");
                data.Add("Ahmad");
                data.Add("Zeeshan");
                data.Add("Daniyal");
                data.Add("Rizwan");
                data.Add("John");
                data.Add("Doe");
                data.Add("Johanna Doe");
                data.Add("Pakistan");
                data.Add("Microsoft");
                data.Add("Programming");
                data.Add("Visual Studio");
                data.Add("Sofiya");
                data.Add("Rihanna");
                data.Add("Eminem");

                return data;
            }
        }
    }
}
