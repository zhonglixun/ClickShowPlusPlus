using System;
using System.IO;
using System.Text.Json;
using System.Windows.Forms;
using System.Windows.Media;
using ClickShow.Settings;
using System.Windows;

namespace ClickShow.Utility
{
    /// <summary>
    /// 配置读写：v2 存 %APPDATA%\ClickShow\setting.json；
    /// 首次运行自动从旧版 我的文档\ClickShow.setting 迁移（双套取相同初值）。
    /// </summary>
    public static class SettingsStore
    {
        public static string NewPath
        {
            get
            {
                var dir = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                    "ClickShow");
                return Path.Combine(dir, "setting.json");
            }
        }

        private static string LegacyPath
        {
            get { return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "ClickShow.setting"); }
        }

        public static AppSetting Load(out string notice)
        {
            notice = null;

            if (File.Exists(NewPath))
            {
                try
                {
                    var json = File.ReadAllText(NewPath);
                    var setting = MyJsonConverter.Deserialize<AppSetting>(json);
                    if (!json.Contains("\"RipplePreset\""))
                    {
                        // 旧 v2 配置没有预设字段：颜色是用户自定义的，不能标成预设
                        setting.Light.RipplePreset = "Custom";
                        setting.Dark.RipplePreset = "Custom";
                    }
                    return setting;
                }
                catch (Exception ex)
                {
                    notice = Res("configCorrupt", "Settings file is corrupted; defaults restored. ") + ex.Message;
                    return new AppSetting();
                }
            }

            if (File.Exists(LegacyPath))
            {
                try
                {
                    var appearance = ParseLegacy(File.ReadAllText(LegacyPath));
                    var setting = AppSetting.FromSingleAppearance(appearance);
                    Save(setting);
                    return setting;
                }
                catch (Exception ex)
                {
                    notice = Res("migrateFail", "Failed to migrate legacy settings; defaults are in use. ") + ex.Message;
                    return new AppSetting();
                }
            }

            return new AppSetting();
        }

        /// <summary>
        /// 轻量读取已保存的语言代码（未保存过返回 null，交由 Languages.Resolve 按系统语言决定）。
        /// </summary>
        public static string ReadLanguageCode()
        {
            try
            {
                if (!File.Exists(NewPath))
                {
                    return null;
                }
                using var doc = JsonDocument.Parse(File.ReadAllText(NewPath));
                if (doc.RootElement.TryGetProperty("Language", out var el) && el.ValueKind == JsonValueKind.String)
                {
                    return el.GetString();
                }
            }
            catch
            {
            }
            return null;
        }

        public static void Save(AppSetting setting)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(NewPath));
            File.WriteAllText(NewPath, MyJsonConverter.Serialize(setting));
        }

        /// <summary>
        /// 从文件导入配置：兼容 v2 json 与旧版单套配置。
        /// </summary>
        public static AppSetting ImportFrom(string path)
        {
            var text = File.ReadAllText(path);
            using (var doc = JsonDocument.Parse(text))
            {
                if (doc.RootElement.TryGetProperty("Light", out _))
                {
                    return MyJsonConverter.Deserialize<AppSetting>(text);
                }
            }
            return AppSetting.FromSingleAppearance(ParseLegacy(text));
        }

        /// <summary>
        /// 解析旧版 DataContractJsonSerializer 生成的配置。
        /// 字典序列化为 [{"Key":1048576,"Value":{"Color":"...","IsEnabled":true}}, ...]
        /// </summary>
        private static string Res(string key, string fallback)
        {
            return System.Windows.Application.Current?.TryFindResource(key) as string ?? fallback;
        }

        private static AppearanceSetting ParseLegacy(string json)
        {
            var appearance = new AppearanceSetting();
            // 旧配置颜色是用户自定义的，不能标成预设
            appearance.RipplePreset = "Custom";
            using var doc = JsonDocument.Parse(json);
            var root = doc.RootElement;

            if (root.TryGetProperty("EnableClickCircle", out var v) && v.ValueKind is JsonValueKind.True or JsonValueKind.False)
            {
                appearance.EnableClickCircle = v.GetBoolean();
            }
            if (root.TryGetProperty("EnableHoverDot", out v) && v.ValueKind is JsonValueKind.True or JsonValueKind.False)
            {
                appearance.EnableHoverDot = v.GetBoolean();
            }
            if (root.TryGetProperty("IndicatorSize", out v) && v.ValueKind == JsonValueKind.Number)
            {
                appearance.IndicatorSize = v.GetDouble();
            }
            if (root.TryGetProperty("HoverDotSize", out v) && v.ValueKind == JsonValueKind.Number)
            {
                appearance.HoverDotSize = v.GetDouble();
            }
            if (root.TryGetProperty("HoverDotFill", out v) && v.ValueKind == JsonValueKind.String)
            {
                appearance.HoverDotFill = v.GetString();
            }
            if (root.TryGetProperty("MouseButtonSettings", out v) && v.ValueKind == JsonValueKind.Array)
            {
                foreach (var item in v.EnumerateArray())
                {
                    if (!item.TryGetProperty("Key", out var keyEl))
                    {
                        continue;
                    }

                    MouseButtons button;
                    if (keyEl.ValueKind == JsonValueKind.Number)
                    {
                        button = (MouseButtons)keyEl.GetInt32();
                    }
                    else if (keyEl.ValueKind == JsonValueKind.String && Enum.TryParse(keyEl.GetString(), out MouseButtons named))
                    {
                        button = named;
                    }
                    else
                    {
                        continue;
                    }

                    if (!appearance.MouseButtonSettings.TryGetValue(button, out var bs))
                    {
                        bs = new ButtonSetting();
                        appearance.MouseButtonSettings[button] = bs;
                    }

                    if (item.TryGetProperty("Value", out var valueEl))
                    {
                        if (valueEl.TryGetProperty("Color", out var colorEl) && colorEl.ValueKind == JsonValueKind.String)
                        {
                            bs.Color = colorEl.GetString();
                        }
                        if (valueEl.TryGetProperty("IsEnabled", out var enabledEl) && enabledEl.ValueKind is JsonValueKind.True or JsonValueKind.False)
                        {
                            bs.IsEnabled = enabledEl.GetBoolean();
                        }
                    }
                }
            }

            return appearance;
        }
    }
}
