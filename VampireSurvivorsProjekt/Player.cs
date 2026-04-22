using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Ink;
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
        public Rectangle playerhitboxdebug;
        

        public Player(double playerXpos, double playerYPos, double playerSpeed, Canvas GameCanvas)
        {
            this.playerXPos = playerXpos;
            this.playerYPos = playerYPos;
            this.playerSpeed = playerSpeed;

            playerchar = new Rectangle();
            playerchar.Fill = Brushes.Red;
            playerchar.Height = 50;
            playerchar.Width = 50;

            playerhitbox = new Rect(playerXpos , playerYPos, playerchar.Width, playerchar.Height); // Tatsächliche hitbox

            playerhitboxdebug = new Rectangle(); // sichtbare Hitbox für debugging
            playerhitboxdebug.Width = playerhitbox.Width;
            playerhitboxdebug.Height = playerhitbox.Height;
            playerhitboxdebug.Fill = Brushes.Transparent;
            playerhitboxdebug.Stroke = Brushes.Black;
            playerhitboxdebug.StrokeThickness = 2;


            GameCanvas.Children.Add(playerchar);
            
        }


        public void Move(bool wIsPressed, bool aIsPressed, bool dIsPressed, bool sIsPressed, double deltaTime)
        {
            // jedem Frame bei 0 starten
            xDirection = 0;
            yDirection = 0;

            // Richtung berechnen
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
            }

            // Position aktualisieren
            playerXPos += xDirection * playerSpeed * deltaTime;
            playerYPos += yDirection * playerSpeed * deltaTime;
            playerhitbox = new Rect(playerXPos , playerYPos , playerhitbox.Width, playerhitbox.Height);

        }

    }
}
