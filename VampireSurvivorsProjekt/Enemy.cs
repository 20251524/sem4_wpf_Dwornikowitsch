using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
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

        public Enemy(double enemyXPos, double enemyYPos,  double enemySpeed, Canvas GameCanvas)
        {
            this.enemyXPos = enemyXPos;
            this.enemyYPos = enemyYPos;
            this.enemySpeed = enemySpeed;
            enemychar = new Ellipse();
            SolidColorBrush blackBrush = new SolidColorBrush();
            blackBrush.Color = Colors.Black;
            enemychar.Fill = blackBrush;
            enemychar.Height = 50; 
            enemychar.Width = 50;
            GameCanvas.Children.Add(enemychar);
        }

        public void Update(double playerXPos, double playerYPos, double deltaTime)
        {
            xDirection = playerXPos - enemyXPos;
            yDirection = playerYPos - enemyYPos;
            double length = Math.Sqrt(xDirection * xDirection + yDirection * yDirection);
            if(length > 0)
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
    }
}
