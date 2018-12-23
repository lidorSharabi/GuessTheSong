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
        public GameWindow()
        {
            InitializeComponent();
            this.timer = new DispatcherTimer();
            this.timer.Tick += timer_Tick;
            this.timer.Interval = new System.TimeSpan(0, 0, 1);
            this.timer.Start();
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
