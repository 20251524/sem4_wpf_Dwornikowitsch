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
            double minDist = double.MaxValue;
            foreach(Enemy enemy in enemies)
            {
                double closestX = enemy.centerX - playerXPos;
                double closestY = enemy.centerY - playerYPos;

                double dist = closestX * closestX + closestY * closestY;

                if( dist < minDist)
                {
                    nearestEnemy = enemy;
                    minDist = dist;
                }
            }
            
            return nearestEnemy;
        }
    }

   public class Shuriken : Weapon
    {
       
        public Shuriken() : base(10, 1, 100)
        {

        }

        public void UpdateShuriken(double deltaTime, Player player, List<Projectile> projectileList, Canvas GameCanvas)
        {
            base.UpdateWeapon(deltaTime);

            if(cooldownTimer >= 1 /  attacksPerSecond)
            {
                Shape shurikenVisual = new Ellipse { Width = 10, Height = 10, Fill = Brushes.Silver };
                double targetX = player.playerXPos + (player.playerchar.Width / 2) + (player.lastXDirection);
                double targetY = player.playerYPos + (player.playerchar.Height / 2) + (player.lastYDirection );

                projectileList.Add(new Projectile(
                player.playerXPos + (player.playerchar.Width / 2), // Start in der Mitte des playerchar
                player.playerYPos + (player.playerchar.Height / 2),
                300, // speed
                this.damage,
                targetX,
                targetY,
                GameCanvas,
                shurikenVisual));

            }
        }
    }

    public class Fireball : Weapon
    {

        public Fireball() : base(5, 1, 100) // Fireball konstruktor ruft die base klasse auf
        {
            
        }

        public void UpdateFireball(double deltaTime, List<Enemy> enemies, Player player, List<Projectile> projectileList, Canvas GameCanvas)
        {

            base.UpdateWeapon(deltaTime); // Cooldown runterzählen

            if (cooldownTimer >= 1 / attacksPerSecond) // CD überprüfen
            {
                Enemy nearest = FindNearestEnemy(enemies, player.playerXPos, player.playerYPos);

                if (nearest != null)
                {
                        Shape fireballVisual = new Ellipse { Width = 15, Height = 15, Fill = Brushes.OrangeRed };

                        projectileList.Add(new Projectile(
                        player.playerXPos + (player.playerchar.Width /2), // Start in der Mitte des playerchar
                        player.playerYPos + (player.playerchar.Height / 2),
                        300, // speed
                        this.damage,
                        nearest.centerX, 
                        nearest.centerY,
                        GameCanvas,
                        fireballVisual));
                }
            }
        }

    }

}
