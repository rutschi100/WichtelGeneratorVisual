using NUnit.Framework;

namespace WichtelGenerator.Core.Test.SantaManager
{
    public class SantaManagerTests
    {
        //Given-When-Then
        [Test]
        public void SantaHasNotBeenAddedYet_SantaAdded_EventShouldInvoke()
        {
            
        }

        [Test]
        public void SantaHasNotBeenAddedYet_SantaAdded_SantaSouldBeInTheList()
        {
            
        }

        [Test]
        public void SantaIsAlreadyAdded_SantaAdded_ReturnsAlreadyExistsResult()
        {
            
        }
        
        [Test]
        public void SantaHasNotBeenAddedYet_SantaAddedWithExitsName_ReturnsAlreadyUsedResult()
        {
            
        }
        
        [Test]
        public void SantaHasNotBeenAddedYet_SantaAdded_SantaShouldBeOnOthersWhiteList()
        {
            
        }
        
        [Test]
        public void SantaHasNotBeenAddedYet_SantaAdded_SantaShouldBeOnHisOwnBlackList()
        {
            
        }
        
        [Test]
        public void MultibleSantasAdded_SantaMovedToBlackList_SantaShouldBeRemovedFromWhiteList()
        {
            
        }
        
        [Test]
        public void MultibleSantasAdded_SantaMovedToBlackList_SantaShouldHasNotZeroWhiteListSantas()
        {
            
        }
        
        [Test]
        public void MultibleSantasAdded_SantaMovedToBlackList_NotMoreThanOneShouldHasOnlyTheSameSantaOnWhiteList()
        {
            
        }
        
        [Test]
        public void MultibleSantasAdded_SantaMovedToWhiteList_SantaShouldBeRemovedOnBlackList()
        {
            
        }
        
        [Test]
        public void MultibleSantasAdded_SantaRemovedFromMember_SantaShouldBeRomovedOnEveryList()
        {
            
        }
    }
}