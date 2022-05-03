using System.Collections.Generic;
using System.Linq;
using Autofac.Extras.Moq;
using Moq;
using NUnit.Framework;
using WichtelGenerator.Core.Exeptions;
using WichtelGenerator.Core.Lottery;
using WichtelGenerator.Core.Models;
using WichtelGenerator.Core.Services;

namespace WichtelGenerator.Core.Test.Lottery
{
    public class LotteryServicesTests
    {
        [Test]
        public void GIVEN_InconsistentData_WHEN_StartRaffle_THEN_ThrowsException()
        {
            using var mock = AutoMock.GetLoose();

            #region Data Mock

            var firstModel = new SecretSantaModel();
            var secondModel = new SecretSantaModel {Name = "Test User2"};

            var allTestModels = new List<SecretSantaModel>
            {
                firstModel,
                secondModel
            };
            var inctance = mock.Create<LotteryService>();

            #endregion

            //Act
            var action = new TestDelegate(() => inctance.Raffle(allTestModels));

            //Assert
            Assert.Throws<LotteryFailedExeption>(action);
        }


        [Test]
        public void GIVEN_CorrectData_WHEN_StartRaffle_THEN_GiveResults()
        {
            using var mock = AutoMock.GetLoose();
            //Arrange

            #region Data Mock

            var firstModel = new SecretSantaModel {Name = "2"};
            var secondModel = new SecretSantaModel {Name = "1"};
            var thirtModel = new SecretSantaModel {Name = "3"};

            var allTestModels = new List<SecretSantaModel>
            {
                firstModel,
                secondModel,
                thirtModel
            };

            #endregion

            #region Mock Returns

            mock.Mock<IListMappingService>()
                .Setup(
                    mappingService => mappingService.GetCount(
                        It.IsAny<SecretSantaModel>(),
                        It.IsAny<SecredSantaMappingType>()
                    )
                )
                .Returns(1);

            mock.Mock<IListMappingService>()
                .Setup(
                    mappingService => mappingService.GetHoleList(
                        firstModel,
                        SecredSantaMappingType.WhiteListed,
                        It.IsAny<bool>()
                    )
                )
                .Returns(
                    new List<SecretSantaModel>
                    {
                        secondModel,
                        thirtModel
                    }
                );

            mock.Mock<IListMappingService>()
                .Setup(
                    mappingService => mappingService.GetHoleList(
                        secondModel,
                        SecredSantaMappingType.WhiteListed,
                        It.IsAny<bool>()
                    )
                )
                .Returns(
                    new List<SecretSantaModel>
                    {
                        firstModel,
                        thirtModel
                    }
                );


            mock.Mock<IListMappingService>()
                .Setup(
                    mappingService => mappingService.GetHoleList(
                        thirtModel,
                        SecredSantaMappingType.WhiteListed,
                        It.IsAny<bool>()
                    )
                )
                .Returns(
                    new List<SecretSantaModel>
                    {
                        secondModel,
                        firstModel
                    }
                );

            #endregion

            var service = mock.Create<LotteryService>();

            //Act
            var result = service.Raffle(allTestModels);

            //Assert
            Assert.AreEqual(result.Count(), allTestModels.Count);
            Assert.False(result.Any(model => model.Choise == null));
            Assert.False(result.Any(model => model.Name == model.Choise.Name));
            var grouped = result.GroupBy(p => p.Choise);
            Assert.False(grouped.Any(models => models.Count() > 1));
        }


        [Test]
        public void GIVEN_toLessSantas_WHEN_StartRuffle_THEN_ThrowsException()
        {
            using var mock = AutoMock.GetLoose();
            //Arrange

            #region Data Mock

            var firstModel = new SecretSantaModel {Name = "2"};

            mock.Mock<IListMappingService>()
                .Setup(
                    mappingService => mappingService.GetCount(
                        It.IsAny<SecretSantaModel>(),
                        It.IsAny<SecredSantaMappingType>()
                    )
                )
                .Returns(1);

            mock.Mock<IListMappingService>()
                .Setup(
                    mappingService => mappingService.GetHoleList(
                        firstModel,
                        SecredSantaMappingType.WhiteListed,
                        It.IsAny<bool>()
                    )
                )
                .Returns(new List<SecretSantaModel>());

            #endregion

            var allTestModels = new List<SecretSantaModel> {firstModel};
            var instance = mock.Create<LotteryService>();

            //Act
            var action = new TestDelegate(() => instance.Raffle(allTestModels));

            //Assert
            Assert.Throws<LotteryFailedExeption>(action);
        }
    }
}