using System;
using System.Collections.Generic;
using Autofac.Extras.Moq;
using Moq;
using NUnit.Framework;
using WichtelGenerator.Core.Configuration;
using WichtelGenerator.Core.Models;
using WichtelGenerator.Core.Notification;

namespace WichtelGenerator.Core.Test.Notification
{
    public class NotificationManagerTests
    {
        internal Mock<INotificationMail> NotificationMailMock { get; set; } = new Mock<INotificationMail>();

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
        public void ShouldSendAll()
        {
            using var mock = AutoMock.GetLoose();
            mock.Mock<INotificationMail>().Setup(p => p.Enabled).Returns(true);
            mock.Mock<INotificationMail>().Setup(p => p.SendRuffleResult()).Returns(true);
            mock.Mock<IConfigManager>().Setup(p => p.Read()).Returns(
                new ConfigModel
                {
                    NotificationsEnabled = true
                }
            );


            mock.Mock<IConfigManager>().SetupGet(p => p.ConfigModel).Returns(
                new ConfigModel { MailNotificationEnabled = true }
            );

            var test = mock.Create<IConfigManager>();

            mock.Mock<INotification>().Setup(p => p.SendRuffleResult()).Returns(true);

            var santaList = PreparadeSantas();

            try
            {
                var manager = mock.Create<NotificationManager>();
                manager.SendRaffle(santaList);
            }
            catch (Exception)
            {
                Assert.Fail();
            }

            Assert.True(true);
        }

        [Test]
        public void ShouldThrowExeptionCouseSomethingNotSended()
        {
            using var mock = AutoMock.GetLoose();
            mock.Mock<INotificationMail>().Setup(p => p.Enabled).Returns(true);
            mock.Mock<INotificationMail>().Setup(p => p.SendRuffleResult()).Returns(false);


            var santaList = PreparadeSantas();

            try
            {
                var manager = mock.Create<NotificationManager>();
                manager.SendRaffle(santaList);
            }
            catch (Exception)
            {
                Assert.Pass();
            }

            Assert.Fail();
        }
    }
}