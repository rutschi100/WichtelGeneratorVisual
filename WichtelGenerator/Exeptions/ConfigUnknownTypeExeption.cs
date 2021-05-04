using System;

namespace WichtelGenerator.Core.Exeptions
{
    public class ConfigUnknownTypeExeption : Exception
    {
        public ConfigUnknownTypeExeption(string classname) : base($"Klassen Typ {classname} ist unbekannt")
        {
        }
    }
}