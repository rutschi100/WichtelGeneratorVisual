using System;
using System.IO;
using System.Text.Json;

namespace WichtelGenerator.Core.Configuration
{
    internal class ConfigManager : IConfigManager
    {
        public ConfigManager()
        {
            if (!ConfigExists())
            {
                Write(new ConfigModel());
            }
        }

        private string AppDataFile { get; } = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) +
                                              @"\WichtelGenerator.json";

        public ConfigModel Read()
        {
            var jsonString = File.ReadAllText(AppDataFile);
            return JsonSerializer.Deserialize<ConfigModel>(jsonString);
        }

        public void Write(ConfigModel configModel)
        {
            if (ConfigExists()) return;
            var jsonString = JsonSerializer.Serialize(configModel);
            File.Create(AppDataFile);
            var writer = new StreamWriter(AppDataFile);
            writer.Write(jsonString);
            writer.Close();
        }

        private bool ConfigExists()
        {
            return File.Exists(AppDataFile);
        }
    }
}