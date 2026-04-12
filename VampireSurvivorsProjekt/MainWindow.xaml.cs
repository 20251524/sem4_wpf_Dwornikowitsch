using System;
using System.Diagnostics;
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
        Player player;
        List<Enemy> enemies;      
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
            player = new Player();
            enemies = new List<Enemy>();
            enemies.Add(new Enemy(40, 40, 100, GameCanvas));
            enemies.Add(new Enemy(20, 20, 50, GameCanvas));
            stopwatch.Start();
            lastTime = stopwatch.Elapsed.TotalSeconds;
            CompositionTarget.Rendering += GameLoop;


        }

        private void GameLoop(object sender, EventArgs e)
        {
            double currentTime = stopwatch.Elapsed.TotalSeconds;
            double deltaTime = currentTime - lastTime;
            lastTime = currentTime;
            count++;
            if(count >= 100)
            {
                enemies.Add(new Enemy(20, 20, 50, GameCanvas));
                count = 0;
            }

            player.Move(wIsPressed, aIsPressed, dIsPressed, sIsPressed, deltaTime);
            Canvas.SetLeft(PlayerCharacter, player.playerXPos);
            Canvas.SetTop(PlayerCharacter, player.playerYPos);
            foreach (Enemy enemy in enemies)
            {
                enemy.Update(player.playerXPos, player.playerYPos, deltaTime);
                Canvas.SetLeft(enemy.enemychar, enemy.enemyXPos);
                Canvas.SetTop(enemy.enemychar, enemy.enemyYPos);
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