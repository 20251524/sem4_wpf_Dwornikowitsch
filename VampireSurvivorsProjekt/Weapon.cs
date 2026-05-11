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

   public class Knife : Weapon
    {
        public Knife() : base(10, 2, 100)
        {

        }

        public void UpdateKnife(double deltaTime, Player player, List<Projectile> projectileList, Canvas GameCanvas)
        {
            base.UpdateWeapon(deltaTime);

            if(cooldownTimer >= 1 /  attacksPerSecond)
            {
                projectileList.Add(new Projectile(
                player.playerXPos + (player.playerchar.Width / 2), // Start in der Mitte des playerchar
                player.playerYPos + (player.playerchar.Height / 2),
                300, // speed
                this.damage,
                player.xDirection,
                player.yDirection,
                GameCanvas));

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
                    projectileList.Add(new Projectile(
                        player.playerXPos + (player.playerchar.Width /2), // Start in der Mitte des playerchar
                        player.playerYPos + (player.playerchar.Height / 2),
                        300, // speed
                        this.damage,
                        nearest.centerX, 
                        nearest.centerY,
                        GameCanvas));
                }
            }
        }

    }

}
