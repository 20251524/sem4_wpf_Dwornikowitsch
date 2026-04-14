using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace VampireSurvivorsProjekt
{
    internal class Player
    {

        public double playerXPos = 100;
        public double playerYPos = 100;
        double playerSpeed = 150;
        double xDirection;
        double yDirection;
        public Rectangle playerchar;
        public Rect playerhitbox;
        

        public Player(double playerPpos, double playerYPos, double playerSpeed, Canvas GameCanvas)
        {
            this.playerXPos = playerPpos;
            this.playerYPos = playerYPos;
            this.playerSpeed = playerSpeed;
            playerchar = new Rectangle();
            SolidColorBrush blackBrush = new SolidColorBrush();
            blackBrush.Color = Colors.Red;
            playerchar.Fill = blackBrush;
            playerchar.Height = 50;
            playerchar.Width = 50;
            playerhitbox.Width = 50;
            playerhitbox.Height = 50;
            GameCanvas.Children.Add(playerchar); 
        }


        public void Move(bool wIsPressed, bool aIsPressed, bool dIsPressed, bool sIsPressed, double deltaTime)
        {
            /*
            if (wIsPressed == true)
            {
                playerYPos -= playerSpeed * deltaTime;
            }
            if(sIsPressed == true)
            {
                playerYPos += playerSpeed * deltaTime;
            }
            if(aIsPressed == true)
            {
                playerXPos -= playerSpeed * deltaTime;
            }
            if(dIsPressed == true)
            {
                playerXPos += playerSpeed * deltaTime;
            }
            */

            if (wIsPressed == true)
            {
                yDirection -= 1;
            }
            if (sIsPressed == true)
            {
                yDirection += 1;
            }
            if(sIsPressed && wIsPressed || wIsPressed == false && sIsPressed == false )
            {
                yDirection = 0;
            }
            if (aIsPressed == true)
            {
                xDirection -= 1;
            }
            if (dIsPressed == true)
            {
                xDirection += 1;
            }
            if (aIsPressed && dIsPressed || aIsPressed == false && dIsPressed == false)
            {
                xDirection = 0;
            }

            double length = Math.Sqrt(yDirection * yDirection + xDirection * xDirection); // berechnung zur vektor normalisierung
            if (length > 0) // Division durch 0 verhindern
            {
                xDirection = xDirection / length;
                yDirection = yDirection / length;

                playerXPos += xDirection * playerSpeed * deltaTime;
                playerYPos += yDirection * playerSpeed * deltaTime;
            }


        }
    }
}
