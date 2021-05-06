using System;
using System.IO;
using System.Text.Json;

namespace WichtelGenerator.Core.Configuration
{
    internal class ConfigManager : IConfigManager
    {
        public ConfigManager()
        {
            ConfigModel = new ConfigModel();
            if (!ConfigExists())
            {
                Write(ConfigModel);
            }
        }

        private string AppDataFile { get; } = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) +
                                              @"\WichtelGenerator.json";

        public ConfigModel ConfigModel { get; set; }

        public ConfigModel Read()
        {
            if (!ConfigExists())
            {
                return new ConfigModel();
            }
            var jsonString = File.ReadAllText(AppDataFile);

            if (string.IsNullOrEmpty(jsonString))
            {
                return new ConfigModel();
            }

            ConfigModel = JsonSerializer.Deserialize<ConfigModel>(jsonString);
            return ConfigModel;
        }

        public void Write(ConfigModel configModel)
        {
            RemoveConfigFromFileSystem();

            var jsonString = JsonSerializer.Serialize(configModel);
            var result = File.Create(AppDataFile);
            result.Close();

            var writer = new StreamWriter(AppDataFile);
            writer.Write(jsonString);
            writer.Close();
        }

        private void RemoveConfigFromFileSystem()
        {
            if (ConfigExists())
            {
                File.Delete(AppDataFile);
            }
        }

        private bool ConfigExists()
        {
            return File.Exists(AppDataFile);
        }
    }
}