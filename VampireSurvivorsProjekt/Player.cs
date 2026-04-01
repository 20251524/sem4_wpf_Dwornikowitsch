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
        double playerSpeed = 2;



        public void Move(bool wIsPressed, bool aIsPressed, bool dIsPressed, bool sIsPressed)
        {

            if (wIsPressed == true)
            {
                playerYPos -= playerSpeed;
            }
            if(sIsPressed == true)
            {
                playerYPos += playerSpeed;
            }
            if(aIsPressed == true)
            {
                playerXPos -= playerSpeed;
            }
            if(dIsPressed == true)
            {
                playerXPos += playerSpeed;
            }

              
        }
    }
}
