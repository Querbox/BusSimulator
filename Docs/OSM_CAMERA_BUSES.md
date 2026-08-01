# OSM-Import, Bus-Modelle und Kamera-System

## OSM (OpenStreetMap) Import

### OSMImporter.cs
- **Funktion**: Importiert OSM-Daten und konvertiert sie zu Unity-Geometrie
- **Features**:
  - Parst OSM-Nodes und Ways
  - Bestimmt automatisch Way-Typ (Straße, Gebäude, Wasser, Grünflächen)
  - Konvertiert Lat/Lon zu lokalen Unity-Koordinaten
  - Erstellt visuelle Repräsentationen

### OSMDataGenerator.cs
- **Funktion**: Generiert realistische Test-Daten für Hechingen-Region
- **Enthält**:
  - Hauptstraßen (Süd-Nord, Ost-West)
  - Gebäude-Polygone
  - Korrekte Koordinaten für alle Orte

## Bus-Modelle

### BusModel.cs
- **Generiert**: 3D-Bus-Modell prozedural
- **Komponenten**:
  - Bus-Körper (konfigurabale Größe)
  - 4 Räder mit korrekter Positionierung
  - 4 Fenster entlang der Seite
  - Tür (Einstieg)
  - Material und Farbe (standardmäßig rot)

### BusPhysicsController.cs
- **Physik**:
  - Realistische Beschleunigung/Bremse
  - Lenkverhalten
  - Fahrzeug-Gewicht (12 Tonnen, erhöht sich mit Fahrgästen)
  - Reibung und Abbremsen
  - Neigung bei Kurven
- **Features**:
  - Motor Start/Stop (E-Taste)
  - Fahrgast-Management
  - Dynamisches Gewicht

## Kamera-System

### CameraController.cs
- **4 Kamera-Modi**:

  1. **First Person (Fahrer-Sicht)**
     - Kamera aus Fahrer-Perspektive
     - Optional: Mouse-Look mit Alt
  
  2. **Third Person (Verfolgungskamera)**
     - Folgt Bus von hinten/oben
     - Schaut nach vorne
  
  3. **Orbiting (Umlaufbahn)**
     - Kamera umkreist den Bus
     - Automatische Rotation
  
  4. **Cinematic (Automatisch)**
     - Wechselt zwischen verschiedenen Kamerapositionen
     - Filme-ähnliche Perspektive

- **Steuerung**:
  - `C` = Nächster Kamera-Modus
  - `Numpad 1-4` = Direkter Modus-Wechsel
  - `Alt + Mouse` = Freie Kamera-Rotation (First Person)

## Steuerung im Spiel

- **W/S oder Pfeiltasten**: Gas / Bremse rückwärts
- **A/D oder Pfeiltasten**: Links / Rechts lenken
- **Space**: Bremse
- **E**: Motor Start/Stop
- **C**: Kamera-Modus wechseln
- **Numpad 1-4**: Spezifische Kamera-Modi
- **Alt + Mouse**: Freie Kamera-Kontrolle

## SceneInitializer.cs
- **Zentrale Szenen-Verwaltung**:
  - Lädt OSM-Daten
  - Spawnt Bus an Start-Position
  - Initialisiert Kamera
  - Verbindet alle Komponenten

## Verwendung

1. Erstelle eine neue leere Scene in Unity
2. Erstelle ein GameObject "SceneManager"
3. Füge SceneInitializer-Script hinzu
4. Weise OSMImporter, RealisticRouteManager in Inspector zu
5. Play drücken!

## Nächste Schritte

- [ ] Echte OSM-Datei-Importer (XML-Parser)
- [ ] 3D-Bus-Modelle mit Details
- [ ] Straßen-Physik (Fahrbahn-Erkennung)
- [ ] Fahrgast-Animation
- [ ] Sounds & Musik
