# Frosty Targets – 3AHMNS

Unity-Version: **6000.3.24f1**. Projekt: Frosty Targets.

## Projekt öffnen
1. Unity Hub öffnen, **Add project from disk** wählen und diesen Ordner auswählen.
2. Unity **6000.3.24f1** verwenden und den ersten Paketimport abwarten.
3. Szene `Assets/MyGame/Scenes/ShootingScene.unity` öffnen, **Play** drücken.
4. `Assets/MyGame/Scripts/GameManager.cs` und `Target.cs` untersuchen.

## Hinweis bei `UnityEngine.UI` / `Text`-Fehlern
Das Paket **Unity UI (uGUI)** ist unter `Packages/manifest.json` als `com.unity.ugui` eingetragen. Falls trotzdem CS0234 oder CS0246 erscheint: `Window > Package Management > Package Manager` öffnen und unter **In Project** prüfen, ob Unity UI geladen ist. Im Paketmanager **Refresh/Resolve** ausführen; gegebenenfalls Unity schließen und den Ordner `Library` löschen, dann erneut öffnen. Nicht die `using UnityEngine.UI`-Zeile entfernen: sie wird für die vorhandene Punkteanzeige gebraucht.

## Unterricht
Das vorhandene Spawning, der Score und der Sieg bei zehn Treffern sind **Ausgangscode**. Schüler:innen erweitern diese Mechaniken anhand der GitHub-Issues. Der Erstimport ist eine Ausgangsversion; keine Aufgaben sind vorweg implementiert.

## Analyse des Ausgangscodes

### Ablauf: Erzeugen → Anklicken → Punkte → Spielende
1. **Erzeugen:** Nach 1 Sekunde erzeugt der GameManager jede Sekunde ein neues Ziel an einer zufälligen Position.
2. **Anklicken:** Jeder Linksklick spielt einen Schuss-Sound ab. Trifft der Klick ein Ziel, meldet das Ziel den Treffer an den GameManager und verschwindet. Wird ein Ziel nicht getroffen, löscht es sich nach 2 Sekunden selbst.
3. **Punkte:** Jeder Treffer erhöht den Score um 1. Der neue Wert wird sofort in der Punkteanzeige (UI-Text) angezeigt.
4. **Spielende:** Bei 10 Punkten ist das Spiel gewonnen. Das Sieg-Bild wird eingeblendet und es entstehen keine neuen Ziele mehr.


### Bereits implementiert
- **Zufallsposition:** `Random.Range` wählt x zwischen −365 und 365 und y zwischen −202 und 202. Es wird `localPosition` benutzt, weil das Ziel ein Kind von `parentOfTargets` ist.
- **Punkteanzeige:** Ein `Text`-Feld (`scoreText`) zeigt den aktuellen Score und wird in `IncrementScore()` bei jedem Treffer aktualisiert.
- **Siegbedingung:** Ab `score >= 10` wird `win = true` gesetzt, das Sieg-Bild (`winSprite`) eingeblendet und das Spawnen gestoppt.

### Geplante Änderungen
- **Sound nur bei Treffer:** Der Schuss-Sound soll nicht bei jedem Klick kommen, sondern passend zum Treffer bzw. Fehlschuss.
- **Neustart:** Nach dem Sieg soll man das Spiel neu starten können, ohne die Szene neu zu laden.
- **Fehlschüsse bestrafen:** Daneben klicken oder ein Ziel verpassen soll Punkte kosten.
- **Einstellbare Werte:** Spawn-Intervall, Lebensdauer der Ziele und Siegpunktzahl sollen im Inspector einstellbar sein statt fest im Code.
- **Timer:** Eine Zeitbegrenzung, damit man auch verlieren kann.


### Methoden
- **`Start()`** (GameManager): Startet mit `InvokeRepeating` das wiederholte Aufrufen von `Spawn()`, zum ersten Mal nach 1 s, danach jede Sekunde.
- **`Start()`** (Target): Sucht den GameManager in der Szene, merkt ihn sich und zerstört das Ziel automatisch nach 2 s.
- **`Update()`** (GameManager): Läuft jeden Frame. Nach dem Sieg stoppt es das Spawnen (`CancelInvoke`), und bei jedem Linksklick spielt es den Schuss-Sound ab.
- **`Spawn()`** (GameManager): Erzeugt eine Kopie des Ziel-Prefabs als Kind von `parentOfTargets` und setzt sie auf eine zufällige `localPosition`.
- **`OnMouseDown()`** (Target): Wird beim Anklicken des Ziels aufgerufen, ruft `IncrementScore()` im GameManager auf und zerstört das Ziel.