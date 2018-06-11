using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace FinalProject
{
    //The Obstcle class handles the creation of the objects the player needs to avoid and sets up the threading for
    //each object to be able to move independantly of each other. Also handles the detection of collision with the
    //player

    class Obstacle
    {
        static bool collision = false;
        Rectangle ob;
        public Rect obHitBox;
        Rectangle player;
        Rect pHitBox;
        Canvas playField;
        Dispatcher dispatcher;
        Int32 waitTime;
        public Thread posnThread;
        double incrementSize = 2.0;
        int xPos;
        int yPos = 15;
        Random rnd = new Random();

        public Obstacle(Canvas playField, Dispatcher dispatcher, int rand, Rectangle player, Rect pHitBox, Int32 waitTime = 100)
        {

            this.playField = playField;
            this.dispatcher = dispatcher;
            this.waitTime = waitTime;
            this.player = player;
            this.pHitBox = pHitBox;

            //Creation of the obstacle object
            Random rnd = new Random();
            xPos = rand;
            ob = new Rectangle();
            ob.Stroke = new SolidColorBrush(Colors.Red);
            ob.StrokeThickness = 2;
            ob.Fill = new SolidColorBrush(Colors.Red);
            ob.Width = 50; 
            ob.Height = 50;
            Canvas.SetLeft(ob, xPos);
            Canvas.SetTop(ob, yPos);
            playField.Children.Add(ob);

            //Creation of the hitBox for the obstacles
            obHitBox = new Rect();
            obHitBox.Location = new Point(xPos, yPos);
            obHitBox.Size = new Size(50, 50);

            //Starts the movement thread for the instance
            posnThread = new Thread(position);
            posnThread.IsBackground = true;
            posnThread.Start();           
        } //end constructor

        //Handles the movement of the obstcles
        void position()
        {
            while (true)
            {
                yPos += (int)incrementSize;
                if (obHitBox.IntersectsWith(pHitBox))
                {
                    collision = true;
                } //end if

                if(pHitBox.Y > 450)
                {
                    posnThread.Abort();
                }
                updatePosition();

                Thread.Sleep(waitTime);
            } //end while
        } //end position()

        //updates the position of the object on the screen
        void updatePosition()
        {
            Action action = () =>
            {
                Canvas.SetLeft(ob, xPos);
                Canvas.SetTop(ob, yPos);
                obHitBox.Y = yPos;
            };
            dispatcher.BeginInvoke(action);
        } //end updatePosition()

        public static bool getCollision()
        {
            return collision;
        }

        //Hamdles the shutdown of the thread
        void shutDown()
        {
            if(posnThread != null)
            {
                posnThread.Abort();
                
            }
        } //end shutDown()
    } //end class
} //end namespace
