using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Timers;

namespace FinalProject
{

    //This is the core of the code. The Engine class is what handles the creation of the player object and all of its movement
    //as well as the spawning of the obstacles the player needs to avoid

    class Engine
    {
        protected Canvas playField;
        protected Dispatcher dispatcher;
        Int32 waitTime;
        Rectangle player;
        Rect pHitBox;
        List<Obstacle> obstacles;
        int xPos = 125;
        int yPos = 400;
        int speed = 50;
        Random rand = new Random();

        public Engine(Canvas playField, Dispatcher dispatcher, Int32 waitTime = 100)
        {
            this.playField = playField;
            this.dispatcher = dispatcher;
            this.waitTime = waitTime;

            //This creates the visual for the player
            player = new Rectangle();
            player.Stroke = new SolidColorBrush(Colors.Black);
            player.StrokeThickness = 2;
            player.Fill = new SolidColorBrush(Colors.Black);
            player.Width = 50;
            player.Height = 50;
            Canvas.SetLeft(player, xPos);
            Canvas.SetTop(player, yPos);
            playField.Children.Add(player);
            obstacles = new List<Obstacle>();

            //This is the collider for the player to detect when it hits and obstacles
            pHitBox = new Rect();
            pHitBox.Location = new Point(xPos, yPos);
            pHitBox.Size = new Size(50, 50);
        } //end constructor

        //All movement of the player and its collider are handle by this method
        public void movePlayer(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Left)
            {
                Canvas.SetLeft(player, Canvas.GetLeft(player) - 5);
                pHitBox.X -= 5;
                if(Canvas.GetLeft(player) < 0)
                {
                    Canvas.SetLeft(player, Canvas.GetLeft(player) + 5);
                    pHitBox.X += 5;
                } // end if
            } //end if

            if (e.Key == Key.Right)
            {
                Canvas.SetLeft(player, Canvas.GetLeft(player) + 5);
                pHitBox.X += 5;
                if (Canvas.GetLeft(player) > (playField.ActualWidth - player.Width))
                {
                    Canvas.SetLeft(player, Canvas.GetLeft(player) - 5);
                    pHitBox.X -= 5;
                }// end if
            } //end if
        } //end movePlayer()
        
        //Handles the spawning of the objects the player must avoid
        //Called every few seconds by a Timer run by the MainWindow
        public void spawn()
        {
            Obstacle obstacle = new Obstacle(playField, dispatcher, rand.Next(0, 250), player, pHitBox, speed);
            obstacles.Add(obstacle);
        } //end Spawn()
        
        public bool getCollision()
        {
            return Obstacle.getCollision();
        }

        public bool detectCollision()
        {
            bool trigger = false;
            foreach(Obstacle obstacle in obstacles)
            {
                if (pHitBox.IntersectsWith(obstacle.obHitBox))
                {
                    trigger =  true;
                }
            } //end foreach
            return trigger;
        } //end detectCollision
    } //end class
} //end namespace
