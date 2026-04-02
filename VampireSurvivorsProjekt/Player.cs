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



        public void Move(bool wIsPressed, bool aIsPressed, bool dIsPressed, bool sIsPressed, double deltaTime)
        {

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

              
        }
    }
}
