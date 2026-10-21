[简体中文](README.md) | [English](README.en-US.md) | [日本語](README.ja-JP.md) | [한국어](README.ko-KR.md) | [Español](README.es-ES.md) | **Français** | [Deutsch](README.de-DE.md) | [Português (Brasil)](README.pt-BR.md) | [Русский](README.ru-RU.md) | [Italiano](README.it-IT.md)

# ClickShow++ 2.0

Embellisseur de clics de souris pour Windows. Chaque clic gauche, milieu, droit, X1 et X2 déploie une onde colorée sous le curseur.

![Démo d'ondes](https://user-images.githubusercontent.com/1972649/122925974-f17ead00-d399-11eb-9c57-9b2f14dd5973.gif)

![Paramètres](docs/screenshot-settings.png)

> Réécriture moderne de [cuiliang/ClickShow](https://github.com/cuiliang/clickshow) (basée sur 1.4.1). Merci à l'auteur original. Interface et fonctions entièrement réécrites ; Windows 10/11 uniquement.

## Fonctions

### Onde de clic
- Couleur et activation indépendantes pour les cinq boutons (gauche/milieu/droit/X1/X2), avec sélecteur de couleur et saisie hexadécimale ;
- **8 thèmes de couleurs prédéfinis** : Classique / Néon nocturne / Bonbon / Océan / Coucher de soleil / Forêt / Baie / Macaron — application en un clic ;

![Thèmes prédéfinis](docs/theme-presets.png)

- **Durée de l'animation** réglable de 200 à 600 ms ;
- **5 styles de diffusion** : linéaire / décélération (naturelle) / fort freinage (eau) / léger rebond / élastique ;
- Aperçu sur double fond : essayez l'effet sur scènes claires et sombres instantanément.

### Profils clair et sombre
Les apparences claire et sombre sont enregistrées séparément (taille, couleurs, durée, style) ; elles suivent automatiquement le thème du système ou se figent sur l'un des deux.

### Point de souris
Indicateur circulaire suivant le curseur, taille et couleur réglables, activation indépendante.

### Silence
- **Masquage automatique en plein écran** : aucun effet quand la fenêtre au premier plan remplit l'écran — jeux et vidéos restent propres ;
- **Silence par programme** : ajoutez des programmes (saisie, parcours ou capture du premier plan) pour masquer l'effet pendant qu'ils sont actifs.

### Langue de l'interface
简体中文 / English / 日本語 / 한국어 / Español / Français / Deutsch / Português (Brasil) / Русский / Italiano. Suit la langue du système au premier lancement ; modifiable dans les paramètres.

### Barre d'état système
- Clic gauche sur l'icône pour ouvrir les paramètres ;
- Menu contextuel : ouvrir les paramètres / mettre l'onde en pause / quitter ;
- La pause s'applique instantanément et se rétablit proprement.

## Configuration requise

Windows 10 1809+ ou Windows 11 (x64). Runtime inclus — **aucune installation de .NET nécessaire**.

## Installation

**Choisir la version** : `ClickShow-Setup-2.0.0.exe` inclut le runtime .NET (~63 Mo) et fonctionne sur tout Windows 10/11 ; `ClickShow-Setup-2.0.0-lite.exe` ne fait que ~2 Mo mais **exige .NET 8 Desktop Runtime (x64)**, sinon l'application ne démarre pas (l'installateur le détecte et propose le téléchargement). En cas de doute, prenez la version complète.

Deux versions portables également : `ClickShowPlusPlus-Portable-2.0.0.zip` (complète, ~63 Mo) et `ClickShowPlusPlus-Portable-2.0.0-lite.zip` (Lite, ~0,6 Mo, même prérequis).

1. Téléchargez ClickShow-Setup-2.0.0.exe depuis la page Releases de ce dépôt ;
2. Exécutez l'assistant : chemin d'installation personnalisé pris en charge (la racine d'un lecteur ajoute automatiquement un dossier) ; raccourcis bureau et menu Démarrer en option ;
3. Si une ancienne version est en cours d'exécution, l'installateur demandera de la fermer.

Version portable : téléchargez ClickShowPlusPlus-Portable-2.0.0.zip, décompressez et lancez — sans installation ; les paramètres vivent dans %APPDATA%/ClickShow.

## Mise à niveau depuis 1.x

- Vos anciens paramètres (`Documents/ClickShow.setting`) migrent automatiquement au premier lancement vers `%APPDATA%/ClickShow/setting.json` — couleurs et options conservées ;
- Les profils clair et sombre commencent comme copies identiques ;
- Réinstaller par-dessus conserve tous les réglages et l'entrée de démarrage.

## Compilation

```
dotnet publish ClickShow/ClickShow.csproj -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true -o publish
```

Programme d'installation : compilez `installer/ClickShow.iss` avec [Inno Setup 6](https://jrsoftware.org/isinfo.php).

## FAQ

- **Pas d'onde en plein écran ?** « Masquage automatique en plein écran » fait son travail — désactivez-le dans les paramètres ;
- **« Mettre l'onde en pause » grisé dans la barre d'état ?** Le hook souris n'a pas pu s'installer (rare) — redémarrez l'application ;
- **Entrée de démarrage cassée après désinstallation ?** Le désinstallateur nettoie automatiquement l'entrée du registre.

## Historique des versions

### 2.0.0 (2026-10)
- Interface entièrement réécrite : paramètres mono-fenêtre, thèmes clair/sombre suivant le système, 10 langues d'interface ;
- Rendu des ondes refait : animation par transformation d'échelle (images pleine fréquence sur écrans à haute fréquence), anticrénelage vectoriel, durée et styles de diffusion réglables ;
- 8 thèmes de couleurs prédéfinis, profils clair et sombre indépendants ;
- Masquage plein écran et silence par programme ;
- Sélecteur de couleur + hexadécimal ; pause depuis la barre d'état ; import/export de configuration ;
- .NET Framework 4.7.2 → .NET 8 (fichier unique autocontenu), programme d'installation Inno Setup.

### 1.4.1 et antérieures
Voir les [versions du projet original](https://github.com/cuiliang/clickshow/releases).

## Licence
Même licence que le projet original (voir LICENSE). Merci à [cuiliang](https://github.com/cuiliang) pour le ClickShow original.
