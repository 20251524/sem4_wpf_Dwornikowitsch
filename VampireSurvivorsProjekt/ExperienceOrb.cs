using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace VampireSurvivorsProjekt
{
    public class ExperienceOrb
    {
        public double orbXPos;
        public double orbYPos;
        double xpValue;
        public Shape visual;
        public bool isCollected = false;

        public ExperienceOrb(double enemyCenterX, double enemyCenterY, double xpValue, Shape visual, Canvas GameCanvas)
        {
            this.xpValue = xpValue;
            this.orbXPos = enemyCenterX;
            this.orbYPos = enemyCenterY;
            this.visual = visual;

            GameCanvas.Children.Add(this.visual);
        }

        public void UpdateOrb()
        {

        }
    }
    
    public class BlueOrb : ExperienceOrb
    {
        public BlueOrb(double orbXPos, double orbYPos, Canvas GameCanvas) : base(orbXPos,orbYPos, 1, new Ellipse { Width = 10, Height = 10, Fill = Brushes.Blue }, GameCanvas) //Linke Klammer dynamische Werte die übergeben werden, rechte Klammer alle + fixe Wert
        {

        }

        
    }
    
}
