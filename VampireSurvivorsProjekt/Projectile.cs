using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace VampireSurvivorsProjekt
{
    public class Projectile
    {
        public double xPos;
        public double yPos;
        double xDir;
        double yDir;
        double speed;
        public double damage;
        double radius;
        public bool toRemove;
        public Shape visual;

        public Projectile(double xPos, double yPos, double speed, double damage, double xTarget, double yTarget, Canvas GameCanvas, Shape customVisual)
        {
            this.xPos = xPos;
            this.yPos = yPos;
            this.speed = speed;
            this.damage = damage;

            this.xDir = xTarget - xPos;
            this.yDir = yTarget - yPos;
            double length = Math.Sqrt(xDir * xDir + yDir * yDir);
            
            if (length > 0) // Division durch 0 verhindern
            {
                this.xDir = xDir / length;
                this.yDir = yDir / length;
            }
                    
            this.visual = customVisual;
            GameCanvas.Children.Add(visual);
            radius = visual.Width / 2;
        }

        public void UpdateProjectile(double deltatime)
        {
            this.xPos += xDir * deltatime * speed;
            this.yPos += yDir * deltatime * speed;
        }
    }
}
