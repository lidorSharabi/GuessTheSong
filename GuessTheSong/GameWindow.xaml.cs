﻿using System;
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
        public GameWindow()
        {
            InitializeComponent();
            this.lives = 5;
            this.level = 1;
            this.levelTime = 30;
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
                    TimeIsUp();
                }
                this.time = this.time.Add(TimeSpan.FromSeconds(-1));
            }, Application.Current.Dispatcher);
            this.timer.Start();
        }

        private void TimeIsUp()
        {
            if(this.lives == 0)
            {
                //TODO- GAME OVER
                return;
            }
            panel.Children.RemoveAt(0);
            this.lives--;
            //TODO - LOSS SCREEN
            EndLevel();
            if(this.lives > 0)
            {
                StartNewLevel();
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
            //if answer is right, set text color to green
            selectedAnswer.Foreground = new SolidColorBrush(Colors.Green);
            //else answer is worng, set text color to red
            selectedAnswer.Foreground = new SolidColorBrush(Colors.Red);
            //wait for one second and then display next question
            await Task.Run(() => NextQ(selectedAnswer));
        }
        internal void NextQ(Button selectedAnswer)
        {
            Thread.Sleep(1000);
            Dispatcher.Invoke(new Action(() =>
            {
                answ1.Foreground = new SolidColorBrush(Colors.Black);
                answ2.Foreground = new SolidColorBrush(Colors.Black);
                answ3.Foreground = new SolidColorBrush(Colors.Black);
                answ4.Foreground = new SolidColorBrush(Colors.Black);
            }));
        }
        private void StartNewLevel()
        {
            //TODO - new query and answers

            StartTimer();
        }
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
