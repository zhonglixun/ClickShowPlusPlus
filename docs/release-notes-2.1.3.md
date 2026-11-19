# ClickShow++ 2.1.3

修复应用内更新：下载完成后误报「下载失败」，并让进度条真正显示进度。

## 本次更新

- 修复应用内更新下载到最后报「下载失败」的问题：校验前文件句柄没有关闭，读文件时抛异常；现在写完先关闭再校验；
- 更新进度条现在真实显示进度；百分比与已下载/总大小单独一行，不再与提示文字挤在一起；
- 修复更新检查回退路径（GitHub API）里的版本号解析正则，之前标签解析失败会直接报检查失败。

## 下载

| 文件 | 大小 | 运行环境 |
|---|---|---|
| `ClickShow-Setup-2.1.3.exe` | 约 63 MB | 自带 .NET 运行时 |
| `ClickShowPlusPlus-Portable-2.1.3.zip` | 约 63 MB | 解压即用 |
| `ClickShow-Setup-2.1.3-lite.exe` | 约 2 MB | 需要 .NET 8 桌面运行时（x64） |
| `ClickShowPlusPlus-Portable-2.1.3-lite.zip` | 约 0.16 MB | 同上 |

## 校验值（SHA256）

```
ClickShow-Setup-2.1.3.exe
A9BB8D9D59FDBE0D968C64F6904FF789EF2858DF1E4E4613A68751AF80B27B95

ClickShowPlusPlus-Portable-2.1.3.zip
0084BE22D34F381AD45B2E9C7D6738BBB32E199CD7E2A7FF5F5A5675311FF778

ClickShow-Setup-2.1.3-lite.exe
28C7726165BF254296B39495A708DAAE1C127F5D8F112FE25B9B3488BB0EB185

ClickShowPlusPlus-Portable-2.1.3-lite.zip
0F33D54802A5608257CAC3054C7EE3CA2820790BFDC3A54F6C373C1B63F59F86
```

---

Fixed the in-app updater reporting "download failed" after a complete download, and made the progress bar show real progress with the percentage on its own line.

