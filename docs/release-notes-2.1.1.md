# ClickShow++ 2.1.1

修复更新检查受 GitHub API 限额影响的问题。

## 本次更新

- 更新检查改为读取仓库内的静态清单 `docs/latest.json`（raw CDN），不再消耗 GitHub API 的 60 次/小时匿名配额；
- 清单不可用时自动回退到 GitHub API，保证兼容；
- 应用内下载、SHA256 校验、自动启动安装程序的流程保持不变。

## 下载

| 文件 | 大小 | 运行环境 |
|---|---|---|
| `ClickShow-Setup-2.1.1.exe` | 约 63 MB | 自带 .NET 运行时 |
| `ClickShowPlusPlus-Portable-2.1.1.zip` | 约 63 MB | 解压即用 |
| `ClickShow-Setup-2.1.1-lite.exe` | 约 2 MB | 需要 .NET 8 桌面运行时（x64） |
| `ClickShowPlusPlus-Portable-2.1.1-lite.zip` | 约 0.2 MB | 同上 |

## 校验值（SHA256）

```
ClickShow-Setup-2.1.1.exe
A71DCE97E15EC2B81289E0FBD6B773BE3F23D8F8FCE451039F1A003FF4BD228C

ClickShowPlusPlus-Portable-2.1.1.zip
6FB70E7B73056AF8CF2AAC661A51F0D51DE7E8395E87240BF96331B5D7969B31

ClickShow-Setup-2.1.1-lite.exe
081E45D97245EB9FE8941C6864E163B33496E9C1D4FE3687B4052FD8F00918B4

ClickShowPlusPlus-Portable-2.1.1-lite.zip
3A38E95FC7233CD8CF3323708DBF0ADF519329D4A100DA4704A81479C595CE75
```

---

Update checks now use a static manifest on the raw CDN instead of the rate-limited GitHub API, with automatic fallback to the API.
