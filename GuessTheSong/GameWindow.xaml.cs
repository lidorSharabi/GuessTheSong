using GuessTheSongServer.DB;
using GuessTheSongServer.DM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace GuessTheSong
{
    /// <summary>
    /// Interaction logic for GameWindow.xaml
    /// </summary>
    public partial class GameWindow : Window
    {
        private DispatcherTimer timer;
        private TimeSpan time;
        private int lives;
        private int level;
        private int levelTime;
        private int score = 0;
        private DataBaseHandler dbHandler;
        private QuestionsManager qm;
        private List<Song> questionPool;
        private Question currentQuestion = new Question();
        public GameWindow(User user, DataBaseHandler dbHandler)
        {
            InitializeComponent();
            this.dbHandler = dbHandler;
            InitializeGameData(user);
        }

        private void InitializeGameData(User user)
        {
            this.lives = 5;
            this.level = 1;
            this.levelTime = 30;
            ScoreTxtb.Text = this.score.ToString();
            UserNameTxtb.Text = user.FirstName + " " + user.LastName;
            //TODO - use year, artist and genre id's
            qm = new QuestionsManager("dreamtheater", "Rock", 2000, dbHandler);
            questionPool = qm.AskQuestion();
            raffledNextQuestion();
            StartTimer();
        }

        private void StartTimer()
        {
            this.time = TimeSpan.FromSeconds(this.levelTime);
            this.timer = new DispatcherTimer(new TimeSpan(0, 0, 1), DispatcherPriority.Normal, delegate
            {
                CountDown.Text = this.time.Seconds.ToString();
                ProgressBar.Value = this.time.Seconds;
                if (this.time == TimeSpan.Zero)
                {
                    this.timer.Stop();
                    UpdateLives();
                    EndLevel();
                    if (lives > 0)
                    {
                        NextQuestion();
                    }
                }
                this.time = this.time.Add(TimeSpan.FromSeconds(-1));
            }, Application.Current.Dispatcher);
            this.timer.Start();
        }

        private async void Choose_Answer(object sender, RoutedEventArgs e)
        {
            Button selectedAnswer = ((Button)sender);
            string answer = selectedAnswer.Content.ToString();
            //if answer is right, set text color to green
            if (answer.Equals(currentQuestion.RightAnswer))
            {
                selectedAnswer.Foreground = new SolidColorBrush(Colors.Green);
                this.score += 10;
                this.timer.Stop();
            }
            else
            {
                //else answer is wrong, set text color to red
                selectedAnswer.Foreground = new SolidColorBrush(Colors.Red);
                this.timer.Stop();
                UpdateLives();
                if (lives == 0)
                {
                    ScoresWindow ScoreWin = new ScoresWindow(dbHandler);
                    this.Visibility = Visibility.Hidden;
                    ScoreWin.Owner = this;
                    ScoreWin.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                    ScoreWin.ShowDialog();
                }
            }
            ScoreTxtb.Text = this.score.ToString();
            EndLevel();
            await Task.Run(() => NextQuestion());
        }

        internal void NextQuestion()
        {
            Thread.Sleep(1000);
            StartTimer();
            raffledNextQuestion();
        }

        private void raffledNextQuestion()
        {
            List<Song> pool = questionPool.Where(q => q.Valid && q.Correctness).ToList();
            pool[0].Valid = false;
            Song selectedSong = pool[0];
            List<string> fourAnswers = new List<string>();
            if (pool.Count >= 4) // because we need at least 4 answers
            {
                //get distinct wrong Answers list shuffled
                List<string> wrongAnswers = questionPool.Where(q => q.SongName != selectedSong.SongName).OrderBy(q => Guid.NewGuid()).Select(q => q.SongName).Distinct().ToList();
                fourAnswers.Add(selectedSong.SongName);
                fourAnswers.Add(wrongAnswers[0]);
                fourAnswers.Add(wrongAnswers[1]);
                fourAnswers.Add(wrongAnswers[2]);
                //shuffle the four options for answer
                fourAnswers = fourAnswers.OrderBy(a => Guid.NewGuid()).ToList();

            }
            //TODO - if this is the hardest level (means there is no more questions, then and game)
            //TODO - choose new questions from harder level and call raffledNextQuestion again (with new questionPool)
            currentQuestion.RightAnswer = pool.Count >= 4 ? selectedSong.SongName : "selectedSong.SongName";
            currentQuestion.Lyrics = pool.Count >= 4 ? selectedSong.Lyrics.Substring(0, NthIndexOf(selectedSong.Lyrics, ' ', 8)) : "Lyrics";
            currentQuestion.Answer1 = pool.Count >= 4 ? fourAnswers[0] : "fourAnswers[0].SongName";
            currentQuestion.Answer2 = pool.Count >= 4 ? fourAnswers[1] : "fourAnswers[1].SongName";
            currentQuestion.Answer3 = pool.Count >= 4 ? fourAnswers[2] : "fourAnswers[2].SongName";
            currentQuestion.Answer4 = pool.Count >= 4 ? fourAnswers[3] : "fourAnswers[3].SongName";

            Dispatcher.Invoke(new Action(() =>
            {
                ReSetUINewQuestion(currentQuestion);
            }));
        }

        private void ReSetUINewQuestion(Question q)
        {
            answ1.Foreground = new SolidColorBrush(Colors.Black);
            answ2.Foreground = new SolidColorBrush(Colors.Black);
            answ3.Foreground = new SolidColorBrush(Colors.Black);
            answ4.Foreground = new SolidColorBrush(Colors.Black);
            LyricsTxtb.Text = q.Lyrics;
            answ1.Content = q.Answer1;
            answ2.Content = q.Answer2;
            answ3.Content = q.Answer3;
            answ4.Content = q.Answer4;
        }

        private void UpdateLives()
        {
            this.lives--;
            panel.Children.RemoveAt(0);
            if (this.lives == 0)
            {
                //update score in table
                this.dbHandler.SaveUserScore(this.score);
                //TODO open new page - game over + scores
                return;
            }
        }

        private void EndLevel()
        {
            this.level++;
            if (this.levelTime > 10)
            {
                this.levelTime = this.levelTime - 5;
            }
        }

        private void Remez_Click(object sender, RoutedEventArgs e)
        {
            Button remez_btn = (Button)sender;
            switch (remez_btn.Name)
            {
                case "remez1_btn":
                    remez1_btn.IsEnabled = false;
                    remez1_brd.Background = new SolidColorBrush(Colors.Gray);
                    break;
                case "remez2_btn":
                    remez2_btn.IsEnabled = false;
                    remez2_brd.Background = new SolidColorBrush(Colors.Gray);
                    break;
                case "remez3_btn":
                    remez3_btn.IsEnabled = false;
                    remez3_brd.Background = new SolidColorBrush(Colors.Gray);
                    break;

            }
        }

        public int NthIndexOf(string target, char value, int n)
        {
            int num = 0;
            int length = 0;
            
            while (num != n && length < target.Length)
            {
                if (target[length] == value)
                    num++;
                length++;
            }
            return length;
        }
    }
}
