using System;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore;

[assembly: InternalsVisibleTo("WichtelGenerator.Core.Test")]

namespace WichtelGenerator.Core.Configuration
{
    public class ConfigManager : IConfigManager
    {
        public const string DataBaseFile = "Database.db";

        public ConfigManager()
        {
            ConfigModel = new ConfigModel();
            ReadFromDb();
        }

        public ConfigModel ConfigModel { get; set; }

        public EventHandler OnSaveEventHandler { get; set; }
        public EventHandler OnReadEventHandler { get; set; }

        public ConfigModel ReadSettings()
        {
            OnReadEventHandler?.Invoke(this, EventArgs.Empty);
            return ConfigModel;
        }

        public void SaveSettings(ConfigModel configModel)
        {
            ConfigModel = configModel;
            WriteInDb();
            OnSaveEventHandler?.Invoke(this, EventArgs.Empty);
        }

        private void ReadFromDb()
        {
            using var context = new ConfigContext();

            var results = context.ConfigModels
                .Include(p => p.SecretSantaModels)
                .ThenInclude(model => model.Choise);

            if (results.Count() > 1)
            {
                throw new Exception(
                    $"Es sind zu viele ConfigModels ({results.Count()}) gespeichert worden."
                );
            }

            ConfigModel = results.FirstOrDefault() ?? new ConfigModel();
            context.Dispose();
        }

        

        private void WriteInDb()
        {
            using var database = new ConfigContext();

            database.ConfigModels.Update(ConfigModel);
            database.SaveChanges();
            
            database.Dispose();
        }
    }
}