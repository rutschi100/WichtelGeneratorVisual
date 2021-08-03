using System;
using System.Collections.Generic;
using Autofac;
using Autofac.Extras.Moq;
using Moq;
using NUnit.Framework;
using TimMailLib;
using WichtelGenerator.Core.Configuration;
using WichtelGenerator.Core.Models;
using WichtelGenerator.Core.Notification;

namespace WichtelGenerator.Core.Test.Notification
{
    public class NotificationMailTests
    {
        public Factory Factory { get; set; } = new Factory();

        public List<SecretSantaModel> CreateSomeModels()
        {
            var firstModel = new SecretSantaModel { Name = "2" };
            var secondModel = new SecretSantaModel { Name = "1" };
            var thirtModel = new SecretSantaModel { Name = "3" };
            firstModel.WhiteList.Add(secondModel);
            firstModel.WhiteList.Add(thirtModel);
            secondModel.WhiteList.Add(firstModel);
            secondModel.WhiteList.Add(thirtModel);
            thirtModel.WhiteList.Add(firstModel);
            thirtModel.WhiteList.Add(secondModel);

            return new List<SecretSantaModel> { firstModel, secondModel, thirtModel };
        }


        [Test]
        public void ShouldSendAnMail()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<ConfigModel>().PropertiesAutowired();
            builder.Build();
            using var mock = AutoMock.GetLoose();
            //ConfigManager.ConfigModel.MailNotificationEnabled;


            mock.Mock<IConfigManager>().Setup(x => x.Read()).Returns(
                new ConfigModel
                {
                    MailNotificationEnabled = true,
                    SecretSantaModels = CreateSomeModels()
                }
            );


            mock.Mock<IConfigManager>().Setup(p => p.ConfigModel).Returns(mock.Create<ConfigModel>());
            mock.Mock<IMailSender>().Setup(p => p.MailSettings).Returns(mock.Create<MailSettings>());


            var mailer = mock.Create<NotificationMail>();

            var sended = mailer.SendRuffleResult();

            Assert.True(sended);
        }

        [Test]
        public void ShouldNotSendAMail()
        {
            var manager = Factory.GetConfigManager();
            var mailMock = new Mock<IMailSender>();
            mailMock.Setup(p =>
                p.SendMessage(It.IsAny<string>(), It.IsAny<string>())).Throws(new Exception());

            var mailer = new NotificationMail(manager, mailMock.Object);
            var sended = mailer.SendRuffleResult();

            Assert.False(sended);
        }
    }
}