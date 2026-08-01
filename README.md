# Bus Simulator

Ein sauberer Neustart für eine realistische Bus-Simulation im Stil klassischer Omnibus-Simulatoren. Das Projekt wird **schrittweise** entwickelt; der erste Meilenstein ist bewusst klein, nachvollziehbar und auf einem MacBook ausführbar.

## Meilenstein 1: Fahrbarer Prototyp

Der aktuelle Stand enthält:

- ein minimales Unity-6-Projekt ohne fremde Assets,
- eine automatisch erzeugte Teststrecke,
- einen physikbasierten Platzhalter-Bus,
- eine nachlaufende Kamera,
- ein direkt bedienbares Haupt- und Pausenmenü,
- eine Anzeige für Geschwindigkeit und Steuerung.

## Auf dem MacBook starten

1. [Unity Hub](https://unity.com/download) installieren.
2. Über **Installs → Install Editor** die in `ProjectSettings/ProjectVersion.txt` angegebene Unity-Version installieren. Das Modul **Mac Build Support** genügt für diesen Prototyp.
3. In Unity Hub **Add → Add project from disk** wählen und diesen Ordner öffnen.
4. `Assets/BusSimulator/Scenes/Start.unity` öffnen.
5. Oben auf **Play** klicken.
6. Im Hauptmenü **Fahrt starten** auswählen.

### Steuerung

| Aktion | Tasten |
| --- | --- |
| Beschleunigen / Bremsen | `W` / `S` oder Pfeiltasten |
| Lenken | `A` / `D` oder Pfeiltasten |
| Handbremse | Leertaste |
| Zurücksetzen | `R` |
| Pausenmenü | `Esc` |

> Auf macOS fragt Unity beim ersten Start eventuell nach Zugriffsrechten für den Projektordner. Diese müssen erlaubt werden, damit Unity die `Library` erzeugen kann.

## Projektstruktur

```text
Assets/BusSimulator/Runtime/  Spiellogik
Assets/BusSimulator/Scenes/   Startszene
Packages/                     reproduzierbare Unity-Pakete
ProjectSettings/              Editor- und Projekteinstellungen
```

## Nächste Schritte

1. Radaufhängung und Antriebsstrang mit `WheelCollider` umsetzen.
2. Ein erstes modulares Busmodell mit Fahrerplatz, Türen und Instrumenten erstellen.
3. Haltestellen, Fahrplan und Linienlogik ergänzen.
4. Danach Fahrgäste, KI-Verkehr, Wetter und Kartenwerkzeuge aufbauen.

Marken, Namen und Inhalte anderer Spiele werden nicht übernommen; dieses Repository entwickelt eine eigenständige Simulation.
