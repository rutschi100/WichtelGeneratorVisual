using System;
using System.Collections.Generic;
using Autofac.Extras.Moq;
using NUnit.Framework;
using WichtelGenerator.Core.Configuration;
using WichtelGenerator.Core.Models;
using WichtelGenerator.Core.Notification;

namespace WichtelGenerator.Core.Test.Notification
{
    public class NotificationManagerTests
    {
        private IEnumerable<SecretSantaModel> PreparadeSantas()
        {
            var santaList = new List<SecretSantaModel>();
            var firstSanta = new SecretSantaModel();
            var secondSanta = new SecretSantaModel();
            firstSanta.Choise = secondSanta;
            secondSanta.Choise = firstSanta;
            firstSanta.MailAdress = "bla@bla";
            secondSanta.MailAdress = "bla@bla";
            firstSanta.Name = "User 1";
            secondSanta.Name = "User 2";
            santaList.Add(firstSanta);
            santaList.Add(secondSanta);

            return santaList;
        }

        [Test]
        public void GIVEN_ABunchOfNotificationToSend_WHEN_SendingAll_THEN_ShouldSendSuccessful()
        {
            using var mock = AutoMock.GetLoose();
            //Arrange

            mock.Mock<INotificationMail>().Setup(p => p.Enabled).Returns(true);
            mock.Mock<IConfigManager>()
                .Setup(p => p.ReadSettings())
                .Returns(new ConfigModel { NotificationsEnabled = true });


            mock.Mock<IConfigManager>()
                .SetupGet(p => p.ConfigModel)
                .Returns(new ConfigModel { MailNotificationEnabled = true });

            var santaList = PreparadeSantas();
            var manager = mock.Create<NotificationManager>();

            //Act
            var action = new TestDelegate(() => manager.SendRaffle(santaList));

            //Assert
            Assert.DoesNotThrow(action);
        }

        [Test]
        public void GIVEN_SomeThingCanNotBeSended_WHEN_SendingAll_THEN_ThrowException()
        {
            using var mock = AutoMock.GetLoose();
            //Arrange

            mock.Mock<INotificationMail>().Setup(p => p.Enabled).Returns(true);

            mock.Mock<INotificationMail>()
                .Setup(mail => mail.SendRuffleResult())
                .Throws<Exception>();

            mock.Mock<IConfigManager>()
                .Setup(configManager => configManager.ReadSettings())
                .Returns(new ConfigModel { NotificationsEnabled = true });

            var santaList = PreparadeSantas();

            var manager = mock.Create<NotificationManager>();
            //Act
            var action = new TestDelegate(() => manager.SendRaffle(santaList));

            //Assert
            Assert.Throws<Exception>(action);
        }


        [Test]
        public void ShouldThrowExeptionCouseSomethingNotSended()
        {
            using var mock = AutoMock.GetLoose();
        }
    }
}