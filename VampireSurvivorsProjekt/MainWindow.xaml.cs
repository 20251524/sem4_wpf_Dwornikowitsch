using System;
using System.Diagnostics;
using System.Drawing;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace VampireSurvivorsProjekt
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        bool wIsPressed = false;
        bool aIsPressed = false;
        bool sIsPressed = false;
        bool dIsPressed = false;
        bool fIsPressed = false;
        bool debugmode = false;
        Player player;
        public List<Enemy> enemies;
        Stopwatch stopwatch = new Stopwatch();
        double lastTime;
        double spawnTimer = 0;
        double spawnInterval = 1; // spawnt jede Sekunde
        double cameraX = 1280 / 2;
        double cameraY = 720 / 2;
        int windowWidth = 1280;
        int windowHeight = 720;
        public List<Projectile> activeProjectilesList = new List<Projectile>();
        public double deltaTime;
        Fireball activeFireball;
        Knife activeKnife;


        public MainWindow()
        {
            InitializeComponent();
            KeyDown += Form_KeyDown;
            KeyUp += Form_KeyUp;
            Activate();
            Focus();
            player = new Player(200, 200, 150, GameCanvas);
            enemies = new List<Enemy>();
            activeFireball = new Fireball();
            activeKnife = new Knife();
            stopwatch.Start();
            lastTime = stopwatch.Elapsed.TotalSeconds;
            CompositionTarget.Rendering += GameLoop;


        }

        private void GameLoop(object sender, EventArgs e)
        {
            //Zeit Berechnung
            UpdateDeltaTime();

            //Gegner spawnuing
            SpawnHandling();

            //Bewegung und Logik
            UpdateGameObjects();

            //Kollisionen
            CollisionHandling();

            //Kamera Updaten
            UpdateCamera();

            //Alles auf den Screen zeichnen
            DrawEverything();

            //Gegner entfernen
            Cleanup();

            DebugMode();
        }

        private void UpdateDeltaTime()
        {
            double currentTime = stopwatch.Elapsed.TotalSeconds; // Zeit seit Start des Spiels in Sekunden
            deltaTime = currentTime - lastTime; // Zeitdifferenz seit dem letzten Frame (DeltaTime)
            lastTime = currentTime; // Aktuelle Zeit für den nächsten Frame speichern
        }

        private void SpawnHandling()
        {
            //gegner außerhalb des sichtbaren bereichs spawnen
            spawnTimer += deltaTime;
            if (spawnTimer >= spawnInterval) //spawnrate
            {
                SpawnEnemies();
                spawnTimer = 0;
            }
        }

        private void DrawEverything()
        {
            foreach (Projectile proj in activeProjectilesList)
            {
                // Player
                Canvas.SetLeft(player.playerchar, player.playerXPos - cameraX);
                Canvas.SetTop(player.playerchar, player.playerYPos - cameraY);

                // Enemies
                foreach (Enemy enemy in enemies)
                {
                    Canvas.SetLeft(enemy.enemychar, enemy.enemyXPos - cameraX);
                    Canvas.SetTop(enemy.enemychar, enemy.enemyYPos - cameraY);
                }

                // projectiles
                Canvas.SetLeft(proj.visual, proj.xPos - cameraX);
                Canvas.SetTop(proj.visual, proj.yPos - cameraY);
            }
        }

        private void UpdateGameObjects()
        {
            // Waffe
            activeFireball.UpdateFireball(deltaTime, enemies, player, activeProjectilesList, GameCanvas);
            activeKnife.UpdateShuriken(deltaTime, player, activeProjectilesList, GameCanvas);

            // Projektile bewegen
            foreach (Projectile proj in activeProjectilesList)
            {
                proj.UpdateProjectile(deltaTime);
            }

            // Player bewegen
            player.Move(wIsPressed, aIsPressed, dIsPressed, sIsPressed, deltaTime);

            // Gegner bewegen & Logik
            foreach (Enemy enemy in enemies)
            {
                enemy.Update(player.playerXPos, player.playerYPos, deltaTime);
                enemy.getCenter();
            }
        }

        private void CollisionHandling()
        {
            foreach (Enemy enemy in enemies)
            {
                double closestX = Math.Clamp(enemy.centerX, player.playerhitbox.Left, player.playerhitbox.Right);  // Nähesten X-Punkt am player rect finden
                double closestY = Math.Clamp(enemy.centerY, player.playerhitbox.Top, player.playerhitbox.Bottom);  // Nähesten Y-Punkt am player rect finden
                double dx = enemy.centerX - closestX;  // dx = X Distanz zum player
                double dy = enemy.centerY - closestY;  // dy = Y Distanz zum player
                double distance = Math.Sqrt(dx * dx + dy * dy);  // Gerade zum player mittels Pythagoras

                //Debug.WriteLine(closestY);
                if (distance <= enemy.radius)
                {
                    Debug.WriteLine("Collision!");
                    enemy.isdead = true;
                }

                foreach(Projectile proj in activeProjectilesList)
                {
                    double pdx = proj.xPos - enemy.centerX;
                    double pdy = proj.yPos - enemy.centerY;

                    if ((pdx * pdx + pdy * pdy) < (enemy.radius * enemy.radius))
                    {
                        enemy.health -= proj.damage;
                        if (enemy.health <= 0)
                        {
                            enemy.isdead = true;
                        }
                        proj.toRemove = true;
                    }
                }
            }
        }

        private void Cleanup()
        {
            for (int i = enemies.Count - 1; i >= 0; i--)
            {
                if (enemies[i].isdead == true)
                {
                    GameCanvas.Children.Remove(enemies[i].enemychar);  // jeden enemychar entfernen bei dem isdead true ist
                    GameCanvas.Children.Remove(enemies[i].debugCenterPoint);
                    enemies.RemoveAt(i);
                }
            }
            
            for(int i = activeProjectilesList.Count - 1; i>= 0; i--)
            {
                if (activeProjectilesList[i].toRemove == true)
                {
                    GameCanvas.Children.Remove(activeProjectilesList[i].visual);
                    activeProjectilesList.RemoveAt(i);
                }

            }
            
        }


        private void UpdateCamera()
        {
            cameraX = (player.playerXPos + (player.playerchar.Width)) - (GameCanvas.ActualWidth / 2) ; // links 
            cameraY = (player.playerYPos + (player.playerchar.Height)) - (GameCanvas.ActualHeight / 2) ; // oben
        }



        //Spawn Logik
        private void SpawnEnemies()
        {
            Random random = new Random();
            int rnd = random.Next(1, 5);
            int minX = (int)cameraX; // (int) lässt den double wert wie einen int behandeln. CameraX ist oben links
            int maxX = (int)(cameraX + GameCanvas.ActualWidth); // vom linken Rand des Bildschirms zum rechten
            int minY = (int)cameraY; // oben links
            int maxY = (int)(cameraY + GameCanvas.ActualHeight); // von oben nach unten
            switch (rnd)
            {
                case 1:
                    enemies.Add(new Enemy(random.Next(minX, maxX),cameraY -50, 50, GameCanvas)); // oben
                    break;
                case 2:
                    enemies.Add(new Enemy(random.Next(minX, maxX), cameraY + windowHeight + 50 , 50, GameCanvas)); // unten
                    break;
                case 3:
                    enemies.Add(new Enemy(cameraX -50, random.Next(minY, maxY), 50, GameCanvas)); // links
                    break;
                case 4:
                    enemies.Add(new Enemy(cameraX + windowWidth + 50, random.Next(minY, maxY), 50, GameCanvas)); // rechts
                    break;
            }   
        }



        private void DebugMode()
        {
            if (fIsPressed == true) // bei Debug on
            {
                Canvas.SetLeft(player.playerhitboxdebug, player.playerhitbox.Left - cameraX);
                Canvas.SetTop(player.playerhitboxdebug, player.playerhitbox.Top - cameraY);

                foreach (Enemy enemy in enemies)
                {
                    if(GameCanvas.Children.Contains(enemy.debugCenterPoint) == false)
                    {
                        GameCanvas.Children.Add(enemy.debugCenterPoint);
                    }
                    Canvas.SetLeft(enemy.debugCenterPoint, enemy.centerX - cameraX);
                    Canvas.SetTop(enemy.debugCenterPoint, enemy.centerY - cameraY);
                }
                if (debugmode == false)
                {                   
                    GameCanvas.Children.Add(player.playerhitboxdebug);
                    debugmode = true;
                }

            }

            if (fIsPressed == false && debugmode == true) // bei Debug off
            {
                
                foreach (Enemy enemy in enemies)
                {
                    GameCanvas.Children.Remove(enemy.debugCenterPoint);
                }
                GameCanvas.Children.Remove(player.playerhitboxdebug);
                debugmode = false;
            }
        }



        private void Form_KeyDown(object sender, KeyEventArgs e)
        {


            if (e.Key == Key.W)
            {
                wIsPressed = true;
            }
            if (e.Key == Key.A)
            {
                aIsPressed = true;
            }
            if (e.Key == Key.S)
            {
                sIsPressed = true;
            }
            if (e.Key == Key.D)
            {
                dIsPressed = true;
            }
            if(e.Key == Key.F)
            {
                if(fIsPressed == false)
                {
                    fIsPressed = true;
                }
                else
                {
                    fIsPressed = false;
                }
            }
        }

        private void Form_KeyUp(object sender, KeyEventArgs e)
        {


            if (e.Key == Key.W)
            {
                wIsPressed = false;
            }
            if (e.Key == Key.A)
            {
                aIsPressed = false;
            }
            if (e.Key == Key.S)
            {
                sIsPressed = false;
            }
            if (e.Key == Key.D)
            {
                dIsPressed = false;
            }

        }


    }
}