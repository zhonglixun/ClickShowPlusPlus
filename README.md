# ClickShow 2.0

简体中文 | [English](README.en-US.md) | [日本語](README.ja-JP.md) | [한국어](README.ko-KR.md) | [Español](README.es-ES.md) | [Français](README.fr-FR.md) | [Deutsch](README.de-DE.md) | [Português (Brasil)](README.pt-BR.md) | [Русский](README.ru-RU.md) | [Italiano](README.it-IT.md)

鼠标点击波纹美化工具（Windows）。点击鼠标时在光标处扩散一圈彩色波纹，为每次左键、中键、右键、X1、X2 显示不同颜色的反馈。

> 本项目是 [cuiliang/ClickShow](https://github.com/cuiliang/clickshow) 的现代化重制版（基于 1.4.1），感谢原作者。界面与功能全面重写，仅支持 Windows 10/11。

![波纹效果演示](https://user-images.githubusercontent.com/1972649/122925974-f17ead00-d399-11eb-9c57-9b2f14dd5973.gif)

![设置界面](docs/screenshot-settings.png)

## 功能

### 点击波纹
- 五键（左/中/右/X1/X2）独立颜色与启用开关，支持取色器和十六进制精调；
- **8 套颜色预设主题**：经典默认 / 霓虹夜行 / 糖果 / 海洋 / 夕阳 / 森林 / 莓果 / 马卡龙，一键应用；
- **动画时长** 200–600ms 可调；
- **5 种扩散样式**：匀速 / 减速（自然）/ 强减速（水波）/ 轻微回弹 / 弹性波动；
- 双底色预览区：浅色/深色场景即时试玩效果。

### 明暗双套
浅色 / 深色两套外观独立保存（大小、颜色、时长、样式），跟随系统主题自动切换，也可固定使用其中一套。

### 鼠标浮标
跟随光标的圆形指示点，大小与颜色可调，独立启停。

### 静默
- **全屏自动隐藏**：前台窗口铺满显示器时不显示特效，游戏、看片不干扰；
- **指定程序静默**：把程序加入列表（手动输入 / 浏览选择 / 抓取前台），该程序在前台时始终隐藏特效。

### 界面语言
简体中文 / English / 日本語 / 한국어 / Español / Français / Deutsch / Português (Brasil) / Русский / Italiano。首次启动跟随系统语言，可在设置中更改。

### 托盘
- 左键点击托盘图标打开设置；
- 右键菜单：打开设置 / 暂停波纹 / 退出；
- 暂停状态即时生效，恢复后不影响任何配置。

## 系统要求

Windows 10 1809+ 或 Windows 11（x64）。自包含运行时，**无需安装 .NET**。

## 安装

1. 从本仓库的 Releases 页面下载 ClickShow-Setup-2.0.0.exe；
2. 运行安装向导：支持自定义安装路径（选择磁盘根目录时会自动套一层文件夹），桌面与开始菜单快捷方式可选；
3. 若旧版本正在运行，安装器会提示关闭后继续。

## 从 1.x 升级

- 首次启动自动迁移旧配置（`我的文档\ClickShow.setting` → `%APPDATA%\ClickShow\setting.json`），原颜色与设置保留；
- 明/暗两套外观初始取相同值；
- 覆盖安装保留全部配置与开机自启设置。

## 构建

```
dotnet publish ClickShow\ClickShow.csproj -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true -o publish
```

安装包：用 [Inno Setup 6](https://jrsoftware.org/isinfo.php) 编译 `installer/ClickShow.iss`。

## 常见问题

- **全屏下看不到波纹？** 这是"全屏自动隐藏"在起作用，可在设置中关闭，或把该程序加入静默列表的反向操作；
- **托盘"暂停波纹"置灰？** 鼠标钩子安装失败（极少见），重启程序即可；
- **卸载后自启动失效项？** 卸载器会自动清理开机自启注册表项。

## 更新历史

### 2.0.0（2026-10）
- 界面全面重写：单窗口设置、明暗主题跟随系统、10 种界面语言；
- 波纹渲染重做：缩放变换动画（高刷屏满帧）、矢量抗锯齿、时长与扩散样式可调；
- 主题预设 8 套、明暗双套配置；
- 全屏自动隐藏与指定程序静默；
- 取色器 + 十六进制输入；托盘暂停波纹；配置导入导出；
- .NET Framework 4.7.2 → .NET 8（自包含单文件），Inno Setup 中文安装包。

### 1.4.1 及更早
见[原项目发布页](https://github.com/cuiliang/clickshow/releases)。

## 许可

沿用原项目许可（见 LICENSE）。感谢 [cuiliang](https://github.com/cuiliang) 的原作。
