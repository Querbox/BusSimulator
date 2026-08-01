# Projekt Übersicht

## 📁 Ordnerstruktur

```
BusSimulator/
├── Assets/
│   ├── Scripts/
│   │   ├── Bus/
│   │   │   ├── BusController.cs
│   │   │   ├── BusModel.cs
│   │   │   └── BusPhysicsController.cs
│   │   ├── Camera/
│   │   │   └── CameraController.cs
│   │   ├── Depot/
│   │   │   ├── BusDepot.cs
│   │   │   └── ParkingSpotMarker.cs
│   │   ├── Game/
│   │   │   ├── GameManager.cs
│   │   │   ├── DepotManager.cs
│   │   │   ├── SceneInitializer.cs
│   │   │   └── SceneInitializerV2.cs
│   │   ├── Map/
│   │   │   ├── MapManager.cs
│   │   │   ├── MapManagerV2.cs
│   │   │   ├── RealisticRouteManager.cs
│   │   │   ├── OSMImporter.cs
│   │   │   └── OSMDataGenerator.cs
│   │   ├── Passenger/
│   │   │   └── PassengerManager.cs
│   │   ├── UI/
│   │   │   └── HUD.cs
│   │   └── Setup/
│   │       └── AutoSetup.cs          ← NEUES AUTO-SETUP!
│   ├── Scenes/
│   │   └── MainScene.unity           ← Hauptszene
│   ├── Models/
│   ├── Materials/
│   ├── Data/
│   │   ├── routes.json
│   │   ├── realistic_routes_data.json
│   │   └── map_export.osm
│   └── Resources/
├── Data/
├── Docs/
│   ├── SETUP.md
│   ├── ARCHITECTURE.md
│   ├── OSM_CAMERA_BUSES.md
│   ├── DEPOT_SYSTEM.md
│   ├── QUICKSTART.md              ← NEUE ANLEITUNG!
│   └── README.md
├── ProjectSettings/
├── .gitignore
└── README.md
```

## 🎮 Spiel-Features

### ✅ Implementiert
- **Bus-Simulator** mit realistischer Physik
- **4 Kamera-Modi** (First Person, Third Person, Orbiting, Cinematic)
- **Realistische Buslinien** (Linie 753, 752, 754)
- **9 Bushaltestellen** in Hechingen, Boll, Burg Hohenzollern
- **Bus-Depot** mit 12 Parkplätzen + 2 Wartungsplätzen
- **OpenStreetMap-Integration** für echte Straßen
- **3D-Bus-Modelle** prozedural generiert
- **Auto-Setup** für instant Play

### 🔄 In Entwicklung
- Fahrgast-AI
- Fahrplan-Synchronisation
- Tank-/Fuel-System
- Sounds & Musik
- Route-Editor
- Mehrspieler-Support

## 🚀 Schnellstart

1. **Scene öffnen**: `Assets/Scenes/MainScene.unity`
2. **AutoSetup hinzufügen** (siehe QUICKSTART.md)
3. **Play drücken!** (Ctrl+P)
4. **Bus wählen** (Taste 1, 2 oder 3)
5. **Fahren!** 🚌

## 🛠️ Technologie-Stack

- **Engine**: Unity 2022 LTS
- **Sprache**: C#
- **Kartendaten**: OpenStreetMap (OSM)
- **Physik**: Unity Rigidbody
- **Daten-Format**: JSON

## 📊 Statistiken

- **C# Scripts**: 16
- **Buslinien**: 3 (Linie 753, 752, 754)
- **Bushaltestellen**: 9
- **Parkplätze**: 12 + 2 Wartung
- **Kamera-Modi**: 4
- **Zeilen Code**: ~3000+

## 📝 Lizenz

Private

## 👤 Entwickelt von

Fabian (Querbox)

## 🐛 Bug-Reports & Feature-Requests

Bitte GitHub Issues nutzen!
