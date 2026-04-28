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
        public double damage;
        double range;
        double level;
        public double attacksPerSecond;
        public double cooldownTimer;
        public Weapon(double damage, double attacksPerSecond, double range)
        {
            this.damage = damage;
            this.attacksPerSecond = attacksPerSecond;
            this.range = range;
            this.level = 1;
            this.cooldownTimer = 0;
        }

        public void UpdateWeapon(double deltaTime)
        {
            if (cooldownTimer > 0)
            {
                cooldownTimer -= deltaTime;
            }
            if (cooldownTimer <= 0)
            {
                cooldownTimer = 1 / attacksPerSecond;
            }
        }

        public Enemy FindNearestEnemy(List<Enemy> enemies, double playerXPos, double playerYPos)
        {
            Enemy nearestEnemy = null;
            double minDist = 5000;
            foreach(Enemy enemy in enemies)
            {
                double closestX = enemy.centerX - playerXPos;
                double closestY = enemy.centerY - playerYPos;

                double dist = closestX * closestY + closestY * closestX;

                if( dist < minDist)
                {
                    nearestEnemy = enemy;
                }
            }
            
            return nearestEnemy;
        }
    }

    public class Fireball : Weapon
    {
        public Fireball() : base(5, 1, 100) // Fireball konstruktor ruft die base klasse auf
        {

        }

    }

}
