using System;
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
        // private Mock<INotificationManager> NotificationManagerMock { get; } = new Mock<INotificationManager>();
        // public Factory Factory { get; set; } = new Factory();


        [Test]
        public void GIVEN_InconsistentData_WHEN_StartRaffle_THEN_ThrowsException()
        {
            using var mock = AutoMock.GetLoose();

            #region Data Mock
            var firstModel = new SecretSantaModel();
            var secondModel = new SecretSantaModel
            {
                Name = "Test User2"
            };

            var allTestModels = new List<SecretSantaModel> { firstModel, secondModel };
            var inctance = mock.Create<LotteryService>();
            #endregion

            //Act
            var action = new TestDelegate(() => inctance.Raffle(allTestModels));

            //Assert
            Assert.Throws<LotteryFailedExeption>(action);
        }
        

        // [Test]
        // public void ShouldValidRaffle()
        // {
        //     using var mock = AutoMock.GetLoose();
        //
        //     var firstModel = new SecretSantaModel { Name = "2" };
        //     var secondModel = new SecretSantaModel { Name = "1" };
        //     var thirtModel = new SecretSantaModel { Name = "3" };
        //     firstModel.WhiteListModel.WhitList.Add(secondModel);
        //     firstModel.WhiteListModel.WhitList.Add(thirtModel);
        //     secondModel.WhiteListModel.WhitList.Add(firstModel);
        //     secondModel.WhiteListModel.WhitList.Add(thirtModel);
        //     thirtModel.WhiteListModel.WhitList.Add(firstModel);
        //     thirtModel.WhiteListModel.WhitList.Add(secondModel);
        //
        //     var allTestModels = new List<SecretSantaModel> { firstModel, secondModel, thirtModel };
        //     var service = mock.Create<LotteryService>();
        //
        //     var result = service.Raffle(allTestModels);
        //
        //     if (result.Count() != allTestModels.Count)
        //     {
        //         Assert.Fail();
        //         return;
        //     }
        //
        //     foreach (var secretSantaModel in result)
        //     {
        //         if (secretSantaModel.Choise == null)
        //         {
        //             Assert.Fail();
        //             return;
        //         }
        //
        //         if (secretSantaModel.Name != secretSantaModel.Choise.Name) continue;
        //         Assert.Fail();
        //         return;
        //     }
        //
        //     var grouped = result.GroupBy(p => p.Choise).Where(p => p.Count() > 1);
        //     if (grouped.Any())
        //     {
        //         Assert.Fail();
        //         return;
        //     }
        //
        //     //Assert.Pass($"Passed all validations");
        //     Assert.True(true);
        // }

        // [Test]
        // public void ShouldThrowExeptionCauseToLessSantas()
        // {
        //     var manager = Factory.GetConfigManager();
        //     var firstModel = new SecretSantaModel { Name = "2" };
        //
        //     var allTestModels = new List<SecretSantaModel> { firstModel };
        //
        //     var settings = manager.ReadSettings();
        //     settings.NotificationsEnabled = false;
        //     manager.SaveSettings(settings);
        //     var service = new LotteryService(NotificationManagerMock.Object);
        //     try
        //     {
        //         var result = service.Raffle(allTestModels);
        //     }
        //     catch (LotteryFailedExeption)
        //     {
        //         Assert.Pass();
        //     }
        //
        //     Assert.Fail();
        // }
    }
}