# Projekt-Architektur

## Überblick

Das Spiel folgt einer modularen Architektur mit klaren Verantwortlichkeiten:

## Komponenten

### GameManager
- Zentrale Spielverwaltung
- Spielzustände (Playing, Paused, GameOver)
- Globale Event-Verwaltung

### BusController
- Bus-Physik und Fahrdynamik
- Input-Handling
- Geschwindigkeit und Lenkung

### MapManager
- Kartendaten-Verwaltung
- OSM-Daten-Import
- Umgebungs-Generierung

### PassengerManager
- Passagier-Spawning
- Fahrgast-Verwaltung
- AI für Fahrgäste

### HUD
- UI-Anzeigen
- Spielerinformationen
- Statusanzeigen

## Datenfluss

```
Input → BusController → Physics → Passagiere
  ↓
GameManager → State Management
  ↓
MapManager → Umgebung
  ↓
HUD → Anzeige
```

## Erweiterbarkeit

- Neue Fahrzeuge: Erben von `BusController`
- Neue AI-Typen: Implementieren `IPassengerBehavior`
- Custom Maps: Über `MapManager.LoadOSMData()`
