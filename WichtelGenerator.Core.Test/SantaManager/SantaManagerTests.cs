using System;
using System.Linq;
using Autofac.Extras.Moq;
using NUnit.Framework;
using WichtelGenerator.Core.Configuration;
using WichtelGenerator.Core.Enums;
using WichtelGenerator.Core.Models;

namespace WichtelGenerator.Core.Test.SantaManager
{
    public class SantaManagerTests
    {
        private const string TestSantaName_1 = "User 1";
        private const string TestSantaName_2 = "User 2";
        private const string TestSantaName_3 = "User 3";

        private SantaManaager.SantaManager CreateManagerWithTestData()
        {
            var manager = new SantaManaager.SantaManager(new ConfigManager());
            manager.AddNewSanta(new SecretSantaModel { Name = "User 1" });
            manager.AddNewSanta(new SecretSantaModel { Name = "User 2" });
            manager.AddNewSanta(new SecretSantaModel { Name = "User 3" });
            manager.AddNewSanta(new SecretSantaModel { Name = "User 4" });

            return manager;
        }

        //Given-When-Then
        [Test]
        public void SantaHasNotBeenAddedYet_SantaAdded_EventShouldInvoke()
        {
            using var mock = AutoMock.GetLoose();
            var magager = mock.Create<SantaManaager.SantaManager>();
            var eventRaised = false;
            magager.NewUserAddedEvent += (sender, e) => { eventRaised = true; };

            magager.AddNewSanta(new SecretSantaModel { Name = "Hans" });

            Assert.True(eventRaised);
        }

        [Test]
        public void SantaHasNotBeenAddedYet_SantaAdded_SantaSouldBeInTheList()
        {
            using var mock = AutoMock.GetLoose();
            var manager = mock.Create<SantaManaager.SantaManager>();
            var santa = new SecretSantaModel { Name = "Hans" };

            manager.AddNewSanta(santa);

            Assert.True(manager.SecretSantaModels.Any(p => p == santa));
        }

        [Test]
        public void SantaIsAlreadyAdded_SantaAdded_ReturnsAlreadyExistsResult()
        {
            var manager = CreateManagerWithTestData();
            var santa = new SecretSantaModel { Name = "Hans" };
            manager.AddNewSanta(santa);

            var result = manager.AddNewSanta(santa);

            Assert.True(result == AddUserResult.SantaAllReadyExists);
        }

        [Test]
        public void SantaHasNotBeenAddedYet_SantaAddedWithExitsName_ReturnsAlreadyUsedResult()
        {
            using var mock = AutoMock.GetLoose();
            var manager = mock.Create<SantaManaager.SantaManager>();
            var santa = new SecretSantaModel { Name = "Hans" };
            manager.AddNewSanta(santa);
            var secondSanta = new SecretSantaModel { Name = "Hans" };

            var result = manager.AddNewSanta(secondSanta);

            Assert.True(result == AddUserResult.SantaAllReadyExists);
        }

        [Test]
        public void SantaHasNotBeenAddedYet_SantaAdded_SantaShouldBeOnOthersWhiteList()
        {
            using var mock = AutoMock.GetLoose();
            var manager = CreateManagerWithTestData();
            var santa = new SecretSantaModel { Name = "Hans" };

            manager.AddNewSanta(santa);

            var isOnEveryList = manager.SecretSantaModels.Where(oneModel => oneModel != santa)
                .All(oneModel => oneModel.WhiteListModel.WhitList.Any(p => p == santa));
            Assert.True(isOnEveryList);
        }

        [Test]
        public void SantaHasNotBeenAddedYet_SantaAdded_SantaShouldBeOnHisOwnBlackList()
        {
            using var mock = AutoMock.GetLoose();
            var manager = mock.Create<SantaManaager.SantaManager>();
            var santa = new SecretSantaModel { Name = "Hans" };

            manager.AddNewSanta(santa);

            var isOnHisOwnBlackList = santa.BlackListModel.BlackList.Any(p => p == santa);
            Assert.True(isOnHisOwnBlackList);
        }

        [Test]
        public void MultibleSantasAdded_SantaMovedToBlackList_SantaShouldBeRemovedFromWhiteList()
        {
            var manager = CreateManagerWithTestData();
            var firstSanta = manager.SecretSantaModels.FirstOrDefault(p => p.Name == TestSantaName_1);
            var secondSanta = manager.SecretSantaModels.FirstOrDefault(p => p.Name == TestSantaName_2);

            manager.AddSantaToBlackList(firstSanta, secondSanta);

            var moved = ((firstSanta?.WhiteListModel.WhitList) ?? throw new InvalidOperationException()).All(p => p != secondSanta);
            Assert.True(moved);
        }

        [Test]
        public void MultibleSantasAdded_SantaMovedToBlackList_SantaShouldHasNotZeroWhiteListSantas()
        {
            using var mock = AutoMock.GetLoose();
            var manager = mock.Create<SantaManaager.SantaManager>();
            manager.SecretSantaModels.Clear(); // Wegen GUI werden Test Daten hinzugefügt.
            var firstSanta = new SecretSantaModel { Name = TestSantaName_1 };
            var secondSanta = new SecretSantaModel { Name = TestSantaName_2 };
            manager.AddNewSanta(firstSanta);
            manager.AddNewSanta(secondSanta);

            var result = manager.AddSantaToBlackList(firstSanta, secondSanta);

            Assert.True(result == SantaBlackListWishResult.MaxValueAlreadyUsed);
        }

        [Test]
        public void MultibleSantasAdded_SantaMovedToBlackList_NotMoreThanOneShouldHasOnlyTheSameSantaOnWhiteList()
        {
            using var mock = AutoMock.GetLoose();
            var manager = mock.Create<SantaManaager.SantaManager>();
            manager.SecretSantaModels.Clear(); // Wegen GUI werden Test Daten hinzugefügt.
            var firstSanta = new SecretSantaModel { Name = TestSantaName_1 };
            var lastWhiteListSanta = new SecretSantaModel { Name = TestSantaName_2 };
            var thirdSanta = new SecretSantaModel { Name = TestSantaName_3 };
            manager.AddNewSanta(firstSanta);
            manager.AddNewSanta(lastWhiteListSanta);
            manager.AddNewSanta(thirdSanta);
            manager.AddSantaToBlackList(firstSanta, thirdSanta);

            var result = manager.AddSantaToBlackList(thirdSanta, firstSanta);

            Assert.True(result == SantaBlackListWishResult.CombinationAlreadyExist);
        }

        [Test]
        public void MultibleSantasAdded_SantaMovedToWhiteList_SantaShouldBeRemovedOnBlackList()
        {
            var manager = CreateManagerWithTestData();
            var firstSanta = manager.SecretSantaModels.FirstOrDefault(p => p.Name == TestSantaName_1);
            var secondSanta = manager.SecretSantaModels.FirstOrDefault(p => p.Name == TestSantaName_2);
            manager.AddSantaToBlackList(firstSanta, secondSanta);

            manager.AddSantaToWhiteList(firstSanta, secondSanta);

            var moved = firstSanta?.WhiteListModel.WhitList.Any(p => p == secondSanta);
            Assert.True(moved);
        }

        [Test]
        public void MultibleSantasAdded_SantaRemovedFromMember_SantaShouldBeRomovedOnEveryList()
        {
            var manager = CreateManagerWithTestData();
            var firstSanta = manager.SecretSantaModels.FirstOrDefault(p => p.Name == TestSantaName_1);

            manager.RemoveSanta(firstSanta);

            var moved = false;
            foreach (var everyOnWhite in from oneModel in manager.SecretSantaModels
                let everyOnWhite = oneModel.WhiteListModel.WhitList.Any(p => p.Name == TestSantaName_1)
                let everyOnBlack = oneModel.BlackListModel.BlackList.Any(p => p.Name == TestSantaName_1)
                where !everyOnBlack && !everyOnWhite
                select everyOnWhite)
            {
                moved = true;
            }

            Assert.True(moved);
        }
    }
}