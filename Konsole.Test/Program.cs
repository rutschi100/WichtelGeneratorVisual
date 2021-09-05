using Microsoft.EntityFrameworkCore;
using System;
using System.Data.SQLite;
using System.IO;

namespace Konsole.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            CreateDatabase();
            MigrateEF();
        }

        public static string DataBaseFile { get; set; } = "Test.db";

        private static void CreateDatabase()
        {
            if (File.Exists(DataBaseFile))
            {
                File.Delete(DataBaseFile);
            }

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
    }
}