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

## Eingabe- und Paket-Cache

Das Projekt verwendet den in Unity integrierten Input Manager. Das separate
Paket `com.unity.inputsystem` wird bewusst nicht eingebunden, weil dessen
Editorcode mit Unity 6000.5 unter anderem durch die als Fehler markierte
veraltete `EntityId`-Konvertierung (`CS0619`) die Projektkompilierung blockieren
kann. Quellcode unter `Library/PackageCache` darf nicht von Hand geändert werden.

Falls Unity nach dem Wechsel weiterhin Fehler aus einem Cache-Ordner wie
`com.unity.inputsystem@<hash>` meldet:

1. Unity und Unity Hub für dieses Projekt schließen.
2. Den Ordner `Library/PackageCache` löschen.
3. Das Projekt erneut mit Unity 6000.5.6f1 öffnen und die Paketauflösung abwarten.

Unity stellt den Cache ohne das inkompatible Paket aus `Packages/manifest.json`
wieder her. Der Ordner
`Library` gehört nicht zum Quellcode und wird nicht committed.

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
