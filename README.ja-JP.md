[简体中文](README.md) | [English](README.en-US.md) | **日本語** | [한국어](README.ko-KR.md) | [Español](README.es-ES.md) | [Français](README.fr-FR.md) | [Deutsch](README.de-DE.md) | [Português (Brasil)](README.pt-BR.md) | [Русский](README.ru-RU.md) | [Italiano](README.it-IT.md)

# ClickShow 2.0

Windows 用マウスクリック波紋ツール。左・中・右・X1・X2 の各クリックごとに、カーソル位置へ色の違う波紋を表示します。

![Ripple demo](https://user-images.githubusercontent.com/1972649/122925974-f17ead00-d399-11eb-9c57-9b2f14dd5973.gif)

![設定](docs/screenshot-settings.png)

> [cuiliang/ClickShow](https://github.com/cuiliang/clickshow)（1.4.1 ベース）のモダンリライト版。原作者に感謝。UI と機能を全面刷新、Windows 10/11 専用。

## 機能

### クリック波紋
- 5 つのボタン（左/中/右/X1/X2）ごとに色と有効/無効を個別設定、カラーピッカーと 16 進入力に対応；
- **8 種類のカラープリセット**：クラシック / ネオンナイト / キャンディ / オーシャン / サンセット / フォレスト / ベリー / マカロン をワンクリック適用；

![テーマプリセット](docs/theme-presets.png)

- **アニメーション時間** 200〜600ms で調整；
- **5 種類の拡散スタイル**：等速 / 減速（ナチュラル）/ 強い減速（波）/ オーバーシュート / 弾力；
- 明暗 2 背景のプレビューで、その場で効果を確認。

### ライト / ダークプロファイル
ライトとダークの外観を個別保存（サイズ・色・時間・スタイル）。システムテーマに自動追従、どちらかに固定も可能。

### マウスドット
カーソルに追従する円形インジケータ。サイズと色を調整でき、個別に ON/OFF。

### サイレント
- **全画面自動非表示**：前面ウィンドウが画面いっぱいの時は非表示。ゲームや動画を邪魔しません；
- **指定プログラムでサイレント**：リストに追加したプログラムが前面の間は常に非表示（入力 / 参照 / 前面取得）。

### 表示言語
簡体中文 / English / 日本語 / 한국어 / Español / Français / Deutsch / Português (Brasil) / Русский / Italiano。初回起動時にシステム言語に従い、設定で変更できます。

### トレイ
- トレイアイコンを左クリックで設定を開く；
- 右クリックメニュー：設定を開く / 波紋を一時停止 / 終了；
- 一時停止は即時反映、解除しても設定は変わりません。

## 動作環境

Windows 10 1809+ または Windows 11（x64）。ランタイム同梱のため **.NET のインストールは不要**。

## インストール

1. 本リポジトリの Releases ページから ClickShow-Setup-2.0.0.exe をダウンロード；
2. ウィザードに従ってインストール：カスタムパス対応（ドライブ直選択時は自動でフォルダーを追加）。デスクトップ / スタートメニューのショートカットは選択可；
3. 旧バージョンが起動中の場合、終了を促すダイアログが表示されます。

ポータブル版：ClickShow-2.0.0-portable.zip を解凍してすぐ使用可能。インストール不要。設定は %APPDATA%/ClickShow に保存。

## 1.x からのアップグレード

- 初回起動時に旧設定（`ドキュメント/ClickShow.setting`）を `%APPDATA%/ClickShow/setting.json` へ自動移行。色や設定は保持されます；
- ライト / ダークは同じ値で開始；
- 上書きインストールでも設定とスタートアップ登録は保持。

## ビルド

```
dotnet publish ClickShow/ClickShow.csproj -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true -o publish
```

インストーラー：[Inno Setup 6](https://jrsoftware.org/isinfo.php) で `installer/ClickShow.iss` をコンパイル。

## FAQ

- **全画面で波紋が出ない？** 「全画面で自動非表示」が機能しています。設定でオフにできます；
- **トレイの「波紋を一時停止」がグレーアウト？** マウスフックのインストールに失敗しています（まれ）。再起動してください；
- **アンインストール後にスタートアップが残る？** アンインストーラーがレジストリを自動削除します。

## 更新履歴

### 2.0.0（2026-10）
- UI 全面リライト：シングルウィンドウ設定、システム追従のライト/ダーク、10 言語 UI；
- 波紋描画を刷新：スケール変換アニメーション（高リフレッシュレートで満フレーム）、ベクターアンチエイリアス、時間と拡散スタイルを調整可能；
- カラープリセット 8 種、ライト/ダーク個別プロファイル；
- 全画面自動非表示とプログラム別サイレント；
- カラーピッカー + 16 進入力、トレイからの一時停止、設定のインポート/エクスポート；
- .NET Framework 4.7.2 → .NET 8（単一ファイル同梱）、Inno Setup インストーラー。

### 1.4.1 以前
[原文リリースページ](https://github.com/cuiliang/clickshow/releases) を参照。

## ライセンス

原プロジェクトのライセンスに準拠（LICENSE を参照）。オリジナルの [cuiliang](https://github.com/cuiliang) に感謝します。
