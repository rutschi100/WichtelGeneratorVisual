using System;
using System.Collections.Generic;
using Autofac.Extras.Moq;
using Moq;
using NUnit.Framework;
using TimMailLib;
using WichtelGenerator.Core.Configuration;
using WichtelGenerator.Core.Exeptions;
using WichtelGenerator.Core.Models;
using WichtelGenerator.Core.Notification;

namespace WichtelGenerator.Core.Test.Notification
{
    public class NotificationMailTests
    {
        private List<SecretSantaModel> CreateSomeModels()
        {
            var firstModel = new SecretSantaModel { Name = "2" };
            var secondModel = new SecretSantaModel { Name = "1" };
            var thirtModel = new SecretSantaModel { Name = "3" };

            return new List<SecretSantaModel>
            {
                firstModel,
                secondModel,
                thirtModel
            };
        }


        [Test]
        public void GIVEN_CorrectData_WHEN_SendingRufflePerMail_THEN_SendMail()
        {
            using var mock = AutoMock.GetLoose();

            //Arrange
            mock.Mock<IConfigManager>()
                .Setup(x => x.ReadSettings())
                .Returns(
                    new ConfigModel
                    {
                        MailNotificationEnabled = true,
                        SecretSantaModels = CreateSomeModels()
                    }
                );

            mock.Mock<IConfigManager>()
                .Setup(p => p.ConfigModel)
                .Returns(mock.Create<ConfigModel>());
            mock.Mock<IMailSender>()
                .Setup(p => p.MailSettings)
                .Returns(mock.Create<MailSettings>());

            var mailer = mock.Create<NotificationMail>();

            //Act
            mailer.SendRuffleResult();

            //Assert
            // True if not throws Exception!
        }

        [Test]
        public void GIVEN_MailServerNotAvaiable_WHEN_SendingMail_THEN_ThrowsException()
        {
            using var mock = AutoMock.GetLoose();
            //Arrange

            mock.Mock<IMailSender>()
                .Setup(
                    p =>
                        p.SendMessage(It.IsAny<string>(), It.IsAny<string>())
                )
                .Throws(new Exception());

            mock.Mock<IMailSender>()
                .Setup(sender => sender.MailSettings)
                .Returns(new MailSettings());

            mock.Mock<IConfigManager>()
                .Setup(manager => manager.ReadSettings())
                .Returns(
                    new ConfigModel
                    {
                        MailNotificationEnabled = true,
                        SecretSantaModels = CreateSomeModels()
                    }
                );

            var instance = mock.Create<NotificationMail>();

            //Act
            var action = new TestDelegate(() => instance.SendRuffleResult());

            //Assert
            Assert.Throws<NotificationNotSendedException>(action);
        }
    }
}