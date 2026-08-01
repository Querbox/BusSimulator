# Bus Simulator 3D

Ein realistischer 3D-Bus-Simulator mit echten Straßendaten und Haltestellen.

## Features

- 🚌 Realistische Bus-Fahrdynamik
- 🗺️ Echte Straßendaten aus OpenStreetMap
- 👥 Fahrgäste aufsammeln und absetzen
- ⏰ Fahrpläne einhalten
- 🎮 Immersive 3D-Perspektive

## Technologie

- **Engine**: Unity 6.5 (6000.5.6f1, 2026 Supported Release)
- **Sprache**: C#
- **Karten-Daten**: OpenStreetMap
- **3D-Grafik**: Universal Render Pipeline (URP)

## Projekt-Struktur

```
BusSimulator/
├── Assets/
│   ├── Scripts/
│   │   ├── Bus/
│   │   ├── GameManager/
│   │   ├── Map/
│   │   ├── Passenger/
│   │   └── UI/
│   ├── Models/
│   ├── Scenes/
│   ├── Materials/
│   └── Data/
├── Data/
│   └── map_export.osm
├── Docs/
└── ProjectSettings/
```

## Setup

1. Unity 6.5 (6000.5.6f1, 2026 Supported Release) installieren
2. Project clonen: `git clone <repo>`
3. In Unity Hub über **Add project from repository** `Querbox/BusSimulator` und Branch `main` auswählen oder den geklonten Ordner öffnen. Unity erkennt das Repository durch `ProjectSettings/ProjectVersion.txt` automatisch als Unity-Projekt.
4. In Unity öffnen
5. Scene laden: `Assets/Scenes/MainScene.unity`
6. Play drücken! 🎮

## Lizenz

Private
