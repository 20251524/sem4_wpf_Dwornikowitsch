using System;
using System.Data.SQLite;
using System.Diagnostics;
using System.Drawing;
using System.Security.Cryptography;
using System.Security.Cryptography.Xml;
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
using static VampireSurvivorsProjekt.Enemy;

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
        Shuriken activeShuriken;
        Garlic activeGarlic;
        public List<ExperienceOrb> ExpOrbsList;
        bool isPaused = true;
        public List<DamageText> damageTexts;
        double damageCooldown = 0; 
        double damageCooldownMax = 0.5; // Spieler kann nur alle 0.5 Sekunden Schaden bekommen

        string playerName = "Spieler 1";
        int totalKills = 0;
        double survivedTime = 0; // Misst die überlebten Sekunden im Spiel

        bool gameStarted = false;

        //Upgrade Level variablen
        int speedLevel = 1;
        int rangeLevel = 1;
        int fireballLevel = 1;
        int shurikenLevel = 0;
        int garlicLevel = 0;

        //Liste aller Upgrades die es gibt. Müssen mit dem ButtonTag übereinstimmen sonst probleme
        List<string> upgradePool = new List<string> { "speed", "range", "fireball", "shuriken", "garlic" };

        // Erstellt die datei vs_stats.db 
        string dbPath = "Data Source=vs_stats.db";


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
            activeShuriken = null;
            activeGarlic = null;
            ExpOrbsList = new List<ExperienceOrb>();
            damageTexts = new List<DamageText>();
            stopwatch.Start();
            lastTime = stopwatch.Elapsed.TotalSeconds;
            CompositionTarget.Rendering += GameLoop;
            InitializeDatabase();

        }

        private void GameLoop(object sender, EventArgs e)
        {
            //Schutz Klausel
            //Durch das return wird die Methode beendet und das Programm geht aus dem Gameloop raus
            //und ignoriert alles was danach kommt. Effekt = Programm wird pausiert.
            if(isPaused== true)
            {
                return;
            }
            //Zeit Berechnung
            UpdateDeltaTime();

            if (!isPaused && gameStarted)
            {
                survivedTime += deltaTime; // Zählt die echten Sekunden hoch
                TimeSpan t = TimeSpan.FromSeconds(survivedTime);
                TxtInGameTimer.Text = $"{t.Minutes:D2}:{t.Seconds:D2}";
            }

            if (damageCooldown > 0)
            {
                damageCooldown -= deltaTime; // Timer runterzählen
            }

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
            // Player
            Canvas.SetLeft(player.playerchar, player.playerXPos - cameraX);
            Canvas.SetTop(player.playerchar, player.playerYPos - cameraY);

            // Enemies
            foreach (Enemy enemy in enemies)
            {
                Canvas.SetLeft(enemy.enemychar, enemy.enemyXPos - cameraX);
                Canvas.SetTop(enemy.enemychar, enemy.enemyYPos - cameraY);
            }

            foreach (Projectile proj in activeProjectilesList)
            {

                // projectiles
                Canvas.SetLeft(proj.visual, proj.xPos - cameraX);
                Canvas.SetTop(proj.visual, proj.yPos - cameraY);
            }

            foreach(ExperienceOrb orb in ExpOrbsList)
            {
                Canvas.SetLeft(orb.visual, orb.orbXPos - cameraX - orb.radius);
                Canvas.SetTop(orb.visual , orb.orbYPos - cameraY - orb.radius);
            }

            foreach(DamageText dt in damageTexts)
            {
                Canvas.SetLeft(dt.visual, dt.xPos - cameraX);
                Canvas.SetTop(dt.visual , dt.yPos - cameraY);
            }

            if (activeGarlic != null)
            {
                if (GameCanvas.Children.Contains(activeGarlic.auraVisual) == false)
                {
                    GameCanvas.Children.Add(activeGarlic.auraVisual);
                }

                // falls der radius geändert wurde werte updaten sonst buggy
                activeGarlic.auraVisual.Width = activeGarlic.range * 2;
                activeGarlic.auraVisual.Height = activeGarlic.range * 2;

                double playerCenterX = player.playerXPos + (player.playerchar.Width / 2);
                double playerCenterY = player.playerYPos + (player.playerchar.Height / 2);

                Canvas.SetLeft(activeGarlic.auraVisual, playerCenterX - cameraX - activeGarlic.range);
                Canvas.SetTop(activeGarlic.auraVisual, playerCenterY - cameraY - activeGarlic.range);
            }
        }

        private void UpdateGameObjects()
        {
            // Waffe
            if(activeFireball != null)
            {
                activeFireball.UpdateFireball(deltaTime, enemies, player, activeProjectilesList, GameCanvas);
            }
            if(activeShuriken != null)
            {

                activeShuriken.UpdateShuriken(deltaTime, player, activeProjectilesList, GameCanvas);
            }

            if (activeGarlic != null)
            {
                activeGarlic.UpdateGarlic(deltaTime);
            }

            // Projektile bewegen
            foreach (Projectile proj in activeProjectilesList)
            {
                proj.UpdateProjectile(deltaTime);

                // Abstand zur Kamera
                double diffX = proj.xPos - cameraX;
                double diffY = proj.yPos - cameraY;

                // Puffer sonst verschwinden Projektile im Sichtfeld
                double buffer = 100;

                // Überprüfung
                if (diffX < -buffer || diffX > windowWidth + buffer || diffY < -buffer || diffY > windowHeight + buffer)
                {
                    proj.toRemove = true;
                }
            }

            // Player bewegen
            player.Move(wIsPressed, aIsPressed, dIsPressed, sIsPressed, deltaTime);

            // Gegner bewegen & Logik
            foreach (Enemy enemy in enemies)
            {
                enemy.Update(player.playerXPos, player.playerYPos, deltaTime);
                enemy.getCenter();
            }

            foreach(DamageText dt in damageTexts)
            {
                dt.Update(deltaTime);
            }
        }

        private void CollisionHandling()
        {
            double playerCenterX = player.playerXPos + (player.playerchar.Width / 2);
            double playerCenterY = player.playerYPos + (player.playerchar.Height / 2);

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
                    if (damageCooldown <= 0)
                    {
                        player.currentHp -= 10;
                        HpBar.Value = player.currentHp; 
                        damageCooldown = damageCooldownMax; 

                        if (player.currentHp <= 0)
                        {
                            TriggerGameOver();
                            return; // Schleife und Methode sofort abbrechen
                        }
                    }
                }

                foreach(Projectile proj in activeProjectilesList)
                {
                    double pdx = proj.xPos - enemy.centerX; 
                    double pdy = proj.yPos - enemy.centerY;

                    if ((pdx * pdx + pdy * pdy) < (enemy.radius * enemy.radius))
                    {
                        enemy.health -= proj.damage;
                        damageTexts.Add(new DamageText(enemy.centerX, enemy.centerY, proj.damage, GameCanvas));
                        if (enemy.health <= 0)
                        {
                            enemy.isdead = true;
                            totalKills++;
                        }
                        proj.toRemove = true;
                    }
                }

                if (activeGarlic != null && activeGarlic.isReadyToDamage == true)
                {
                    if (distance <= activeGarlic.range )
                    {
                        enemy.health -= activeGarlic.damage;
                        damageTexts.Add(new DamageText(enemy.centerX, enemy.centerY, activeGarlic.damage, GameCanvas));

                        if (enemy.health <= 0) enemy.isdead = true;
                    }
                }
            }

            if(activeGarlic != null && activeGarlic.isReadyToDamage == true)
            {
                activeGarlic.isReadyToDamage = false;
            }

            foreach(ExperienceOrb orb in ExpOrbsList)
            {
                double dx = orb.orbXPos - playerCenterX; // X Distanz
                double dy = orb.orbYPos - playerCenterY; // Y Distanz

                if((dx * dx + dy * dy) < (player.pickupRange * player.pickupRange)) // Pythagoras
                {
                    orb.isCollected = true;
                    XpBar.Value += orb.xpValue; 

                    if(XpBar.Value >= XpBar.Maximum)
                    {
                        TriggerLevelUp();
                    }
                }
            }

 
        }

        private void StartGame_Click(object sender, RoutedEventArgs e)
        {
            // Namen für später speichern
            if (!string.IsNullOrWhiteSpace(TxtPlayerName.Text))
            {
                playerName = TxtPlayerName.Text;
            }

            player.currentHp = player.maxHp; // Volles Leben garantieren
            HpBar.Maximum = player.maxHp;
            HpBar.Value = player.currentHp;

            StartScreen.Visibility = Visibility.Collapsed;
            gameStarted = true;
            isPaused = false;
            stopwatch.Restart(); // stopwatch neustarten
            lastTime = stopwatch.Elapsed.TotalSeconds;

        }

        public void TriggerGameOver()
        {
            isPaused = true;
            gameStarted = false;
            stopwatch.Stop();

            // Zeit formatieren (Minuten:Sekunden)
            TimeSpan t = TimeSpan.FromSeconds(survivedTime);
            TxtStatsTime.Text = $"Überlebte Zeit: {t.Minutes:D2}:{t.Seconds:D2}"; //D2 = decimal with 2 digits. füllt die Zahl mit führenden nullen auf, falls sie einstellig ist
            TxtStatsKills.Text = $"Besiegte Gegner: {totalKills}";

            GameOverScreen.Visibility = Visibility.Visible;

            using (var connection = new SQLiteConnection(dbPath))
            {
                connection.Open();
                string insertQuery = "INSERT INTO Highscores (PlayerName, SurvivedTime, Kills) VALUES (@name, @time, @kills);";

                //Stats hinzufügen
                using (var command = new SQLiteCommand(insertQuery, connection))
                {
                    command.Parameters.AddWithValue("@name", playerName);
                    command.Parameters.AddWithValue("@time", survivedTime);
                    command.Parameters.AddWithValue("@kills", totalKills);
                    command.ExecuteNonQuery();
                }
            }

        }

        private void RestartGame_Click(object sender, RoutedEventArgs e)
        {
            GameOverScreen.Visibility = Visibility.Collapsed;

            // stats zurücksetzen
            totalKills = 0;
            survivedTime = 0;
            XpBar.Maximum = 10;
            XpBar.Value = 0;

            // lvl zurücksetzen
            speedLevel = 1;
            rangeLevel = 1;
            fireballLevel = 0;

            activeFireball = new Fireball();
            activeShuriken = null;
            activeGarlic = null;

            // alles vom screen löschen
            GameCanvas.Children.Clear();
            activeProjectilesList.Clear();
            enemies.Clear();
            ExpOrbsList.Clear();
            damageTexts.Clear();
            if (activeGarlic != null)
            {
                GameCanvas.Children.Remove(activeGarlic.auraVisual);
            }

            // Spieler zurücksetzen auf die standardeinstellungen
            player.playerXPos = 100;
            player.playerYPos = 100; 
            player.playerSpeed = 150; 
            player.pickupRange = 100;  
            GameCanvas.Children.Add(player.playerchar);
            gameStarted = true;
            isPaused = false;
            lastTime = stopwatch.Elapsed.TotalSeconds;
            stopwatch.Start();
        }

        private void BackToMenu_Click(object sender, RoutedEventArgs e)
        {
            GameOverScreen.Visibility = Visibility.Collapsed;
            StartScreen.Visibility = Visibility.Visible;

            totalKills = 0;
            survivedTime = 0;
            XpBar.Maximum = 10;
            XpBar.Value = 0;
            TxtInGameTimer.Text = "00:00";

            speedLevel = 1;
            rangeLevel = 1;
            fireballLevel = 1;
            garlicLevel = 0;
            shurikenLevel = 0;

            activeFireball = new Fireball();
            activeShuriken = null;
            activeGarlic = null;

            GameCanvas.Children.Clear();
            activeProjectilesList.Clear();
            enemies.Clear();
            damageTexts.Clear();
            if (activeGarlic != null)
            {
                GameCanvas.Children.Remove(activeGarlic.auraVisual);
            }

            GameCanvas.Children.Add(player.playerchar);
            player.maxHp = 100;
            player.currentHp = player.maxHp;
            HpBar.Maximum = player.maxHp;
            HpBar.Value = player.currentHp;
            damageCooldown = 0;

            gameStarted = false;
            isPaused = true;
            stopwatch.Reset();
        }

        private void TriggerLevelUp()
        {
            isPaused = true;
            stopwatch.Stop(); // Stopwatch stoppen damit delta time keine probleme macht sonst teleportation bei gegnern/projektilen
            LevelUpMenu.Visibility = Visibility.Visible;

            Random rnd = new Random();

            // Liste als array kopieren weil shuffle nur mit array funktioniert
            string[] poolArray = upgradePool.ToArray();

            // Array mischen
            rnd.Shuffle(poolArray);

            // 3 Items nehmen 
            var randomSelection = poolArray.Take(3).ToList();

            SetupUpgradeButton(BtnChoice1, randomSelection[0]);
            SetupUpgradeButton(BtnChoice2, randomSelection[1]);
            SetupUpgradeButton(BtnChoice3, randomSelection[2]);
        }

        private void SetupUpgradeButton(Button btn, string upgradeType)
        {
            btn.Tag = upgradeType; 

            switch (upgradeType)
            {
                case "speed":
                    btn.Content = $"Laufschuhe (Lvl {speedLevel} -> {speedLevel + 1})\nTempo: {player.playerSpeed} -> {player.playerSpeed + 20}";
                    break;

                case "range":
                    btn.Content = $"Magnet (Lvl {rangeLevel} -> {rangeLevel + 1})\nRadius: {player.pickupRange} -> {player.pickupRange + 25}";
                    break;

                case "fireball":
                    // Text je nach lvl dynamisch anpassen
                    // Für mehr level mehr if statements hinzufügen
                    string fireballStatText = "";
                    if (fireballLevel == 1)
                    {
                        fireballStatText = $"Schaden: {activeFireball.damage} -> {activeFireball.damage + 5}";
                    }
                    else if (fireballLevel == 2)
                    {
                        fireballStatText = $"Angriffstempo: {activeFireball.attacksPerSecond} -> {activeFireball.attacksPerSecond + 0.5}";
                    }
                    else if(fireballLevel == 3)
                    {
                        fireballStatText = $"Schaden: {activeFireball.damage} -> {activeFireball.damage + 5}";
                    }
                    else
                    {
                        fireballStatText = $"Schaden: {activeFireball.damage} -> {activeFireball.damage + 2}";
                    }

                    btn.Content = $"Feuerball (Lvl {fireballLevel} -> {fireballLevel + 1})\n{fireballStatText}";
                    break;

                case "shuriken":
                    string shurikenStatText = "";

                    if (shurikenLevel == 1)
                    { 
                        shurikenStatText = $"Angriffstempo: {activeShuriken.attacksPerSecond} -> {activeShuriken.attacksPerSecond + 0.5}"; 
                    }
                    else if(shurikenLevel == 2)
                    {
                        shurikenStatText = $"Schaden: {activeShuriken.damage} -> {activeShuriken.damage + 5}";
                    }
                    if (shurikenLevel == 0)
                    {
                        btn.Content = "Shuriken freischalten!\nWirft ein Shuriken in Laufrichtung";
                    }
                    else
                    {
                        btn.Content = $"Shuriken (Lvl {shurikenLevel} -> {shurikenLevel + 1})\n{shurikenStatText}";
                    }
                    break;

                case "garlic":
                    string garlicStatText = "";

                    if (garlicLevel == 0)
                    {
                        btn.Content = "Knoblauch freischalten!\nErzeugt eine schützende Schadens-Aura um dich.";
                    }
                    else if(garlicLevel == 1)
                    {
                        btn.Content = $"Knoblauch (Lvl {garlicLevel} -> {garlicLevel + 1})\nRadius +15";
                    }
                    else if(garlicLevel == 2)
                    {
                        btn.Content = $"Knoblauch (Lvl {garlicLevel} -> {garlicLevel + 1})\nDamage +1";
                    }
                    else if(garlicLevel == 3)
                    {
                        btn.Content = $"Knoblauch (Lvl {garlicLevel} -> {garlicLevel + 1})\nRadius +15";
                    }
                    else
                    {
                        btn.Content = $"Knoblauch (Lvl {garlicLevel} -> {garlicLevel + 1})\nDamage +1";
                    }
                        break;
            }
        }

        private void HandleUpgrade(object sender, RoutedEventArgs e)
        {
            Button clickedButton = (Button)sender;
            string upgradeType = clickedButton.Tag.ToString();

            switch (upgradeType)
            {
                case "speed":
                    player.playerSpeed += 20;
                    speedLevel++;
                    break;

                case "range":
                    player.pickupRange += 25;
                    // Neu berechnen damit es nach lvlup nicht verbuggt ist
                    player.pickupRangeDebug.Width = player.pickupRange * 2;
                    player.pickupRangeDebug.Height = player.pickupRange * 2;

                    Canvas.SetLeft(player.pickupRangeDebug, (player.playerXPos + player.playerchar.Width / 2) - cameraX - player.pickupRange);
                    Canvas.SetTop(player.pickupRangeDebug, (player.playerYPos + player.playerchar.Height / 2) - cameraY - player.pickupRange);
                    rangeLevel++;
                    break;

                case "fireball":
                    fireballLevel++;
                    if (fireballLevel == 2)
                    {
                        activeFireball.damage += 5;
                    }
                    else if (fireballLevel == 3)
                    {
                        activeFireball.attacksPerSecond += 0.5;
                    }
                    else if(fireballLevel == 4)
                    {
                        activeFireball.damage += 5;
                    }
                    else
                    {
                        activeFireball.damage += 2;
                    }
                    break;

                case "shuriken":
                    shurikenLevel++;
                    if (shurikenLevel == 1)
                    {
                        activeShuriken = new Shuriken();
                        shurikenLevel = 1;
                    }
                    if (shurikenLevel == 2)
                    {
                        activeShuriken.attacksPerSecond += 0.5;
                    }
                    else if (shurikenLevel == 3)
                    {
                        activeShuriken.damage += 5;
                    }
                    break;

                case "garlic":
                    garlicLevel++;
                    if (garlicLevel == 1)
                    {
                        activeGarlic = new Garlic();
                    }
                    else if(garlicLevel == 2)
                    {
                        activeGarlic.range += 15;
                    }
                    else if(garlicLevel == 3)
                    {
                        activeGarlic.damage += 1;
                    }
                    else if(garlicLevel == 4)
                    {
                        activeGarlic.range += 15;
                    }
                    else
                    {
                        activeGarlic.damage += 1;
                    }
                        break;
            }

            // UI schließen und Pause beenden
            LevelUpMenu.Visibility = Visibility.Collapsed;
            isPaused = false;

            stopwatch.Start(); // Stopwatch wieder starten
            

            // Xp Value weitergeben und gebrauchte Xp für lvl up erhöhen
            double currentOverfill = XpBar.Value - XpBar.Maximum;
            XpBar.Maximum *= 1.2;
            XpBar.Value = currentOverfill; // Falls man mehr XP gesammelt hat, als für das Lvl nötig war
        }

        private void Cleanup()
        {
            for (int i = enemies.Count - 1; i >= 0; i--)
            {
                if (enemies[i].isdead == true)
                {
                    GameCanvas.Children.Remove(enemies[i].enemychar);  // jeden enemychar entfernen bei dem isdead true ist
                    GameCanvas.Children.Remove(enemies[i].debugCenterPoint);
                    Random rnd = new Random();
                    int chance = rnd.Next(1, 101);

                    // Drop-Verteilung:
                    if (chance <= 2) // 2% Chance 
                    {
                        ExpOrbsList.Add(new RedOrb(enemies[i].centerX, enemies[i].centerY, GameCanvas));
                    }
                    else if (chance <= 7) // 5% Chance 
                    {
                        ExpOrbsList.Add(new GreenOrb(enemies[i].centerX, enemies[i].centerY, GameCanvas));
                    }
                    else // 93% Chance
                    {
                        ExpOrbsList.Add(new BlueOrb(enemies[i].centerX, enemies[i].centerY, GameCanvas));
                    }
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

            for (int i = ExpOrbsList.Count - 1; i >= 0; i--)
            {
                if (ExpOrbsList[i].isCollected == true)
                {
                    GameCanvas.Children.Remove(ExpOrbsList[i].visual);
                    GameCanvas.Children.Remove(ExpOrbsList[i].debugCenterPoint);
                    ExpOrbsList.RemoveAt(i);
                }

            }

            for(int i = damageTexts.Count -1; i >= 0; i--)
            {
                if (damageTexts[i].lifetime <= 0)
                {
                    GameCanvas.Children.Remove (damageTexts[i].visual);
                    damageTexts.RemoveAt(i);
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
            double spawnX = 0;
            double spawnY = 0;
            switch (rnd)
            {
                case 1:
                    spawnX = random.Next(minX, maxX);
                    spawnY = cameraY - 50; // oben
                    break;
                case 2:
                    spawnX = random.Next(minX, maxX);
                    spawnY = cameraY + windowHeight + 50; // unten
                    break;
                case 3:
                    spawnX = cameraX -50;
                    spawnY = random.Next(minY, maxY); // links
                    break;
                case 4:
                    spawnX = cameraX + windowWidth + 50;
                    spawnY = random.Next(minY, maxY); // rechts
                    break;
            }

            int r = random.Next(1, 101);

            if (survivedTime < 30) // ersten 30sec nur zombies
            {
                enemies.Add(new Zombie(spawnX, spawnY, GameCanvas));
            }
            else if (survivedTime >= 30 && survivedTime < 90) // 0:30 - 1:30 Fledermäuse
            {
                if (r <= 70) enemies.Add(new Zombie(spawnX, spawnY, GameCanvas)); // 70% Chance
                else enemies.Add(new Bat(spawnX, spawnY, GameCanvas)); // 30% Chance
            }
            else // ab 1:30 Oger
            {
                if (r <= 50) enemies.Add(new Zombie(spawnX, spawnY, GameCanvas)); // 50%
                else if (r <= 85) enemies.Add(new Bat(spawnX, spawnY, GameCanvas)); // 35%
                else enemies.Add(new Oger(spawnX, spawnY, GameCanvas)); // 15% 
            }

            // Spawn interval jedes mal wenn ein gegner gespawnt wird reduzieren für scaling
            if (spawnInterval > 0.5)
            {
                spawnInterval -= 0.005;
            }
        }



        private void DebugMode()
        {
            if (fIsPressed == true) // bei Debug on
            {
                Canvas.SetLeft(player.playerhitboxdebug, player.playerhitbox.Left - cameraX);
                Canvas.SetTop(player.playerhitboxdebug, player.playerhitbox.Top - cameraY);

                // Berechnet die vertikale Position des Sammel-Radius:
                // 1. (player.playerYPos + player.playerchar.Height / 2) -> Ermittelt die Mitte des Spielers
                // 2. - cameraY -> Rechnet die Welt-Position in die aktuelle Bildschirm-Position um
                // 3. - player.pickupRange -> Verschiebt den Kreis um seinen Radius nach oben, 
                //    damit sein Mittelpunkt (nicht die Ecke) auf dem Spieler liegt.
                Canvas.SetLeft(player.pickupRangeDebug, (player.playerXPos + player.playerchar.Width / 2) - cameraX - player.pickupRange);
                Canvas.SetTop(player.pickupRangeDebug, (player.playerYPos + player.playerchar.Height / 2) - cameraY - player.pickupRange);

                foreach (Enemy enemy in enemies)
                {
                    if(GameCanvas.Children.Contains(enemy.debugCenterPoint) == false)
                    {
                        GameCanvas.Children.Add(enemy.debugCenterPoint);
                    }
                    Canvas.SetLeft(enemy.debugCenterPoint, enemy.centerX - cameraX);
                    Canvas.SetTop(enemy.debugCenterPoint, enemy.centerY - cameraY);
                }

                foreach(ExperienceOrb orb in ExpOrbsList)
                {
                    if(GameCanvas.Children.Contains(orb.debugCenterPoint) == false)
                    {
                        GameCanvas.Children.Add(orb.debugCenterPoint);
                    }
                    Canvas.SetLeft(orb.debugCenterPoint, orb.orbXPos - cameraX - (orb.debugCenterPoint.Width / 2)); // debugCenterPoint / 2 für den Mittelpunkt
                    Canvas.SetTop(orb.debugCenterPoint, orb.orbYPos - cameraY - (orb.debugCenterPoint.Height / 2));
                }
                if (debugmode == false)
                {                   
                    GameCanvas.Children.Add(player.playerhitboxdebug);
                    GameCanvas.Children.Add(player.pickupRangeDebug);
                    debugmode = true;
                }

            }

            if (fIsPressed == false && debugmode == true) // bei Debug off
            {
                
                foreach (Enemy enemy in enemies)
                {
                    GameCanvas.Children.Remove(enemy.debugCenterPoint);
                }

                foreach(ExperienceOrb orb in ExpOrbsList)
                {
                    GameCanvas.Children.Remove(orb.debugCenterPoint);
                }

                GameCanvas.Children.Remove(player.playerhitboxdebug);
                GameCanvas.Children.Remove(player.pickupRangeDebug);
                debugmode = false;
            }
        }

        private void InitializeDatabase()
        {
            using (var connection = new SQLiteConnection(dbPath))
            {
                connection.Open();

                // Tabelle erstellen
                // REAL ist wie Datentyp double
                string createTableQuery = @"
                CREATE TABLE IF NOT EXISTS Highscores (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                PlayerName TEXT,
                SurvivedTime REAL,
                Kills INTEGER
            );";

                using (var command = new SQLiteCommand(createTableQuery, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }

        private void OpenHighscores_Click(object sender, RoutedEventArgs e)
        {
            StartScreen.Visibility = Visibility.Collapsed;
            HighscoreScreen.Visibility = Visibility.Visible;

            TxtHighscoreList.Text = string.Format("{0,-15} | {1,-10} | {2,-6}\n", "NAME", "ZEIT", "KILLS");
            TxtHighscoreList.Text += "----------------------------------------\n";

            using (var connection = new SQLiteConnection(dbPath))
            {
                connection.Open();
                // Holt die besten 5 Einträge, sortiert nach überlebter Zeit
                string selectQuery = "SELECT PlayerName, SurvivedTime, Kills FROM Highscores ORDER BY SurvivedTime DESC LIMIT 5;";

                using (var command = new SQLiteCommand(selectQuery, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string name = reader.GetString(0);
                            double timeInSec = reader.GetDouble(1);
                            int kills = reader.GetInt32(2);

                            TimeSpan t = TimeSpan.FromSeconds(timeInSec);
                            string formattedTime = $"{t.Minutes:D2}:{t.Seconds:D2}";

                            // Formatiert den Text in saubere Spalten 
                            TxtHighscoreList.Text += string.Format("{0,-15} | {1,-10} | {2,-6}\n", name, formattedTime, kills);
                        }
                    }
                }
            }
        }

        private void CloseHighscores_Click(object sender, RoutedEventArgs e)
        {
            HighscoreScreen.Visibility = Visibility.Collapsed;
            StartScreen.Visibility = Visibility.Visible;
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