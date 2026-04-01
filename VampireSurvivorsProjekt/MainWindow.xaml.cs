using System.Text;
using System.Windows;
using System.Windows.Threading;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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
        DispatcherTimer gameTimer = new DispatcherTimer();
        public MainWindow()
        {
            InitializeComponent();
            KeyDown += Form_KeyDown;
            KeyUp += Form_KeyUp;
            Activate();
            Focus();
            player = new Player();
            gameTimer.Interval = TimeSpan.FromMilliseconds(16); // ~60 FPS
            gameTimer.Tick += GameLoop;
            gameTimer.Start();
            
           

        }

        private void GameLoop(object sender, EventArgs e)
        {

            player.Move(wIsPressed, aIsPressed, dIsPressed, sIsPressed);
            Canvas.SetLeft(PlayerCharacter, player.playerXPos);
            Canvas.SetTop(PlayerCharacter, player.playerYPos);
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