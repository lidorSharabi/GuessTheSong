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
        public GameWindow()
        {
            InitializeComponent();
            StartTimer();
            this.lives = 5;

            //var webImage = new BitmapImage(new Uri("../images/heart.png", UriKind.Relative));
            //var imageControl = new Image();
            //imageControl.Source = webImage;
            //panel.Children.Add(imageControl);
           
        }

        private void StartTimer()
        {
            this.time = TimeSpan.FromSeconds(30);
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
            panel.Children.RemoveAt(5 - this.lives);
            this.lives--;  
        }

        private void Choose_Answer(object sender, RoutedEventArgs e)
        {
            string answer = ((Button)sender).Content.ToString();
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
