using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FinalProject
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Engine engine;
        System.Windows.Forms.Timer tmr;
        System.Windows.Forms.Timer scoreTmr;
        System.Windows.Forms.Timer colTmr;
        int time = 4000;
        int counter = 0;
        int score = 0;
        bool isPaused = false;

        //Intiializes window, creates the engine object to control the game, and starts the timers for the events
        public MainWindow()
        {
            InitializeComponent();
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            engine = new Engine(playField, Dispatcher);

            engine.spawn();
            tmr = new System.Windows.Forms.Timer();
            tmr.Interval = time;
            tmr.Tick += new EventHandler(spawn);
            tmr.Start();

            scoreTmr = new System.Windows.Forms.Timer();
            scoreTmr.Interval = 1;
            scoreTmr.Tick += new EventHandler(scoring);
            scoreTmr.Start();

            colTmr = new System.Windows.Forms.Timer();
            colTmr.Interval = 250;
            colTmr.Tick += new EventHandler(detectCollision);
            //colTmr.Start();

        }

        private void newGame_Click(object sender, RoutedEventArgs e)
        {
            time = 4000;
            score = 0;
            counter = 0;
            lblScore.Content = score.ToString();
            tmr.Start();
            scoreTmr.Start();
            colTmr.Start();

        }

        private void quit_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            engine.movePlayer(sender, e);
        }

        private void spawn(object sender, System.EventArgs e)
        {
            counter++;
            if(counter > 10 && time > 250)
            {
                counter = 0;
                time -= 250;
                tmr.Interval = time;
            }
            engine.spawn();
        }

        private void scoring(object sender, System.EventArgs e)
        {
            score++;
            lblScore.Content = score.ToString();
        }

        private void detectCollision(object sender, System.EventArgs e)
        {
            if (engine.detectCollision())
            {
                tmr.Stop();
                scoreTmr.Stop();
                colTmr.Stop();
                MessageBox.Show(this, "Game Over! Your Score: " + score.ToString());
                //playField.Children.Clear();
            }
        }

        private void about_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Welcome to Dodge\n The object of the game is to avoid the falling objects\n Use the left and Right arrow keys to move");
        }
    }
}
