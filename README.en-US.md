[简体中文](README.md) | **English** | [日本語](README.ja-JP.md) | [한국어](README.ko-KR.md) | [Español](README.es-ES.md) | [Français](README.fr-FR.md) | [Deutsch](README.de-DE.md) | [Português (Brasil)](README.pt-BR.md) | [Русский](README.ru-RU.md) | [Italiano](README.it-IT.md)

# ClickShow++ 2.0

Mouse-click ripple beautifier for Windows. Every left, middle, right, X1 and X2 click spreads a colored ripple right at the cursor.

![Ripple demo](https://user-images.githubusercontent.com/1972649/122925974-f17ead00-d399-11eb-9c57-9b2f14dd5973.gif)

![Settings](docs/screenshot-settings.png)

> A modernized rewrite of [cuiliang/ClickShow](https://github.com/cuiliang/clickshow) (based on 1.4.1). Many thanks to the original author. UI and features fully rewritten; Windows 10/11 only.

## Features

### Click ripple
- Independent color and on/off toggle for all five buttons (left/middle/right/X1/X2), with a color picker and hex input;
- **8 color preset themes**: Classic / Neon night / Candy / Ocean / Sunset / Forest / Berry / Macaron — one-click apply;

![Theme presets](docs/theme-presets.png)

- **Animation duration** adjustable from 200–600 ms;
- **5 spread styles**: linear / decelerate (natural) / strong ease-out (water) / slight overshoot / elastic;
- Dual-background preview: try the effect on light and dark scenes instantly.

### Light & dark profiles
Light and dark looks are saved independently (size, colors, duration, style), switch automatically with the system theme, or lock to one.

### Mouse dot
A circular indicator that follows the cursor, with adjustable size and color and its own toggle.

### Silence
- **Auto-hide in fullscreen**: no effect when the foreground window fills a monitor — games and videos stay clean;
- **Per-program silence**: add programs (type, browse, or capture the foreground) to always hide the effect while they are in the foreground.

### UI language
简体中文 / English / 日本語 / 한국어 / Español / Français / Deutsch / Português (Brasil) / Русский / Italiano. Follows the system language on first run; changeable in Settings.

### Tray
- Left-click the tray icon to open Settings;
- Right-click menu: open settings / pause ripple / exit;
- Pausing takes effect instantly and restores cleanly.

## Requirements

Windows 10 1809+ or Windows 11 (x64). Self-contained runtime — **no .NET install needed**.

## Install

1. Download ClickShow-Setup-2.0.0.exe from the Releases page of this repository;
2. Run the wizard: custom install path supported (choosing a drive root automatically adds a folder); desktop and Start Menu shortcuts are optional;
3. If an older version is running, the installer will ask to close it first.

Portable: download ClickShow-2.0.0-portable.zip, unzip and run — no installation needed; settings live in %APPDATA%/ClickShow.

## Upgrading from 1.x

- Your old settings (`Documents/ClickShow.setting`) migrate automatically on first launch to `%APPDATA%/ClickShow/setting.json` — colors and options are preserved;
- Light and dark profiles start as identical copies;
- Overwriting an install keeps all settings and the autostart entry.

## Build

```
dotnet publish ClickShow/ClickShow.csproj -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true -o publish
```

Installer: compile `installer/ClickShow.iss` with [Inno Setup 6](https://jrsoftware.org/isinfo.php).

## FAQ

- **No ripple in fullscreen?** "Auto-hide in fullscreen" is doing its job — turn it off in Settings;
- **"Pause ripple" in the tray is grayed out?** The mouse hook failed to install (rare) — restart the app;
- **Broken autostart entry after uninstalling?** The uninstaller cleans up the autostart registry entry automatically.

## Changelog

### 2.0.0 (2026-10)
- Fully rewritten UI: single-window settings, system-following light/dark themes, 10 UI languages;
- Ripple rendering redone: scale-transform animation (full frame rate on high-refresh displays), vector anti-aliasing, adjustable duration and spread styles;
- 8 color preset themes, independent light & dark profiles;
- Fullscreen auto-hide and per-program silence;
- Color picker + hex input; tray pause; config import/export;
- .NET Framework 4.7.2 → .NET 8 (self-contained single file), Inno Setup installer.

### 1.4.1 and earlier
See the [original project releases](https://github.com/cuiliang/clickshow/releases).

## License

Same license as the original project (see LICENSE). Thanks to [cuiliang](https://github.com/cuiliang) for the original ClickShow.
