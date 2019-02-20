using GuessTheSongServer.DB;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Shapes;

namespace GuessTheSong
{
    /// <summary>
    /// Interaction logic for ScoresWindow.xaml
    /// </summary>
    public partial class ScoresWindow : Window
    {
        public class Score
        {
            public string name { get; set; }
            public int score { get; set; }
        }

        private DataBaseHandler dbHandler;
        public ScoresWindow(DataBaseHandler dbHandler)
        {
            this.dbHandler = dbHandler;
            InitializeComponent();

            var scoresList = new ObservableCollection<Score>();
            //TODO - bring from db by ascending order
            scoresList.Add(new Score() { name = "hh", score = 30 });
            scoresList.Add(new Score() { name = "ht", score = 1500 });
            scoresList.Add(new Score() { name = "h565", score = 130 });
            scoresList.Add(new Score() { name = "hhodaya", score = 70 });
            scoresList.Add(new Score() { name = "hhdfkkd", score = 0 });
            scoresList.Add(new Score() { name = "hhodaya", score = 70 });
            scoresList.Add(new Score() { name = "hhodaya", score = 70 });
            scoresList.Add(new Score() { name = "hhodaya", score = 70 });
            scoresList.Add(new Score() { name = "hhodaya", score = 70 });
            scoresList.Add(new Score() { name = "hhodaya", score = 70 });
            dataGrid1.ItemsSource = scoresList;
        }
    }
}
