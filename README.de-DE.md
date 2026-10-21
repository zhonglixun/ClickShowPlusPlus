[简体中文](README.md) | [English](README.en-US.md) | [日本語](README.ja-JP.md) | [한국어](README.ko-KR.md) | [Español](README.es-ES.md) | [Français](README.fr-FR.md) | **Deutsch** | [Português (Brasil)](README.pt-BR.md) | [Русский](README.ru-RU.md) | [Italiano](README.it-IT.md)

# ClickShow++ 2.0

Mausklick-Wellen-Tool für Windows. Jeder Links-, Mittel-, Rechts-, X1- und X2-Klick entfaltet eine farbige Welle direkt am Cursor.

![Wellen-Demo](https://user-images.githubusercontent.com/1972649/122925974-f17ead00-d399-11eb-9c57-9b2f14dd5973.gif)

![Einstellungen](docs/screenshot-settings.png)

> Moderne Neufassung von [cuiliang/ClickShow](https://github.com/cuiliang/clickshow) (basierend auf 1.4.1). Dank an den Originalautor. Oberfläche und Funktionen vollständig neu geschrieben; nur Windows 10/11.

## Funktionen

### Klick-Welle
- Farbe und Ein/Aus unabhängig für alle fünf Tasten (Links/Mitte/Rechts/X1/X2), mit Farbwähler und Hex-Eingabe;
- **8 Farbthemen**: Klassik / Neon-Nacht / Bonbon / Ozean / Sonnenuntergang / Wald / Beere / Macaron — mit einem Klick;

![Farbthemen](docs/theme-presets.png)

- **Wellengröße** einstellbar über 200–600 ms;
- **5 Ausbreitungsstile**: linear / abbremsen (natürlich) / stark abbremsen (Wasser) / leicht überschwingen / elastisch;
- Vorschau auf hellem und dunklem Hintergrund — Effekt sofort testbar.

### Helle und dunkle Profile
Helles und dunkles Aussehen werden getrennt gespeichert (Größe, Farben, Dauer, Stil); folgen automatisch dem Systemthema oder lassen sich festlegen.

### Mauspunkt
Kreisförmiger Indikator, der dem Cursor folgt; Größe und Farbe einstellbar, eigene Ein/Aus-Schaltung.

### Stumm
- **Automatisch im Vollbild ausblenden**: kein Effekt, wenn das Vordergrundfenster den Bildschirm füllt — Spiele und Videos bleiben sauber;
- **Stumm pro Programm**: Programme der Liste (tippen, auswählen oder Vordergrund erfassen) blenden den Effekt immer aus.

### Oberflächensprache
简体中文 / English / 日本語 / 한국어 / Español / Français / Deutsch / Português (Brasil) / Русский / Italiano. Folgt beim ersten Start der Systemsprache; in den Einstellungen änderbar.

### Taskleiste
- Linksklick auf das Symbol öffnet die Einstellungen;
- Kontextmenü: Einstellungen öffnen / Welle pausieren / Beenden;
- Die Pause wirkt sofort und stellt sich sauber wieder her.

## Systemanforderungen

Windows 10 1809+ oder Windows 11 (x64). Laufzeit integriert — **keine .NET-Installation nötig**.

## Installation

1. ClickShow-Setup-2.0.0.exe von der Releases-Seite dieses Repositories herunterladen;
2. Assistent ausführen: benutzerdefinierter Installationspfad unterstützt (Wahl eines Laufwerksstammverzeichnisses ergänzt automatisch einen Ordner); Desktop- und Startmenü-Verknüpfungen optional;
3. Läuft eine ältere Version, bittet das Setup sie zu schließen.

Portable Version: ClickShow-2.0.0-portable.zip herunterladen, entpacken und starten — keine Installation; Einstellungen liegen in %APPDATA%/ClickShow.

## Upgrade von 1.x

- Alte Einstellungen (`Dokumente/ClickShow.setting`) werden beim ersten Start automatisch nach `%APPDATA%/ClickShow/setting.json` migriert — Farben und Optionen bleiben erhalten;
- Helle und dunkle Profile starten mit gleichen Werten;
- Überschreibende Installation behält alle Einstellungen und den Autostart-Eintrag.

## Build

```
dotnet publish ClickShow/ClickShow.csproj -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true -o publish
```

Installer: `installer/ClickShow.iss` mit [Inno Setup 6](https://jrsoftware.org/isinfo.php) kompilieren.

## FAQ

- **Keine Welle im Vollbild?** „Automatisch im Vollbild ausblenden" arbeitet — in den Einstellungen deaktivieren;
- **„Welle pausieren" in der Taskleiste ist grau?** Der Maus-Hook konnte nicht installiert werden (selten) — App neu starten;
- **Autostart-Eintrag nach der Deinstallation defekt?** Der Deinstaller räumt den Registrierungseintrag automatisch auf.

## Versionshistorie

### 2.0.0 (2026-10)
- Oberfläche neu geschrieben: Einzelfenster-Einstellungen, helles/dunkles Thema folgt dem System, 10 Oberflächensprachen;
- Wellen-Rendering neu: Scale-Transform-Animation (volle Bildrate auf High-Refresh-Displays), Vektor-Antialiasing, Dauer und Ausbreitungsstil einstellbar;
- 8 Farbthemen, getrennte helle/dunkle Profile;
- Vollbild-Ausblendung und Stumm pro Programm;
- Farbwähler + Hex-Eingabe; Pause über die Taskleiste; Konfigurationsimport/-export;
- .NET Framework 4.7.2 → .NET 8 (einzeldatei, selbstständig), Inno-Setup-Installer.

### 1.4.1 und älter
Siehe die [Releases des Originalprojekts](https://github.com/cuiliang/clickshow/releases).

## Lizenz
Wie das Originalprojekt (siehe LICENSE). Dank an [cuiliang](https://github.com/cuiliang) für das ursprüngliche ClickShow.
