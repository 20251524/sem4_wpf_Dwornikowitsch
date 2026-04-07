using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace VampireSurvivorsProjekt
{
    internal class Enemy
    {
        public double enemyXPos = 100;
        public double enemyYPos = 100;
        double xDirection = 0;
        double yDirection = 0;
        double enemySpeed = 100;

        public void Update(double playerXPos, double playerYPos, double deltaTime)
        {
            xDirection = playerXPos - enemyXPos;
            yDirection = playerYPos - enemyYPos;
            double length = Math.Sqrt(xDirection * xDirection + yDirection * yDirection);
            xDirection = xDirection /length;
            yDirection = yDirection /length;

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
