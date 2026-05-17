using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace VampireSurvivorsProjekt
{
    public class DamageText
    {
        public TextBlock visual;
        public double xPos;
        public double yPos;

        public DamageText(double xPos, double yPos, double damage, Canvas GameCanvas)
        {
            this.xPos = xPos;
            this.yPos = yPos; 

            // TextBlock erstellen und stylen
            visual = new TextBlock();
            visual.Text = damage.ToString();
            visual.FontSize = 16;
            visual.FontWeight = System.Windows.FontWeights.Bold;
            visual.Foreground = Brushes.Yellow;

            GameCanvas.Children.Add(visual);
        }
    }
}
