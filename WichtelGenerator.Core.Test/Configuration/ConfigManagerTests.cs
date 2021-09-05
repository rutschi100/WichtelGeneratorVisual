using System;
using System.Collections.Generic;
using System.IO;
using NUnit.Framework;
using WichtelGenerator.Core.Configuration;
using WichtelGenerator.Core.Models;

namespace WichtelGenerator.Core.Test.Configuration
{
    public class ConfigManagerTests
    {
        public string ConfigFilePath { get; set; }
        public Factory Factory { get; set; } = new Factory();

        public ConfigModel TestModel { get; set; } = new ConfigModel
        {
            Absender = "Test Absender",
            EmpfaengerListe = new List<MailAdressModel>(),
            MailNotificationEnabled = true,
            NotificationsEnabled = true,
            Passwort = "abc",
            Port = 25,
            ServerName = "Test Server",
            SslOn = true,
            Username = "The User",
            SecretSantaModels = new List<SecretSantaModel>
            {
                new SecretSantaModel
                {
                    MailAdress = "MyMail@Test",
                    Name = "Test Santa"
                }
            }
        };

        [SetUp]
        public void Init()
        {
            ConfigFilePath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) +
                             @"\WichtelGenerator.json";

            var mail_1 = new MailAdressModel { Mail = "TestMail@Testingen" };
            var mail_2 = new MailAdressModel { Mail = "TestMail@Testingen_2" };

            TestModel.EmpfaengerListe = new List<MailAdressModel> { mail_1, mail_2 };
        }


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
            manager.SaveSettings(TestModel);
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
            manager.SaveSettings(TestModel);

            var loadedModel = manager.ReadSettings();

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
            var loadedModel = manager.ReadSettings();

            Assert.True(string.IsNullOrEmpty(loadedModel.ServerName));
        }
    }
}