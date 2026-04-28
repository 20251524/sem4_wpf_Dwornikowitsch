using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;

namespace VampireSurvivorsProjekt
{
    internal class Projectile
    {
        public double xPos;
        public double yPos;
        double xDir;
        double yDir;
        double speed;
        double damage;
        public Shape visual;

        public Projectile(double xPos, double yPos, double xDir, double yDir, double speed, double damage, double xTarget, double yTarget)
        {
            this.xPos = xPos;
            this.yPos = yPos;
            this.speed = speed;
            this.damage = damage;

            xDir = xTarget - xPos;
            yDir = yTarget - yPos;
            double length = Math.Sqrt(xDir * xDir + yDir * yDir);

            if (length > 0) // Division durch 0 verhindern
            {
                xDir = xDir / length;
                yDir = yDir / length;
            }

        }
    }
}
