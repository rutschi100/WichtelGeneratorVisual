using System;
using System.Collections.Generic;
using System.IO;
using NUnit.Framework;
using SimpleInjector;
using WichtelGenerator.Core.Configuration;
using WichtelGenerator.Core.Models;
using WichtelGenerator.Core.Notification;

namespace WichtelGenerator.Core.Test.Configuration
{
    public class ConfigManagerTests
    {
        [SetUp]
        public void Init()
        {
            ConfigFilePath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) +
                             @"\WichtelGenerator.json";
        }


        public string ConfigFilePath { get; set; }
        public Factory Factory { get; set; } = new Factory();

        public ConfigModel TestModel { get; set; } = new ConfigModel()
        {
            Absender = $"Test Absender",
            EmpfaengerListe = new List<string>()
            {
                $"Ein Absender",
                $"Zwei Absender"
            },
            MailNotificationEnabled = true,
            NotificationsEnabled = true,
            Passwort = $"abc",
            Port = 25,
            ServerName = $"Test Server",
            SslOn = true,
            Username = $"The User",
            SecretSantaModels = new List<SecretSantaModel>()
            {
                new SecretSantaModel()
                {
                    MailAdress = $"MyMail@Test",
                    Name = $"Test Santa",
                }
            }
        };


        private bool ConifigFileExists()
        {
            return File.Exists(ConfigFilePath);
        }

        private void RemoveConfigs()
        {
            if (ConifigFileExists())
            {
                File.Delete(ConfigFilePath);
            }
        }

        [Test]
        public void ConfigsShouldBeSaved()
        {
            RemoveConfigs();
            var manager = Factory.GetConfigManager();
            manager.Write(TestModel);
            Assert.True(ConifigFileExists());
        }

        [Test]
        public void ConfigsShouldNotBeSaved()
        {
            RemoveConfigs();

            Assert.False(ConifigFileExists());
        }

        [Test]
        public void ConfigsShouldBeLoadet()
        {
            var manager = Factory.GetConfigManager();
            manager.Write(TestModel);

            var loadedModel = manager.Read();

            if (loadedModel == null)
            {
                Assert.Fail();
            }

            Assert.True(loadedModel.Passwort.Equals(TestModel.Passwort));
        }

        [Test]
        public void ConfigShouldNotBeLoadet()
        {
            RemoveConfigs();
            var manager = Factory.GetConfigManager();
            var loadedModel = manager.Read();

            Assert.True(string.IsNullOrEmpty(loadedModel.ServerName));
        }


    }
}
