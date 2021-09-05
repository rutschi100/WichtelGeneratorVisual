using Microsoft.EntityFrameworkCore;

namespace WichtelGenerator.Core.Configuration
{
    public class ConfigContext : DbContext
    {
        public DbSet<ConfigModel> ConfigModels { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder dbContextOptionsBuilder)
        {
            dbContextOptionsBuilder.UseSqlite($"Data Source={ConfigManager.DataBaseFile}");
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            /*
                Problematik:
                    EF erkennt List<> nicht an
                    EF kommt nicht klar mit referenzierung auf sich selbst in ausnahme wie hier (ungetestet) siehe:
                        https://medium.com/@dmitry.pavlov/tree-structure-in-ef-core-how-to-configure-a-self-referencing-table-and-use-it-53effad60bf

                Recherche:
                    -   Gem. Google, Braucht es für List<> jeweils ein Objekt was eine ID hat, um eine Tabelle darzustellen.
                        Das Ding ist nur, dass SantaModel eine solches Objekt ist... Trotzdem klappt es nicht.


                Lösungen:
                    1. SantaModel wird umgeschrieben und ale referenzen auf Santamodels werden mit Guid erstellt.
                        --> Vorteil: EF kann beibehalten werden und es ist zukünftiger flexibler.
                        --> Inpact: Ganzes Backend etc. muss umgeschrieben werden. Ink. Tests

                    2. Datenbank schema wird selbst geschrieben.
                        --> Vorteil: Aktuelle Code Struktur kann beibehalten werden.
                        --> Inpact: Das ganze Mapping muss händisch erstellt werden und ist künftig unflexibel.

                    3. Speicherung wird es für den Wichtel Generator nie geben.
                        --> Vorteil: Aktuelle Software kann dennoch genutzt werden und verzichtet lediglich auf ein Feature
                        --> Inpact: Falls künftig Interesse besteht an dem Tool, so muss man auf eines der anderen Lösungen zurückgreifen.
            */
        }
    }
}