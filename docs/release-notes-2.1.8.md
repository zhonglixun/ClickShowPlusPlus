# ClickShow++ 2.1.8

圆角窗口、细描边、自定义预设实时配色，更新按钮在有待更新时高亮。

## 本次更新

- 窗口外轮廓改为系统圆角（Win11 DWM），与界面内部卡片圆角统一；四周加 1px 极细描边，避免与相近背景融在一起；
- 「自定义」预设卡片实时显示当前五键颜色（之前一直显示灰占位）；
- 有可用更新时，更新按钮切换为强调色（主按钮样式）；已是最新版本时恢复为普通样式。
- 检查更新时按钮旁显示旋转加载圈，检查期间按钮禁用，避免误以为没反应；
- 更新按钮按文字自适应宽度，去掉多余留白；
- 去掉按钮、导航、开关、输入框等控件上的虚线焦点框；
- 修复第二个实例退出时释放互斥量抛异常导致的崩溃。

## 下载

| 文件 | 大小 | 运行环境 |
|---|---|---|
| `ClickShow-Setup-2.1.8.exe` | 约 63 MB | 自带 .NET 运行时 |
| `ClickShowPlusPlus-Portable-2.1.8.zip` | 约 63 MB | 解压即用 |
| `ClickShow-Setup-2.1.8-lite.exe` | 约 2 MB | 需要 .NET 8 桌面运行时（x64） |
| `ClickShowPlusPlus-Portable-2.1.8-lite.zip` | 约 0.16 MB | 同上 |

## 校验值（SHA256）

```
ClickShow-Setup-2.1.8.exe
0169A6CC3FB490C4562B072F38D1A620E3725ED630FFE6AD282F95C3D145DF3F

ClickShowPlusPlus-Portable-2.1.8.zip
DD95E23781D93A6F85B310ADC9436514CF3A4FC5E839577DD47D269B90F24075

ClickShow-Setup-2.1.8-lite.exe
76213265DE7CBA7F2FF5376D392809C580AB1AF2774459F561B3F8AADD3FA8FA

ClickShowPlusPlus-Portable-2.1.8-lite.zip
F14EDD2E1961413C313FE3CA5D5FD6FA992FADA85FADEFDFBBA246BBFD853C57
```

---

Rounded window corners with a hairline border, live colours on the custom preset card, and the update button highlights when a new build is available.

