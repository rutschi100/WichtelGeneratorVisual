using System.Collections.Generic;
using System.Linq;
using Autofac.Extras.Moq;
using Moq;
using NUnit.Framework;
using WichtelGenerator.Core.Exeptions;
using WichtelGenerator.Core.Lottery;
using WichtelGenerator.Core.Models;
using WichtelGenerator.Core.Notification;

namespace WichtelGenerator.Core.Test.Lottery
{
    public class LotteryServicesTests
    {
        private Mock<INotificationManager> NotificationManagerMock { get; } = new Mock<INotificationManager>();
        public Factory Factory { get; set; } = new Factory();


        [Test]
        public void ShouldThrowCauseNoName()
        {
            var manager = Factory.GetConfigManager();
            var firstModel = new SecretSantaModel();
            var secondModel = new SecretSantaModel
            {
                Name = "Test User2"
            };
            firstModel.WhiteList.Add(secondModel);
            secondModel.WhiteList.Add(firstModel);
            var allTestModels = new List<SecretSantaModel> { firstModel, secondModel };

            var settings = manager.Read();
            settings.NotificationsEnabled = false;
            manager.Write(settings);
            var service = new LotteryService(NotificationManagerMock.Object);
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
        public void ShouldValidRaffle()
        {
            using var mock = AutoMock.GetLoose();

            var firstModel = new SecretSantaModel { Name = "2" };
            var secondModel = new SecretSantaModel { Name = "1" };
            var thirtModel = new SecretSantaModel { Name = "3" };
            firstModel.WhiteList.Add(secondModel);
            firstModel.WhiteList.Add(thirtModel);
            secondModel.WhiteList.Add(firstModel);
            secondModel.WhiteList.Add(thirtModel);
            thirtModel.WhiteList.Add(firstModel);
            thirtModel.WhiteList.Add(secondModel);

            var allTestModels = new List<SecretSantaModel> { firstModel, secondModel, thirtModel };
            var service = mock.Create<LotteryService>();

            var result = service.Raffle(allTestModels);

            if (result.Count() != allTestModels.Count)
            {
                Assert.Fail();
                return;
            }

            foreach (var secretSantaModel in result)
            {
                if (secretSantaModel.Choise == null)
                {
                    Assert.Fail();
                    return;
                }

                if (secretSantaModel.Name != secretSantaModel.Choise.Name) continue;
                Assert.Fail();
                return;
            }

            var grouped = result.GroupBy(p => p.Choise).Where(p => p.Count() > 1);
            if (grouped.Any())
            {
                Assert.Fail();
                return;
            }

            //Assert.Pass($"Passed all validations");
            Assert.True(true);
        }

        [Test]
        public void ShouldThrowExeptionCauseToLessSantas()
        {
            var manager = Factory.GetConfigManager();
            var firstModel = new SecretSantaModel { Name = "2" };

            var allTestModels = new List<SecretSantaModel> { firstModel };

            var settings = manager.Read();
            settings.NotificationsEnabled = false;
            manager.Write(settings);
            var service = new LotteryService(NotificationManagerMock.Object);
            try
            {
                var result = service.Raffle(allTestModels);
            }
            catch (LotteryFailedExeption)
            {
                Assert.Pass();
            }

            Assert.Fail();
        }
    }
}