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
using System.ComponentModel;

namespace GuessTheSong
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DataBaseHandler dbHandler;

        public MainWindow()
        {
            InitializeComponent();

            string DatabaseName = ConfigurationManager.AppSettings["DatabaseName"];
            string Password = ConfigurationManager.AppSettings["Password"];
            string Server = ConfigurationManager.AppSettings["Server"];
            string User = ConfigurationManager.AppSettings["User"];

            dbHandler = new DataBaseHandler(Server, DatabaseName, Password, User);
            List<Artist> artistsList = dbHandler.GetArtists();
            List<Genre> genresList = dbHandler.GetGenres();
            MainWindowVM vm = new MainWindowVM(artistsList, genresList);
            DataContext = vm;
        }


        private void ComboBoxSelection_artist(object sender, RoutedEventArgs e)
        {
            ComboBox cmb = sender as ComboBox;
            ((MainWindowVM)DataContext).SelectedArtist = Int32.Parse(cmb.SelectedValue.ToString());

        }

        private void ComboBoxSelection_genre(object sender, RoutedEventArgs e)
        {
            ComboBox cmb = sender as ComboBox;
            ((MainWindowVM)DataContext).SelectedGenre = Int32.Parse(cmb.SelectedValue.ToString());
        }

        private void Submit_btn(object sender, RoutedEventArgs e)
        {
            DateTime? selectedDate = DateOfBirth.SelectedDate;
            string firstName = FirstNameUI.Text;
            string lastName = LastNameUI.Text;
            int genreID = ((MainWindowVM)DataContext).SelectedGenre;
            int artistID = ((MainWindowVM)DataContext).SelectedArtist;

            dbHandler.SaveUserData(firstName, lastName, selectedDate, genreID, artistID);

            GameWindow gameWin = new GameWindow(new User() { FirstName = firstName, LastName = lastName, ArtistId=artistID, GenreId=genreID }, dbHandler);
            this.Visibility = Visibility.Hidden;
            gameWin.Owner = this;
            gameWin.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            gameWin.ShowDialog();
        }
    }

    public class ArtistVM
    {
        public string Desc { get; set; }
        public int Id { get; set; }

        public ArtistVM() { }

        public ArtistVM(Artist artist)
        {
            Desc = artist.Desc;
            Id = artist.Id;
        }

        public override string ToString()
        {
            return Desc;
        }
    }

    public class GenreVM
    {
        public string Desc { get; set; }
        public int Id { get; set; }

        public GenreVM() { }

        public GenreVM(Genre genre)
        {
            Desc = genre.Desc;
            Id = genre.Id;
        }

        public override string ToString()
        {
            return Desc;
        }
    }

    public class MainWindowVM : INotifyPropertyChanged
    {
        public MainWindowVM(List<Artist> artistsList, List<Genre> genresList)
        {
            IList<ArtistVM> artistVMlist = new List<ArtistVM>();
            IList<GenreVM> genreVMlist = new List<GenreVM>();
            foreach (Artist e in artistsList)
            {
                artistVMlist.Add(new ArtistVM(e));
            }
            foreach (Genre e in genresList)
            {
                genreVMlist.Add(new GenreVM(e));
            }
            _genresComboBoxLut = new CollectionView(genreVMlist);
            _artistComboBoxLut = new CollectionView(artistVMlist);
        }

        private readonly CollectionView _genresComboBoxLut;
        public CollectionView GenresComboBoxLut
        {
            get { return _genresComboBoxLut; }
        }

        public CollectionView _artistComboBoxLut;
        public CollectionView ArtistComboBoxLut
        {
            get { return _artistComboBoxLut; }
        }

        public int SelectedGenre { get; set; }
        public int SelectedArtist { get; set; }

        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}

