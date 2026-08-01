# AUTO SETUP - So startest du das Spiel!

## 🚀 **Schnellstart (2 Schritte):**

### **Schritt 1: Scene öffnen**
1. Öffne Unity
2. Öffne/Erstelle eine Scene:
   - `Assets/Scenes/MainScene.unity`
   - Falls nicht vorhanden: Rechtsklick → New Scene → Speichern als `MainScene`

### **Schritt 2: Auto-Setup Script hinzufügen**
1. Erstelle ein leeres GameObject:
   - Hierarchy (links) → Rechtsklick → "Create Empty"
   - Benenne es: `Startup`
2. Im **Inspector** (rechts):
   - Klick "Add Component"
   - Suche: `AutoSetup`
   - Hinzufügen!
3. **Play drücken!** (Ctrl+P)

Das war's! ✅

---

## 🎮 **Was passiert automatisch:**

✅ Kamera wird eingerichtet  
✅ Licht/Sonne wird erstellt  
✅ GameManager wird initialisiert  
✅ OSM-Karte wird geladen  
✅ Bus-Depot wird erstellt  
✅ Spieler-Bus wird gespawnt  
✅ Alle Scripts werden verbunden  

---

## 🎮 **Im Spiel:**

**Console öffnen** (Fenster oben → Console):
- Depot-Screen mit verfügbaren Bussen
- Wähle Bus: **Taste 1, 2 oder 3**
- Fahre los! 🚌

---

## 🎮 **Steuerung während Fahrt:**

| Taste | Funktion |
|-------|----------|
| **W/S** | Gas / Rückwärts |
| **A/D** | Lenken |
| **Space** | Bremse |
| **E** | Motor Start/Stop |
| **C** | Kamera-Modus wechseln |
| **Numpad 1-4** | Spezifische Kamera |
| **Alt + Mouse** | Freie Kamera (First Person) |

---

## ❌ **Falls Fehler auftritt:**

1. **Scripts kompilieren nicht?**
   - Warte 10-20 Sekunden
   - Console anschauen
   - Fehler kopieren und beheben

2. **"AutoSetup nicht gefunden"?**
   - Stelle sicher, dass `AutoSetup.cs` in `Assets/Scripts/Setup/` ist
   - Dann: Play drücken

3. **Schwarzer Screen?**
   - Kamera ist wahrscheinlich zu nah
   - Warte oder drücke C zum Kamera-Modus wechseln

---

## 💡 **Tipps:**

- **Console anschauen** (Window → General → Console) für Debug-Info
- **Scene View** (Tab oben) um die Welt zu sehen
- **Gizmos aktivieren** um Bus/Haltestellen zu sehen

---

**Viel Spaß! 🚌🎮**
