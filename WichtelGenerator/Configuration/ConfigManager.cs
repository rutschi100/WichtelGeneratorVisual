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
            InitEF();
        }

        public ConfigModel ConfigModel { get; set; }

        public ConfigModel ReadSettings()
        {
            return ConfigModel;
        }

        public void SaveSettings(ConfigModel configModel)
        {
            ConfigModel = configModel;
            WriteInDb();
        }

        private void InitEF()
        {
            var DbExists = File.Exists(DataBaseFile);
            CreateDatabase();
            MigrateEF();
            if (!DbExists)
            {
                WriteInDb();
            }

            ReadFromDb();
        }

        private void CreateDatabase()
        {
            if (!File.Exists(DataBaseFile))
            {
                SQLiteConnection.CreateFile(DataBaseFile);
            }

            if (!File.Exists(DataBaseFile))
            {
                throw new Exception($"Datenbank konnte nicht erstellt oder gefunden werden! {DataBaseFile}");
            }
        }

        private void MigrateEF()
        {
            using var database = new ConfigContext();
            database.Database.Migrate();
        }

        private void ReadFromDb()
        {
            using var database = new ConfigContext();

            var results = database.ConfigModels;

            if (results.Count() > 1)
            {
                throw new Exception($"Es sind zu viele ConfigModels ({results.Count()}) gespeichert worden.");
            }

            if (!results.Any())
            {
                throw new Exception("Es ist kein ConfigModel in der DB gefunden worden.");
            }

            ConfigModel = results.FirstOrDefault();
            LoadDependencies(database);
        }

        private void LoadDependencies(DbContext context)
        {
            context.Entry(ConfigModel).Collection(p=>p.SecretSantaModels).Load();
        }
        
        private void WriteInDb()
        {
            using var database = new ConfigContext();

            database.ConfigModels.Update(ConfigModel);
            database.SaveChanges();
        }
    }
}