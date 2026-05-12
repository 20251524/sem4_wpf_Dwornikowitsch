using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Shapes;

namespace VampireSurvivorsProjekt
{
    public class ExperienceOrb
    {
        double orbXPos;
        double orbYPos;
        double xpValue;
        Shape visual;

        public ExperienceOrb(double enemyCenterX, double enemyCenterY, double xpValue, Shape visual)
        {
            this.xpValue = xpValue;
            this.orbXPos = enemyCenterX;
            this.orbYPos = enemyCenterY;
            this.visual = visual;
        }
    }
    
    public class BlueOrb : ExperienceOrb
    {
        public BlueOrb(double orbXPos, double orbYPos) : base(orbXPos,orbYPos, 1, new Ellipse { Width = 10, Height = 10, Fill = Brushes.Blue })
        {

        }

        
    }
    
}
