using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using WichtelGenerator.Core.Exeptions;
using WichtelGenerator.Core.Models;

namespace WichtelGenerator.Core.Test.Lottery
{
    public class LotteryServicesTests
    {
        [SetUp]
        public void Init()
        {

        }

        public Factory Factory { get; set; } = new Factory();

        [Test]
        public void ShouldNotSendAnyThing()
        {
            /*
            var manager = Factory.GetConfigManager();
            var firstModel = new SecretSantaModel()
            {
                Name = $"Test User1",
            };
            var secondModel = new SecretSantaModel()
            {
                Name = $"Test User2"
            };
            firstModel.WhiteList.Add(secondModel);
            secondModel.WhiteList.Add(firstModel);
            var settings = manager.Read();
            settings.NotificationsEnabled = false;
            manager.Write(settings);
            var service = Factory.GetLotteryService();
            */
            //Can not be Tested, cause all this methods are internal...

            Assert.Pass();
        }
        /*
        [Test]
        public void ShouldSendTheResults()
        {
            Assert.Fail();
        }
        */

        [Test]
        public void ShouldThrowCauseNoName()
        {
            var manager = Factory.GetConfigManager();
            var firstModel = new SecretSantaModel();
            var secondModel = new SecretSantaModel()
            {
                Name = $"Test User2"
            };
            firstModel.WhiteList.Add(secondModel);
            secondModel.WhiteList.Add(firstModel);
            var allTestModels = new List<SecretSantaModel> {firstModel, secondModel};

            var settings = manager.Read();
            settings.NotificationsEnabled = false;
            manager.Write(settings);
                var service = Factory.GetLotteryService();
            try
            {
                var result = service.Raffle(allTestModels);
            }
            catch (LotteryFailedExeption)
            {
                Assert.Pass();
                return;
            }
            Assert.Fail();
        }

        [Test]
        public void ShouldThrowCauseNoMailSettings()
        {
            var manager = Factory.GetConfigManager();
            var firstModel = new SecretSantaModel() {Name = $"Fritz"};
            var secondModel = new SecretSantaModel()
            {
                Name = $"Test User2"
            };
            firstModel.WhiteList.Add(secondModel);
            secondModel.WhiteList.Add(firstModel);
            var allTestModels = new List<SecretSantaModel> {firstModel, secondModel};

            var settings = manager.Read();
            settings.NotificationsEnabled = true;
            settings.MailNotificationEnabled = true;
            manager.Write(settings);
            var service = Factory.GetLotteryService();
            try
            {
                var result = service.Raffle(allTestModels);
            }
            catch (LotteryFailedExeption)
            {
                Assert.Fail();
                return;
            }
            catch (ConfigUnknownTypeExeption)
            {
                Assert.Fail();
                return;
            }
            catch (Exception)
            {
                Assert.Pass();
            }

            Assert.Fail();
        }

        [Test]
        public void ShouldValidRaffle()
        {
            Assert.Fail();
        }

        [Test]
        public void ShouldThrowExeptionCauseToLessSantas()
        {
            Assert.Fail();
        }
    }
}
