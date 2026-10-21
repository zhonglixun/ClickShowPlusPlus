[简体中文](README.md) | [English](README.en-US.md) | [日本語](README.ja-JP.md) | [한국어](README.ko-KR.md) | [Español](README.es-ES.md) | [Français](README.fr-FR.md) | [Deutsch](README.de-DE.md) | [Português (Brasil)](README.pt-BR.md) | [Русский](README.ru-RU.md) | **Italiano**

# ClickShow++ 2.0

Abbellitore di clic del mouse per Windows. Ogni clic sinistro, centrale, destro, X1 e X2 diffonde un'onda colorata proprio accanto al cursore.

![Demo onde](https://user-images.githubusercontent.com/1972649/122925974-f17ead00-d399-11eb-9c57-9b2f14dd5973.gif)

![Impostazioni](docs/screenshot-settings.png)

> Riscrittura moderna di [cuiliang/ClickShow](https://github.com/cuiliang/clickshow) (basata su 1.4.1). Grazie all'autore originale. Interfaccia e funzioni completamente riscritte; solo Windows 10/11.

## Funzioni

### Onda del clic
- Colore e attivazione indipendenti per i cinque pulsanti (sinistro/centrale/destro/X1/X2), con selettore colori e input esadecimale;
- **8 temi colore predefiniti**: Classico / Notte al neon / Caramella / Oceano / Tramonto / Foresta / Frutti di bosco / Macaron — applicazione con un clic;

![Temi predefiniti](docs/theme-presets.png)

- **Durata dell'animazione** regolabile da 200 a 600 ms;
- **5 stili di diffusione**: lineare / decelerazione (naturale) / forte decelerazione (acqua) / leggero rimbalzo / elastico;
- Anteprima a doppio sfondo: prova l'effetto su scene chiare e scure all'istante.

### Profili chiaro e scuro
Le apparenze chiara e scura sono salvate separatamente (dimensione, colori, durata, stile); seguono automaticamente il tema di sistema oppure si fissano su uno.

### Punto del mouse
Indicatore circolare che segue il cursore, con dimensione e colore regolabili e attivazione indipendente.

### Silenzia
- **Nascondi a schermo intero**: nessun effetto quando la finestra in primo piano riempie lo schermo — giochi e video restano puliti;
- **Silenzia per programma**: aggiungi programmi (digita, sfoglia o cattura il primo piano) per nascondere sempre l'effetto mentre sono in primo piano.

### Lingua dell'interfaccia
简体中文 / English / 日本語 / 한국어 / Español / Français / Deutsch / Português (Brasil) / Русский / Italiano. Segue la lingua di sistema al primo avvio; modificabile nelle impostazioni.

### Vassoio
- Clic sinistro sull'icona del vassoio per aprire le impostazioni;
- Menu contestuale: apri impostazioni / pausa onda / esci;
- La pausa ha effetto immediato e si ripristina pulita.

## Requisiti di sistema

Windows 10 1809+ o Windows 11 (x64). Runtime incluso — **nessuna installazione di .NET necessaria**.

## Installazione

**Scegli la versione**: `ClickShow-Setup-2.0.0.exe` include il runtime .NET (~63 MB) e funziona su qualsiasi Windows 10/11; `ClickShow-Setup-2.0.0-lite.exe` pesa solo ~2 MB ma **richiede .NET 8 Desktop Runtime (x64)**, altrimenti l'app non si avvia (il programma di installazione lo rileva e propone il download). In dubbio, usa la completa.

Anche i portatili sono due: `ClickShowPlusPlus-Portable-2.0.0.zip` (completo, ~63 MB) e `ClickShowPlusPlus-Portable-2.0.0-lite.zip` (Lite, ~0,6 MB, stesso requisito).

1. Scarica ClickShow-Setup-2.0.0.exe dalla pagina Releases di questo repository;
2. Esegui la procedura guidata: percorso di installazione personalizzato (la radice di un'unità riceve automaticamente una cartella); scorciatoie opzionali su desktop e menu Start;
3. Se una versione precedente è in esecuzione, il programma di installazione chiederà di chiuderla.

Versione portatile: scarica ClickShowPlusPlus-Portable-2.0.0.zip, estrai ed esegui — senza installazione; le impostazioni vivono in %APPDATA%/ClickShow.

## Aggiornamento dalla 1.x

- Le vecchie impostazioni (`Documenti/ClickShow.setting`) migrano automaticamente al primo avvio in `%APPDATA%/ClickShow/setting.json` — colori e opzioni conservati;
- I profili chiaro e scuro iniziano come copie identiche;
- Reinstallare sopra mantiene tutte le impostazioni e la voce di avvio automatico.

## Compilazione

```
dotnet publish ClickShow/ClickShow.csproj -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true -o publish
```

Programma di installazione: compila `installer/ClickShow.iss` con [Inno Setup 6](https://jrsoftware.org/isinfo.php).

## Domande frequenti

- **Nessuna onda a schermo intero?** Il "nascondi a schermo intero" sta facendo il suo lavoro — disattivalo nelle impostazioni;
- **"Pausa onda" nel vassoio è grigia?** L'hook del mouse non è stato installato (raro) — riavvia l'applicazione;
- **Voce di avvio automatico rotta dopo la disinstallazione?** Il programma di disinstallazione pulisce automaticamente la voce del registro.

## Cronologia versioni

### 2.0.0 (2026-10)
- Interfaccia completamente riscritta: impostazioni a finestra singola, temi chiaro/scuro che seguono il sistema, 10 lingue dell'interfaccia;
- Rendering dell'onda rifatto: animazione a trasformazione di scala (frequenza frame completa sui display ad alta frequenza), antialiasing vettoriale, durata e stili di diffusione regolabili;
- 8 temi colore predefiniti, profili chiaro e scuro indipendenti;
- Nascondi a schermo intero e silenzia per programma;
- Selettore colori + input esadecimale; pausa dal vassoio; importazione/esportazione delle impostazioni;
- .NET Framework 4.7.2 → .NET 8 (singolo file autonomo), programma di installazione Inno Setup.

### 1.4.1 e precedenti
Vedi le [versioni del progetto originale](https://github.com/cuiliang/clickshow/releases).

## Licenza
Stessa licenza del progetto originale (vedi LICENSE). Grazie a [cuiliang](https://github.com/cuiliang) per il ClickShow originale.
