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
        public double lifetime = 1;

        public DamageText(double xPos, double yPos, double damage, Canvas GameCanvas)
        {

            Random rnd = new Random();
            this.xPos = xPos + rnd.Next(-15, 15);
            this.yPos = yPos + rnd.Next(-15, 15);

            // TextBlock erstellen und stylen
            visual = new TextBlock();
            visual.Text = damage.ToString();
            visual.FontSize = 16;
            visual.FontWeight = System.Windows.FontWeights.Bold;
            visual.Foreground = Brushes.Yellow;

            GameCanvas.Children.Add(visual);
        }

        public void Update(double deltatime)
        {
            lifetime -= deltatime;
            yPos -= 50 * deltatime;

            if (lifetime < 0.2)
            {
                visual.Opacity = lifetime / 0.5;
            }
        }
    }
}
