using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace VampireSurvivorsProjekt
{
    internal class Player
    {

        public double playerXPos = 100;
        public double playerYPos = 100;
        double playerSpeed = 150;
        double xDirection;
        double yDirection;



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
