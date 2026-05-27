## Spielprinzip

Dieses Spiel ist ein actiongeladenes **2D-Action-Roguelike (Bullet Heaven / Horde Survival)**. Das Ziel ist denkbar einfach, aber herausfordernd: **Überlebe so lange wie möglich gegen unendliche Horden von Monstern!**

### Kern-Gameplay:
1. **Dauerhafte Bewegung:** Du steuerst den Helden mit <kbd>W</kbd><kbd>A</kbd><kbd>S</kbd><kbd>D</kbd> durch eine offene Welt. Deine Waffen (wie der Feuerball, Shuriken oder Knoblauch-Aura) greifen **vollautomatisch** in festen Zeitintervallen an. Du musst dich voll und ganz auf das Ausweichen konzentrieren!
2. **EXP sammeln & Aufleveln:** Besiegte Gegner lassen Erfahrungsorbs fallen. Sammelst du genug davon ein, steigst du ein Level auf.
3. **Mächtige Upgrades:** Bei jedem Level-Up pausiert das Spiel und du darfst aus zufälligen Upgrades wählen. Verbessere dein Lauftempo, erhöhe dein maximales Leben oder verstärke deine Waffen in (mehr Schaden, größere Reichweite, höheres Angriffstempo).
4. **Strategisches Heilen:** Bist in Bedrängnis, kannst du saftige Brathähnchen wählen, um deine Lebenspunkte mitten im Kampf wieder aufzufüllen.

**Das Motto lautet:** Jede Sekunde spawnen mehr Gegner (Zombies, Fledermäuse, Oger). Wie viele Kills schaffst du, bevor du überrannt wirst?
Die **Highscoreliste** zeigt die 5 besten Runs an, sortiert nach überlebter Zeit.


### Steuerung
| Taste | Aktion |
| :---: | --- |
| <kbd>W</kbd> | Nach **oben** bewegen |
| <kbd>A</kbd> | Nach **links** bewegen |
| <kbd>S</kbd> | Nach **unten** bewegen |
| <kbd>D</kbd> | Nach **rechts** bewegen |

### Debug-Modus
Für Testzwecke gibt es einen Debug-Modus, um bestimmtes Verhalten zu analysieren.
Der Debugmodus hat folgende Features:
- Zentrum der Gegner
- Zentrum der Exp Orbs
- Spieler Exp Pickup range
- Spieler hitbox

* **<kbd>F</kbd> – Debug-Modus umschalten:** Aktiviert/Deaktiviert die Anzeige der unsichtbaren Hitboxen, des Sammelradius (Magnet) und der Mittelpunkte der Gegner und Exp Orbs.
* **<kbd>X</kbd> – Sofortiges Game Over:** *Nur aktiv, wenn der Debug-Modus eingeschaltet ist!* Löst sofort manuell ein Game Over aus, um z. B. den Highscore-Speicher oder den Game-Over-Bildschirm zu testen.

## Verwendete Lernressourcen

https://www.w3schools.com/cs/index.php | Vorallem C# Classes section

https://docs.monogame.net/articles/tutorials/building_2d_games/12_collision_detection/index.html | Kollisionen berechnen

https://www.mooict.com/category/c-sharp-tutorials/ | Verschiedenes

https://www.mooict.com/category/wpf-c-sharp-tutorials/ | Verschiedenes

https://learn.microsoft.com/de-de/dotnet/desktop/wpf/graphics-multimedia/painting-with-solid-colors-and-gradients-overview | Styling für die Gegner

https://learn.microsoft.com/de-de/dotnet/api/system.windows.input.keyeventargs?view=netframework-4.8.1 | Tastatur-Input

https://learn.microsoft.com/de-de/dotnet/csharp/fundamentals/object-oriented/objects | Objekte

https://learn.microsoft.com/de-de/dotnet/csharp/fundamentals/object-oriented/inheritance | Vererbung

https://www.gamedeveloper.com/programming/how-to-structure-a-game | Struktur

http://devmag.org.za/2009/04/13/basic-collision-detection-in-2d-part-1/ | Kollision

https://gamedev.stackexchange.com/questions/201559/how-to-measure-deltatime | DeltaTime Berechnung

https://www.youtube.com/watch?v=r3CExhZgZV8&list=PLZPZq0r_RZOPNy28FDBys3GVP2LiaIyP_ | Verschiedenes (Listen, Konstructor, Objekte, Inheritance)

https://learn.microsoft.com/en-us/dotnet/desktop/wpf/graphics-multimedia/how-to-render-on-a-per-frame-interval-using-compositiontarget | Um jeden Frame den gameloop aufzurufen

https://forums.unrealengine.com/t/how-do-i-make-floating-combat-text-aka-floating-number-over-actor/288965 | Idee für die Umsetzung der Schadensanzeige

https://learn.microsoft.com/de-de/dotnet/standard/data/sqlite/?tabs=net-cli | Datenbank Anbindung

https://learn.microsoft.com/en-us/dotnet/api/system.windows.controls.progressbar?view=netframework-4.8.1 | XP und HP bar

