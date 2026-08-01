# Setup-Anleitung

## Voraussetzungen

- Unity 6.5 (6000.5.6f1), passend zu `ProjectSettings/ProjectVersion.txt`
- Git installiert
- mindestens 5 GB freier Speicherplatz

## Installation

1. **Repository klonen**
   ```bash
   git clone <repository-url>
   cd BusSimulator
   ```

2. **Projekt in Unity öffnen**
   - Öffne Unity Hub
   - Klicke auf "Add"
   - Wähle den Ordner des Projekts
   - Öffne das Projekt

3. **Szene laden**
   - Öffne `Assets/Scenes/Main.unity`
   - Drücke Play (Ctrl+P)

## Input-System- oder `TreeView`-Fehler beheben

Das Projekt verwendet Input System 1.17.0, weil ältere Paketversionen wie 1.14.0
mit Unity 6.5 noch die inzwischen entfernte, nicht generische `TreeView`-API
verwenden. Dateien unter `Library/PackageCache` dürfen nicht von Hand geändert
werden: Unity erzeugt diesen Ordner erneut.

Falls nach einem Update weiterhin `CS0619` für `TreeView`, `TreeViewItem` oder
`TreeViewState` aus `Library/PackageCache/com.unity.inputsystem...` erscheint:

1. Unity und Unity Hub schließen.
2. Im Projektordner `Library/PackageCache` löschen. Falls vorhanden, zusätzlich
   `Packages/packages-lock.json` löschen.
3. Das Projekt wieder mit Unity 6.5 (6000.5.6f1) öffnen und warten, bis der
   Package Manager Input System 1.17.0 neu installiert und die Skripte kompiliert
   hat.

Nicht auf **Assets > Reimport All** ausweichen: Ein Reimport ersetzt kein
inkompatibles Paket und behält den Package-Manager-Cache möglicherweise bei.

## Steuerung

- **W/S oder Pfeiltasten**: Vorwärts/Rückwärts
- **A/D oder Pfeiltasten**: Links/Rechts lenken
- **Space**: Bremse
- **ESC**: Pause

## Nächste Schritte

- [ ] OSM-Daten importieren
- [ ] 3D-Modelle für Bus, Gebäude, etc.
- [ ] Haltestellen-System implementieren
- [ ] Passagier-AI entwickeln
- [ ] UI-Menüs erstellen

## Hilfreiche Links

- [Unity Documentation](https://docs.unity3d.com/)
- [OpenStreetMap](https://www.openstreetmap.org/)
- [OSM2World](https://osm2world.org/)
