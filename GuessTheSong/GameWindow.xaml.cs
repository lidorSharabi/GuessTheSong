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
        private int difficulty = 1;
        private int score = 0;
        private DataBaseHandler dbHandler;
        private QuestionsManager qm;
        private List<Record> questionPool;
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
            qm = new QuestionsManager(user.ArtistId, user.GenreId, user.Year, dbHandler);
            questionPool = qm.AskQuestion(difficulty);
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
                    NextQuestion();
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
            List<Record> pool = questionPool.Where(q => q.Valid && q.Correctness).ToList();
            Record selectedSong = new Record();
            List<string> fourAnswers = new List<string>();
            if (questionPool.Count >= 4 && pool.Count > 0 ) // because we need at least 4 answers and possible good question
            {
                //set question as irrelevant
                questionPool.Where(q => q.Valid && q.Correctness).ToList()[0].Valid = false;
                selectedSong = pool[0];
                //get distinct wrong Answers list shuffled
                List<string> wrongAnswers = questionPool.Where(q => q.Song.SongName != selectedSong.Song.SongName).OrderBy(q => Guid.NewGuid()).Select(q => q.Song.SongName).Distinct().ToList();
                fourAnswers.Add(selectedSong.Song.SongName);
                fourAnswers.Add(wrongAnswers[0]);
                fourAnswers.Add(wrongAnswers[1]);
                fourAnswers.Add(wrongAnswers[2]);
                //shuffle the four options for answer
                fourAnswers = fourAnswers.OrderBy(a => Guid.NewGuid()).ToList();

                int start = NthIndexOf(selectedSong.Song.Lyrics, ' ', 3);
                start = start == selectedSong.Song.Lyrics.Length ? 0 : start;
                int end = NthIndexOf(selectedSong.Song.Lyrics, ' ', 10);

                currentQuestion.RightAnswer = selectedSong.Song.SongName;
                currentQuestion.Lyrics = selectedSong.Song.Lyrics.Substring(start, end - start);
                currentQuestion.Answer1 = fourAnswers[0];
                currentQuestion.Answer2 = fourAnswers[1];
                currentQuestion.Answer3 = fourAnswers[2];
                currentQuestion.Answer4 = fourAnswers[3];

                Dispatcher.Invoke(new Action(() =>
                {
                    ReSetUINewQuestion(currentQuestion);
                }));
            }
            else if (difficulty != 4)
            {
                //choose new questions from harder level
                questionPool = qm.AskQuestion(++difficulty);
                raffledNextQuestion();
            }
            else OpenScoreWindow(); //means there is no more questions - Game ended
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
            if (panel.Children.Count > 0)
                panel.Children.RemoveAt(0);
            if (this.lives == 0)
            {
                //update score in table
                this.dbHandler.SaveUserScore(this.score);
                //open new page - game over + scores
                OpenScoreWindow();
            }
        }

        private void OpenScoreWindow()
        {
            try
            {
                ScoresWindow scoresWin = new ScoresWindow(dbHandler);
                scoresWin.Owner = this;
                scoresWin.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                this.Visibility = Visibility.Hidden;
                scoresWin.ShowDialog();
                this.Close();
            }
            catch (Exception ex) { }
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
                    int count = 0;
                    if (currentQuestion.RightAnswer != answ1.Content.ToString() && count < 2)
                    {
                        answ1.Foreground = new SolidColorBrush(Colors.Red);
                        count++;
                    }
                    if (currentQuestion.RightAnswer != answ2.Content.ToString() && count < 2)
                    {
                        answ2.Foreground = new SolidColorBrush(Colors.Red);
                        count++;
                    }
                    if (currentQuestion.RightAnswer != answ3.Content.ToString() && count < 2)
                    {
                        answ3.Foreground = new SolidColorBrush(Colors.Red);
                        count++;
                    }
                    if (currentQuestion.RightAnswer != answ4.Content.ToString() && count < 2)
                    {
                        answ4.Foreground = new SolidColorBrush(Colors.Red);
                        count++;
                    }
                    break;
                case "remez2_btn":
                    remez2_btn.IsEnabled = false;
                    remez2_brd.Background = new SolidColorBrush(Colors.Gray);
                    NextQuestion();
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
