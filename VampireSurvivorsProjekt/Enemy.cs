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
using System.Windows;

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

            debugCenterPoint = new Ellipse();
            debugCenterPoint.Height = 6;
            debugCenterPoint.Width = 6;
            debugCenterPoint.Fill = Brushes.Red;
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
            public Zombie(double x, double y, Canvas canvas) : base(x, y, 100, 10, 50, CreateZombieBrush(), canvas) { }

            private static Brush CreateZombieBrush()
            {
                RadialGradientBrush brush = new RadialGradientBrush();
                brush.GradientOrigin = new Point(0.3, 0.3);
                brush.Center = new Point(0.5, 0.5);
                brush.GradientStops.Add(new GradientStop(Colors.SlateGray, 0.0));
                brush.GradientStops.Add(new GradientStop(Colors.DarkSlateGray, 0.7));
                brush.GradientStops.Add(new GradientStop(Colors.Black, 1.0));
                return brush;
            }
        }
        public class Bat : Enemy
        {
            public Bat(double x, double y, Canvas canvas) : base(x, y, 170, 3, 30, CreateBatBrush(), canvas) { }

            private static Brush CreateBatBrush()
            {
                RadialGradientBrush brush = new RadialGradientBrush();
                brush.GradientOrigin = new Point(0.3, 0.3);
                brush.Center = new Point(0.5, 0.5);
                brush.GradientStops.Add(new GradientStop(Colors.Red, 0.0));
                brush.GradientStops.Add(new GradientStop(Colors.DarkRed, 0.7));
                brush.GradientStops.Add(new GradientStop(Colors.Purple, 1.0)); // Violetter Schatteneffekt
                return brush;
            }
        }
        public class Oger : Enemy
        {
            public Oger(double x, double y, Canvas canvas) : base(x, y, 50, 45, 80, CreateOgerSkin(), canvas) { }

            private static Brush CreateOgerSkin()
            {
                // Ein radialer Farbverlauf lässt den Vektor 3D-mäßig rund wirken!
                RadialGradientBrush gradient = new RadialGradientBrush();
                gradient.GradientOrigin = new Point(0.3, 0.3); // Lichtquelle oben links
                gradient.Center = new Point(0.5, 0.5);

                gradient.GradientStops.Add(new GradientStop(Colors.LimeGreen, 0.0));
                gradient.GradientStops.Add(new GradientStop(Colors.DarkGreen, 0.8));
                gradient.GradientStops.Add(new GradientStop(Colors.Black, 1.0));

                return gradient;
            }
        }


    }
}
