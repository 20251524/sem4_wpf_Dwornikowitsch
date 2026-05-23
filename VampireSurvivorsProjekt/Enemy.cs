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
    public class Enemy 
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
        public double health;

        public Enemy(double enemyXPos, double enemyYPos, double enemySpeed, double maxHealth, double size, Brush color, Canvas GameCanvas)
        {
            this.enemyXPos = enemyXPos;
            this.enemyYPos = enemyYPos;
            this.enemySpeed = enemySpeed;
            this.health = maxHealth;

            enemychar = new Ellipse();
            enemychar.Fill = color;
            enemychar.Height = size;
            enemychar.Width = size;

            radius = enemychar.Width / 2;
            GameCanvas.Children.Add(enemychar); // Neuen Kreis im Canvas erstellen bei jedem neuen enemy
        } 
     

        public void Update(double playerXPos, double playerYPos, double deltaTime)
        {
            xDirection = playerXPos - enemyXPos; 
            yDirection = playerYPos - enemyYPos; 
            double length = Math.Sqrt(xDirection * xDirection + yDirection * yDirection); // berechnung zur vektor normalisierung
            if(length > 0) // Division durch 0 verhindern
            {
                xDirection = xDirection / length;
                yDirection = yDirection / length;
            }

            enemyXPos += xDirection * enemySpeed * deltaTime;
            enemyYPos += yDirection * enemySpeed * deltaTime;
        }

        public void getCenter()
        {
            centerX = enemyXPos + radius;
            centerY = enemyYPos + radius;
        }

        public class Zombie : Enemy
        {
            public Zombie(double x, double y, Canvas canvas) : base(x, y, 100, 10, 50, Brushes.Black, canvas) { }
        }
        public class Bat : Enemy
        {
            public Bat(double x, double y, Canvas canvas) : base(x, y, 170, 3, 30, Brushes.DarkRed, canvas) { }
        }
        public class Oger : Enemy
        {
            public Oger(double x, double y, Canvas canvas) : base(x, y, 50, 45, 80, Brushes.DarkGreen, canvas) { }
        }
    }
}
