using Autofac.Extras.Moq;
using NUnit.Framework;
using WichtelGenerator.Core.Configuration;

namespace WichtelGenerator.Core.Test.Configuration
{
    public class ConfigManagerTests
    {
        [Test]
        public void GIVEN_UsedConfiguration_WHEN_StartSaving_THEN_GivePositivFeedBack()
        {
            using var mock = AutoMock.GetLoose();
            //Arrange

            #region Data Mock

            var manager = mock.Create<ConfigManager>();
            var setting = manager.ReadSettings();

            setting.Port = 22;

            #endregion

            //Act
            manager.SaveSettings(setting);

            manager = mock.Create<ConfigManager>();
            setting = manager.ReadSettings();

            //Assert
            Assert.AreEqual(22, setting.Port);
        }
    }
}