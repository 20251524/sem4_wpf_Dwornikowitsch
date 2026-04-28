using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace VampireSurvivorsProjekt
{
    public class Weapon
    {
        double damage;
        double range;
        double level;
        double attacksPerSecond;
        public double cooldownTimer;
        public Weapon(double damage, double attacksPerSecond, double range)
        {
            this.damage = damage;
            this.attacksPerSecond = attacksPerSecond;
            this.range = range;
            this.level = 1;
            this.cooldownTimer = 0;
        }


    }

    public class Fireball : Weapon
    {
        public Fireball() : base(5, 1, 100) // Fireball konstruktor ruft die base klasse auf
        {

        }

    }

}
