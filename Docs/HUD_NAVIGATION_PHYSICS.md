# UI & HUD, Navigation & Straßen-Physik

## 🎮 HUD-System (HUDSystem.cs)

### Features:
- **Speedometer** - Echtzeit Geschwindigkeit (km/h)
- **RPM Anzeige** - Motor-Drehzahl
- **Fahrgast-Zähler** - Aktuelle Fahrgäste / Kapazität (0/50)
- **Digital Uhr** - Spielzeit im Format HH:MM
- **Fuel-Anzeige** - Tankstand mit Prozent-Balken
- **Navigation Display**:
  - Nächste Haltestelle Name
  - Entfernung in Metern
  - Kompass-Pfeil (Richtungspfeil)

### UI-Layout:
```
┌─────────────────────────────────────┐
│         Uhrzeit (08:00)             │
│      Fahrgäste (0/50)               │
└─────────────────────────────────────┘

┌──────────────────┐  ┌──────────────────┐
│  Geschwindigkeit │  │ Nächste Halteste │
│  0 km/h          │  │ Entfernung: 500m │
│                  │  │                  │
│  RPM: 0          │  │ [Kompass-Pfeil]  │
└──────────────────┘  └──────────────────┘

┌──────────────────┐
│   FUEL           │
│ [========] 80%   │
└──────────────────┘
```

---

## 🗺️ Navigation System (NavigationSystem.cs)

### Features:
- **Route-Verwaltung** - Lädt Routen aus RealisticRouteManager
- **Haltestellen-Verfolgung** - Nächste Haltestelle automatisch aktualisieren
- **Entfernung berechnen** - Distanz zur nächsten Haltestelle
- **Richtung berechnen** - Winkel zum Ziel (für Kompass)
- **Route-Fortschritt** - Prozentuale Completion
- **Automatische Updates** - Wenn Haltestelle erreicht

### Verwendung:
```csharp
var nav = GetComponent<NavigationSystem>();
nav.StartRoute("R_753"); // Route 753 starten

// In Update:
var nextStop = nav.GetNextStop();
var distance = nav.GetDistanceToNextStop();
var direction = nav.GetDirectionToNextStop();

// Wenn Haltestelle erreicht:
nav.ReachedStop(); // Zum nächsten Stopp
```

---

## 🚗 Straßen-Physik (AdvancedBusPhysics.cs)

### Realistisches Lenkverhalten:
- **Ackermann-Geometrie** - Realistische Lenkung mit Radstand
- **Straßen-Erkennung** - Bus verhält sich anders auf/off-road
- **Unterschiedliche Reibung**:
  - Auf Straße: 0.5 (niedrig) = smoothe Fahrt
  - Off-Road: 5.0 (hoch) = schwere Fahrt
- **Geschwindigkeit limitiert** - Off-road automatisch langsamer
- **Kurven-Neigung** - Bus neigt sich in Kurven

### Physik-Parameter:
```
Max Speed: 80 km/h
Acceleration: 15 m/s²
Brake Power: 20 m/s²
Wheelbase: 8m (Achsabstand)
Max Steering: 30°
Max Tilt: 5°
```

---

## 🛣️ Straßen-System (RoadBuilder.cs & AutoRoadSystem.cs)

### Features:
- **Automatische Straßen-Generierung** - Aus OSM-Daten
- **Mesh-basierte Straßen** - Geometrie-Erstellung
- **Kollisions-Erkennung** - Physics.Raycast für On-Road-Check
- **Breiten-Konfiguration** - Anpassbare Straßenbreite (default 6m)
- **Layer-System** - "Road" Layer für Physik

### Straßen-Struktur:
```
RoadSegment:
- name: Straßenname
- waypoints[]: Punkte entlang der Straße
- width: Straßenbreite (m)
- isTwoWay: Bidirektional?
```

---

## 🎮 Integration mit AutoSetup

Alle Systeme werden automatisch initialisiert:
1. HUDSystem erstellt Canvas & UI-Elemente
2. NavigationSystem mit RealisticRouteManager verbunden
3. AdvancedBusPhysics ersetzt BusPhysicsController
4. RoadBuilder generiert Straßen aus OSM

---

## 📋 Steuerung mit neuen Systemen

| Taste | Funktion |
|-------|----------|
| **W/S** | Gas / Rückwärts |
| **A/D** | Lenken (mit Ackermann) |
| **Space** | Bremse |
| **E** | Motor Start/Stop |
| **C** | Kamera-Modus |

---

## 🔮 Nächste Schritte

- [ ] Fahrgast-System mit Ein-/Aussteigen
- [ ] Sounds (Motor, Hupe, Ansagen)
- [ ] Tank-System mit echtem Verbrauch
- [ ] Fahrplan-Synchronisation
- [ ] Traffic & andere Busse
