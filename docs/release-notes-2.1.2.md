# ClickShow++ 2.1.2

修复两个影响日常使用的问题，并调整侧栏图标。

## 本次更新

- 鼠标浮标跟手修复：落位改为合并派发，同一时刻只排一个任务且始终使用最新坐标，快速移动时不再远远拖在后面；
- 取色器崩溃修复：打开取色器会触发空引用并退出程序，现已修复；十六进制输入框文字不再被裁切；
- 侧栏图标调整：外观（调色板）、静默（隐藏）、显示波纹（波纹）、鼠标浮标（圆点）改用统一简洁的图标，浮标图标实时显示当前设置的浮标颜色；
- README 各语言更新到 2.1.2，界面截图换为无边框、无黑边的客户区截图。

## 下载

| 文件 | 大小 | 运行环境 |
|---|---|---|
| `ClickShow-Setup-2.1.2.exe` | 约 63 MB | 自带 .NET 运行时 |
| `ClickShowPlusPlus-Portable-2.1.2.zip` | 约 63 MB | 解压即用 |
| `ClickShow-Setup-2.1.2-lite.exe` | 约 2 MB | 需要 .NET 8 桌面运行时（x64） |
| `ClickShowPlusPlus-Portable-2.1.2-lite.zip` | 约 0.15 MB | 同上 |

## 校验值（SHA256）

```
ClickShow-Setup-2.1.2.exe
10A85844B12BA77270AD666E025A77BA6668897E41AA724171D808705F860D84

ClickShowPlusPlus-Portable-2.1.2.zip
84EB8B6992B8CB65D5A11AA68349D3953EEA8FA16E14410B833584C31FB1515F

ClickShow-Setup-2.1.2-lite.exe
EF743F3AC7288D868C29E80126F7068EAD38DB541DE951D9435CFE27EA4CA758

ClickShowPlusPlus-Portable-2.1.2-lite.zip
232990FEC96D6269B71CE665A34507938228E1C8B2D38E3EB66A2FFC5AA67EE7
```

---

The mouse dot now tracks fast cursor movement without lag, the color picker no longer crashes on open, and the sidebar icons were refreshed.

