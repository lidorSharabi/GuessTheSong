using GuessTheSongServer.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        public GameWindow(string userName, DataBaseHandler dbHandler)
        {
            InitializeComponent();
            this.lives = 5;
            this.level = 1;
            this.levelTime = 30;
            UserNameTxtb.Text = userName;
            ScoreTxtb.Text = this.score.ToString();
            StartTimer();
            this.dbHandler = dbHandler;
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
                    if(lives > 0)
                    {
                        NextQ();
                    }
                }
                this.time = this.time.Add(TimeSpan.FromSeconds(-1));
            }, Application.Current.Dispatcher);
            this.timer.Start();
        }

        private void UpdateLives()
        {
            this.lives--;
            panel.Children.RemoveAt(0);
            if (this.lives == 0)
            {
               
                //update score in table
                this.dbHandler.SaveUserScore(this.score);
                //open new page - game over + scores

              

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

        private async void Choose_Answer(object sender, RoutedEventArgs e)
        {
            Button selectedAnswer = ((Button)sender);
            string answer = selectedAnswer.Content.ToString();
            //TODO - check whether the answer is right or worng
            bool isRight = true;
            //if answer is right, set text color to green
            if (isRight)
            {
                selectedAnswer.Foreground = new SolidColorBrush(Colors.Green);
                this.score += 10;
                this.timer.Stop();
            }
            else
            {
                //else answer is worng, set text color to red
                selectedAnswer.Foreground = new SolidColorBrush(Colors.Red);
                this.timer.Stop();
                UpdateLives();
                if (lives == 0)
                {
                    return;
                }
                //wait for one second and then display next question
            }
            ScoreTxtb.Text = this.score.ToString();
            EndLevel();
            await Task.Run(() => NextQ());
            //NextQ();
        }

        internal void NextQ()
        {
            Thread.Sleep(1000);
            StartTimer();
            //await Task.Run(() => StartTimer());
            Dispatcher.Invoke(new Action(() =>
            {
                answ1.Foreground = new SolidColorBrush(Colors.Black);
                answ2.Foreground = new SolidColorBrush(Colors.Black);
                answ3.Foreground = new SolidColorBrush(Colors.Black);
                answ4.Foreground = new SolidColorBrush(Colors.Black);
            }));
            
        }
        //private void StartNewLevel()
        //{
        //    //TODO - new query and answers

        //    StartTimer();
        //}
        private void timer_Tick(object sender, System.EventArgs e)
        {
            //this.pb.Value = System.DateTime.Now.Second % 100;
        }

        private void Remez1_Click(object sender, RoutedEventArgs e)
        {
            remez1_btn.IsEnabled = false;
            remez1_brd.Background = new SolidColorBrush(Colors.Gray);
            //remez1_btn.TextBlock.
        }
        private void Remez2_Click(object sender, RoutedEventArgs e)
        {
            remez2_btn.IsEnabled = false;
            remez2_brd.Background = new SolidColorBrush(Colors.Gray);
        }
        private void Remez3_Click(object sender, RoutedEventArgs e)
        {
            remez3_btn.IsEnabled = false;
            remez3_brd.Background = new SolidColorBrush(Colors.Gray);
        }
    }
}
