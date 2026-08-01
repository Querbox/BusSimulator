# Bus-Depot System

## Überblick

Das Bus-Depot ist das Herzstück des Spiels. Hier starten alle Fahrten und Busse werden geparkt.

## Realistische Features

### 📍 Standort
- **HVB Hechingen** - Bushaltestelle in der Brunnenstraße 11, 72379 Hechingen
- Basierend auf echten Koordinaten (Lat: 48.37915, Lon: 8.75095)

### 🏢 Infrastruktur

#### Gebäude
- **Verwaltungs-/Bürogebäude** mit Fenstern und Eingang
- **Wartungshalle/Garage** für Reparaturen
- **Tankstelle** mit Diesel-Reservoir

#### Parkplätze
- **12 Parkplätze** für reguläre Busse (2 Reihen á 6 Plätze)
- **2 Wartungs-Parkplätze** (gelb markiert)
- Automatische Platz-Verwaltung
- Status-Anzeige (Grün = frei, Rot = besetzt, Gelb = Wartung)

### 🚌 Bus-Verwaltung

Zu Spielbeginn stehen 3 Busse zur Verfügung:

1. **HEC-001** (Linie 753)
   - Route: Hechingen → Boll → Burg Hohenzollern
   - Status: Ready (kann sofort gewählt werden)
   - Nächste Abfahrt: 15 Min

2. **HEC-002** (Linie 752)
   - Route: Schulverkehr Hechingen
   - Status: Ready
   - Nächste Abfahrt: 30 Min

3. **HEC-003** (Linie 754)
   - Route: Hohenzollern Express (Touristen)
   - Status: Fueling (wird gerade betankt)
   - Nächste Abfahrt: 45 Min

## Scripts

### BusDepot.cs
Haupt-Verwaltungs-Script für das Depot:
- Erstellt Gebäude-Struktur
- Verwaltet Parkplätze
- Tracks Bus-Status
- Methoden:
  - `GetAvailableParkingSpot()` - Nächsten freien Platz finden
  - `ParkBus(busObject, spotId)` - Bus parken
  - `UnparkBus(spotId)` - Bus abholen

### ParkingSpotMarker.cs
Visualisiert jeden Parkplatz:
- Farbcodierung (Grün/Rot/Gelb)
- Echtzeit-Status-Updates

### DepotManager.cs
Verwaltet den Start-Screen und Bus-Auswahl:
- Zeigt verfügbare Busse an
- Ermöglicht Bus-Auswahl mit Tasten (1-3)
- Startet das Spiel mit gewähltem Bus

### SceneInitializerV2.cs
Koordiniert alle Komponenten:
- Lädt OSM-Daten
- Erstellt Depot
- Spawnt Spieler-Bus
- Initialisiert Kamera und Manager

## Spielablauf

### Start-Szenario
1. Spiel startet
2. Depot-Screen zeigt verfügbare Busse
3. Spieler wählt Bus mit Tasten 1-3
4. Bus wird in Depot an Parkplatz positioniert
5. Spieler steigt ein und fährt los!

### Steuerung im Depot
- **1/2/3**: Bus auswählen
- **ESC**: Spiel abbrechen

### Steuerung beim Fahren
- **W/S**: Gas/Bremse rückwärts
- **A/D**: Lenken
- **Space**: Notbremse
- **E**: Motor Start/Stop
- **C**: Kamera-Modus wechseln
- **Numpad 1-4**: Spezifische Kamera-Modi

## Zukünftige Features

- [ ] Fahrgast-Boarding-Animation
- [ ] Realistische Tank-/Fuel-System
- [ ] Bus-Wartungs-Zustand
- [ ] Depot-3D-Tour vor Fahrt-Start
- [ ] Mehrspielmodus (andere Busse sehen)
- [ ] Tagesabschluss im Depot (Statistiken)

## Technische Details

### Koordinaten-System
- Depot-Zentrum: (0, 0, 0)
- X-Achse: Ost-West
- Y-Achse: Höhe
- Z-Achse: Nord-Süd

### Parkplatz-Layout
```
Reihe 1 (Z=0):    [P1] [P2] [P3] [P4] [P5] [P6]
                                              [Wartung1] [Wartung2]
Reihe 2 (Z=8):    [P7] [P8] [P9] [P10] [P11] [P12]

Gebäude:         [Büro/Admin]      [Tankstelle]    [Garage]
```
