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
- **3D-Grafik**: Universal Render Pipeline 17.5.0
- **Eingabe**: Unity Input Manager

Die verbindliche Versionsbasis steht in `ProjectSettings/ProjectVersion.txt` und
`Packages/manifest.json`. Runtime-Materialien verwenden URP/Lit, Eingaben laufen
über Unitys integrierten Input Manager und Rigidbody-Bewegungen werden in
`FixedUpdate` über die aktuelle Unity-6-Physik-API ausgeführt.

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

1. Unity 6.5 (6000.5.6f1) installieren. Diese exakte Editorversion entspricht der eingecheckten Projektkonfiguration.
2. Project clonen: `git clone <repo>`
3. In Unity Hub über **Add project from repository** `Querbox/BusSimulator` und Branch `main` auswählen oder den geklonten Ordner öffnen. Unity erkennt das Repository durch `ProjectSettings/ProjectVersion.txt` automatisch als Unity-Projekt.
4. In Unity öffnen
5. Scene laden: `Assets/Scenes/Main.unity`
6. Play drücken! 🎮

## Fehlerbehebung: Paket- und Compiler-Cache

Wenn Unity meldet, dass unveränderliche Pakete unerwartet geändert wurden,
ein entferntes Paket wie `com.unity.inputsystem` weiterhin kompiliert oder
`UnityEngine.UI` trotz des Eintrags in `Packages/manifest.json` nicht gefunden
wird, ist in der Regel der generierte lokale Paket- oder Bee-Cache beschädigt. Der fehlende
`updates.txt`-Hinweis ist dabei ein Folgefehler des fehlgeschlagenen
Kompilierungslaufs.

1. Unity und Unity Hub für dieses Projekt schließen.
2. Im Projektordner ausführen:

   ```bash
   ./Tools/reset-unity-package-cache.sh
   ```

3. Das Projekt mit der in `ProjectSettings/ProjectVersion.txt` angegebenen
   Editorversion erneut öffnen und die Wiederherstellung sowie den vollständigen
   Asset-Import abwarten.

Das Skript entfernt ausschließlich den ignorierten, von Unity erzeugten
`Library`-Ordner. Projektinhalte unter `Assets`, die Paketanforderungen unter
`Packages` und Projekteinstellungen bleiben erhalten. Dateien, die Unity im
Package-Manager unter `Packages/com.unity.*` anzeigt, sollten nicht von Hand
bearbeitet werden.

## Lizenz

Private
