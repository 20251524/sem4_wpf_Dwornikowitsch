using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Media3D;
using System.Windows.Shapes;

namespace VampireSurvivorsProjekt
{
    internal class Enemy 
    {
        public double enemyXPos = 100;
        public double enemyYPos = 100;
        double xDirection = 0;
        double yDirection = 0;
        double enemySpeed = 100;
        public Ellipse enemychar;
        public double centerX;
        public double centerY;
        public double radius;
        public bool isdead = false;
        public Ellipse debugCenterPoint;

        public Enemy(double enemyXPos, double enemyYPos,  double enemySpeed, Canvas GameCanvas)
        {
            this.enemyXPos = enemyXPos;
            this.enemyYPos = enemyYPos;
            this.enemySpeed = enemySpeed;
            enemychar = new Ellipse();
            enemychar.Fill = Brushes.Black;
            enemychar.Height = 50; 
            enemychar.Width = 50;

       

            radius = enemychar.Width / 2; // radius berechnen
            centerX = enemyXPos + radius;
            centerY = enemyYPos + radius;

            debugCenterPoint = new Ellipse();
            debugCenterPoint.Height = 5;
            debugCenterPoint.Width = 5;
            debugCenterPoint.Fill = Brushes.Blue;


            GameCanvas.Children.Add(enemychar); // Neuen Kreis im Canvas erstellen bei jedem neuen enemy
        }

        public void Update(double playerXPos, double playerYPos, double deltaTime)
        {
            xDirection = (playerXPos ) - (enemyXPos ); // +25 wegen SetLeft bzw SetTop sonst bewegt er sich zur alten playerpos
            yDirection = (playerYPos ) - (enemyYPos ); // ohne +25 zielen die gegner auf den spieler unten rechts
            double length = Math.Sqrt(xDirection * xDirection + yDirection * yDirection); // berechnung zur vektor normalisierung
            if(length > 0) // Division durch 0 verhindern
            {
                xDirection = xDirection / length;
                yDirection = yDirection / length;
            }


            enemyXPos += xDirection * enemySpeed * deltaTime;
            enemyYPos += yDirection * enemySpeed * deltaTime;

            /*
            if(xDirection > 0)
            {
                enemyXPos += xDirection * enemySpeed * deltaTime;
            }
            if(xDirection < 0)
            {
                enemyXPos -= xDirection * enemySpeed * deltaTime;
            }
            if(yDirection > 0)
            {
                enemyYPos += yDirection * enemySpeed * deltaTime;
            }
            if(yDirection < 0)
            {
                enemyYPos -= yDirection * enemySpeed * deltaTime;
            }
            */


        }

        public void getCenter()
        {
            centerX = enemyXPos + radius;
            centerY = enemyYPos + radius;
        }
    }
}
