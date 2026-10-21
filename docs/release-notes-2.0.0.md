# ClickShow++ 2.0.0

系统美化小玩具：每次点击鼠标，在光标处扩散一圈彩色波纹，让每一次点击都有存在感。

## 下载

**先选版本**：不确定就选前两个完整版。

| 文件 | 大小 | 运行环境 |
|---|---|---|
| `ClickShow-Setup-2.0.0.exe` | 约 63 MB | 自带 .NET 运行时，装上就能用 |
| `ClickShowPlusPlus-Portable-2.0.0.zip` | 约 63 MB | 同上，解压即用 |
| `ClickShow-Setup-2.0.0-lite.exe` | 约 2 MB | **需要 .NET 8 桌面运行时（x64）** |
| `ClickShowPlusPlus-Portable-2.0.0-lite.zip` | 约 0.2 MB | **需要 .NET 8 桌面运行时（x64）** |

lite 版不打包运行时；缺少桌面运行时打不开，安装器会先检测并给出下载地址。装过 .NET SDK 的机器通常已包含。

## 功能

- 五键独立颜色与启用开关（左/中/右/X1/X2），支持取色器与十六进制精调
- 8 套主题预设：经典 / 霓虹夜行 / 糖果 / 海洋 / 夕阳 / 森林 / 莓果 / 马卡龙
- 动画时长 200–600 ms、5 种扩散样式（匀速 / 减速 / 强减速 / 回弹 / 弹性）
- 明暗双套外观，跟随系统主题自动切换
- 鼠标浮标（大小、颜色可调）
- 静默：全屏自动隐藏、指定程序前台静默（手动输入 / 浏览 / 抓取前台）
- 10 种界面语言，首次启动跟随系统语言
- 托盘菜单：打开设置 / 暂停波纹 / 退出

## 系统要求

Windows 10 1809+ 或 Windows 11（x64），自包含运行时，无需安装 .NET。

## 从 1.x 升级

首次启动自动迁移 `我的文档/ClickShow.setting` 到 `%APPDATA%/ClickShow/setting.json`，原颜色与设置保留。

## 文件校验（SHA256）

```
ClickShow-Setup-2.0.0.exe
547DA20BDAEF416F0ABBB4A8808BA7E9B6081D06C3F047040844C878CEA9F739

ClickShowPlusPlus-Portable-2.0.0.zip
65E5CA89119DD19D50EDE0D69A1666CC955936779D7BD9B5EAFA62FFEF46016C

ClickShow-Setup-2.0.0-lite.exe
4C0DD95578595EB81D021117D0BFFB3EA3784EF99A32DFE3582DE983E8CE8E1F

ClickShowPlusPlus-Portable-2.0.0-lite.zip
92D9E6C88FF42FCF9840A87C95D0678533BD55109F719514596318FD744425D0
```

---

Windows-only click ripple toy: a colored ripple spreads at the cursor on every click. Independent colors for five buttons, 8 color presets, adjustable duration and 5 easing styles, light/dark profiles that follow the system theme, fullscreen and per-program silence, 10 UI languages. Requires Windows 10 1809+ / Windows 11 (x64).

Two builds: the full packages (~63 MB) bundle the .NET runtime and work anywhere; the `-lite` packages (~2 MB / 0.2 MB) require the .NET 8 Desktop Runtime (x64) and will not start without it — the installer checks and offers the download.
