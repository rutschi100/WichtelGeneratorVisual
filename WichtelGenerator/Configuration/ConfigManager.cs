using System;
using System.Data.SQLite;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;

[assembly: InternalsVisibleTo("WichtelGenerator.Core.Test")]

namespace WichtelGenerator.Core.Configuration
{
    public class ConfigManager : IConfigManager
    {
        public ConfigManager()
        {
            ConfigModel = new ConfigModel();
            if (!ConfigExists())
            {
                SaveSettings(ConfigModel);
            }
        }

        public const string DataBaseFile = "Database.db";
        
        private static void CreateDatabase()
        {

            SQLiteConnection.CreateFile(DataBaseFile);
            if (!File.Exists(DataBaseFile))
            {
                throw new Exception("Datenbank konnte nicht erstellt werden!");
            }
        }
        
        private static void MigrateEF()
        {
            using var database = new ConfigContext();
            database.Database.Migrate(); 
        }
        
        
        //Alt------------------
        private string AppDataFile { get; } = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) +
                                              @"\WichtelGenerator.json";

        public ConfigModel ConfigModel { get; set; }

        public ConfigModel ReadSettings()
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

        public void SaveSettings(ConfigModel configModel)
        {
            RemoveConfigFromFileSystem();

            var result = File.Create(AppDataFile);
            result.Close();

            if (configModel == null)
            {
                throw new Exception("ConfigModel should not be null!");
            }

            ConfigModel = configModel;
            
            //todo: Promlematik Speichern:
            // SantaModel hat listen mit santas, die Santa Models haben. Damit kommt Serialize nicht klar...
            //todo: Umschreiben um mit EF-CodeFirst zu arbeiten, da gelingt der Verweis auf sich selber!
            var jsonString = JsonSerializer.Serialize(configModel);
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