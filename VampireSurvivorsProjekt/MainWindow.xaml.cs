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
        List<Enemy> enemies;
        List<Enemy> deadenemies;
        Stopwatch stopwatch = new Stopwatch();
        double lastTime;
        double count = 0;



        public MainWindow()
        {
            InitializeComponent();
            KeyDown += Form_KeyDown;
            KeyUp += Form_KeyUp;
            Activate();
            Focus();
            player = new Player(200, 200, 150, GameCanvas);
            enemies = new List<Enemy>();
            deadenemies = new List<Enemy>();
            stopwatch.Start();
            lastTime = stopwatch.Elapsed.TotalSeconds;
            CompositionTarget.Rendering += GameLoop;


        }

        private void GameLoop(object sender, EventArgs e)
        {
            double currentTime = stopwatch.Elapsed.TotalSeconds;
            double deltaTime = currentTime - lastTime;
            lastTime = currentTime;

            //gegner außerhalb des sichtbaren bereichs spawnen
            count++; 
            if(count >= 300) //spawnrate
            {
                SpawnEnemies();
            }

            

            /*
            for(int i = enemies.Count; i >= 0; i--)
            {
                if(enemies[i].isdead == true)
                {
                    GameCanvas.Children.RemoveAt(i);
                    
                }
            }*/

            enemies.RemoveAll(enemy => enemy.isdead); // für jeden Enemy in der Liste prüfen ob er tod ist und dann entfernen




            //player 
            player.Move(wIsPressed, aIsPressed, dIsPressed, sIsPressed, deltaTime);
            Canvas.SetLeft(player.playerchar, player.playerXPos);
            Canvas.SetTop(player.playerchar, player.playerYPos);




            //enemy
            foreach (Enemy enemy in enemies)
            {
                enemy.Update(player.playerXPos, player.playerYPos, deltaTime);
                Canvas.SetLeft(enemy.enemychar, enemy.enemyXPos);
                Canvas.SetTop(enemy.enemychar, enemy.enemyYPos);
            }

            //Debug

            DebugMode();

            foreach(Enemy enemy in enemies)
            {
                enemy.getCenter();
            }

            foreach (Enemy enemy in enemies)
            {
                double closestX = Math.Clamp(enemy.centerX, player.playerhitbox.Left, player.playerhitbox.Right);
                double closestY = Math.Clamp(enemy.centerY, player.playerhitbox.Top, player.playerhitbox.Bottom);
                double dx = enemy.centerX - closestX;
                double dy = enemy.centerY - closestY;
                double distance = Math.Sqrt(dx * dx + dy * dy);
                Debug.WriteLine(distance);
                //Debug.WriteLine(closestX);
                //Debug.WriteLine(closestY);
                if (distance <= enemy.radius)
                {
                    Debug.WriteLine("Collision!");
                    enemy.isdead = true;
                }

            }

        }

        private void DebugMode()
        {
            if (fIsPressed == true)
            {
                Canvas.SetLeft(player.playerhitboxdebug, player.playerhitbox.Left);
                Canvas.SetTop(player.playerhitboxdebug, player.playerhitbox.Top);
                if (debugmode == false)
                {
                    GameCanvas.Children.Add(player.playerhitboxdebug);
                    debugmode = true;
                }

            }
            if (fIsPressed == false && debugmode == true)
            {
                GameCanvas.Children.Remove(player.playerhitboxdebug);
                debugmode = false;
            }
        }


        private void SpawnEnemies()
        {
            Random random = new Random();
            int rnd = random.Next(1, 5);
            switch (rnd)
            {
                case 1:
                    enemies.Add(new Enemy(random.Next(1280), -50, 50, GameCanvas));
                    count = 0;
                    break;
                case 2:
                    enemies.Add(new Enemy(random.Next(1280), 770, 50, GameCanvas));
                    count = 0;
                    break;
                case 3:
                    enemies.Add(new Enemy(-50, random.Next(720), 200, GameCanvas));
                    count = 0;
                    break;
                case 4:
                    enemies.Add(new Enemy(1330, random.Next(720), 200, GameCanvas));
                    count = 0;
                    break;
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