using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace VampireSurvivorsProjekt
{
    public class ExperienceOrb
    {
        public double orbXPos;
        public double orbYPos;
        public double xpValue;
        public Shape visual;
        public bool isCollected = false;
        public Shape debugCenterPoint;
        public double radius;

        public ExperienceOrb(double enemyCenterX, double enemyCenterY, double xpValue, Shape visual, Canvas GameCanvas)
        {
            this.xpValue = xpValue;
            this.orbXPos = enemyCenterX;
            this.orbYPos = enemyCenterY;
            this.visual = visual;
            this.xpValue = xpValue;
            this.radius = this.visual.Width / 2; // radius anhand des visuals berechnen. Koordinaten funktionieren nicht weil sie sich verändern!!!

            debugCenterPoint = new Ellipse();
            debugCenterPoint.Height = 5;
            debugCenterPoint.Width = 5;
            debugCenterPoint.Fill = Brushes.Yellow;

            GameCanvas.Children.Add(this.visual);
        }

        public void UpdateOrb()
        {

        }
    }
    
    public class BlueOrb : ExperienceOrb
    {
        public BlueOrb(double orbXPos, double orbYPos, Canvas GameCanvas) : base(orbXPos,orbYPos, 1, new Ellipse { Width = 20, Height = 20, Fill = Brushes.Blue }, GameCanvas) //Linke Klammer dynamische Werte die übergeben werden, rechte Klammer alle + fixe Wert
        {

        }

        
    }
    
}
