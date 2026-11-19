# ClickShow++ 2.1.0

界面与更新流程的完整重构版本。

## 下载

| 文件 | 大小 | 运行环境 |
|---|---|---|
| `ClickShow-Setup-2.1.0.exe` | 约 63 MB | 自带 .NET 运行时，装上就能用 |
| `ClickShowPlusPlus-Portable-2.1.0.zip` | 约 63 MB | 同上，解压即用 |
| `ClickShow-Setup-2.1.0-lite.exe` | 约 2 MB | **需要 .NET 8 桌面运行时（x64）** |
| `ClickShowPlusPlus-Portable-2.1.0-lite.zip` | 约 0.2 MB | **需要 .NET 8 桌面运行时（x64）** |

## 本次更新

- **设置界面完全重做**：左侧三页导航（外观 / 静默 / 通用），卡片式分组，替代原来的长列表；
- **跟随 Windows 强调色**：读取系统强调色作为主色，深色主题下自动使用亮色变体；
- **侧栏快捷开关**：暂停波纹、鼠标浮标，随时一键开关；
- **控件现代化**：自绘下拉框、细圆角滚动条、分段选择器、滑块、开关、输入框，去掉系统旧样式；
- **更新流程一条龙**：检查更新 → 显示新版本 → 应用内下载（百分比与 MB 进度）→ 自动启动安装程序；
- 版本号 2.1.0，配置与静默列表自动保留。

## 系统要求

Windows 10 1809+ 或 Windows 11（x64）。

## 从 2.0 升级

直接运行新版安装包覆盖安装即可，配置位于 `%APPDATA%/ClickShow/setting.json`，不会被覆盖。

## 校验值（SHA256）

```
+ClickShow-Setup-2.1.0.exe
71408672AF8FC7FE8AE9BE2E2CD2706D0224EBE98A0ECF86A6FB9F6492D457D3

ClickShowPlusPlus-Portable-2.1.0.zip
93379095871230DC5D1653C25AB8721F984C2F8BF635CBB964288040EE7514F8

ClickShow-Setup-2.1.0-lite.exe
21091D593B830EB29B1105EB4BE4C0DFD6412EB7C27F88CCB726CF238F157C18

ClickShowPlusPlus-Portable-2.1.0-lite.zip
947371E48174F0CA84FF5B26862649EF51956A419AC56DBE7059243E27A4D2DE
```

---

Full UI redesign: three-page sidebar navigation, card layout, system accent color, custom-styled controls, quick toggles for ripples and mouse dot, and an in-app update downloader with progress that launches the installer.
