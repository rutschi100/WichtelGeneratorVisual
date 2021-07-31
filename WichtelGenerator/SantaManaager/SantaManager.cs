using System;
using System.Collections.Generic;
using System.Linq;
using WichtelGenerator.Core.Enums;
using WichtelGenerator.Core.Models;

namespace WichtelGenerator.Core.SantaManaager
{
    public class SantaManager : ISantaManager
    {
        public EventHandler<EventArgs> NewUserAddedEvent { get; set; }
        public List<SecretSantaModel> SecretSantaModels { get; } = new List<SecretSantaModel>();

        public SantaBlackListWishResult AddSantaToBlackList(SecretSantaModel owner, SecretSantaModel modelToBeMoved)
        {
            if (owner.BlackList.Count + 1 >= SecretSantaModels.Count)
            {
                return SantaBlackListWishResult.MaxValueAlreadyUsed;
            }

            return IsKobinationAlreadyUsed(owner, modelToBeMoved)
                ? SantaBlackListWishResult.CombinationAlreadyExist
                : SantaBlackListWishResult.Valid;
        }


        public void AddSantaToWhiteList(SecretSantaModel owner, SecretSantaModel modelToBeMoved)
        {
            owner.BlackList.Remove(modelToBeMoved);
            var selectedModel = owner.WhiteList.FirstOrDefault(p => p == modelToBeMoved);
            if (selectedModel != null)
            {
                return;
            }

            owner.WhiteList.Add(modelToBeMoved);
        }

        public AddUserResult AddNewSanta(SecretSantaModel model)
        {
            var selectedModel = SecretSantaModels.FirstOrDefault(p => p == model);
            if (selectedModel != null)
            {
                return AddUserResult.SantaAllReadyExists;
            }

            foreach (var oneModel in SecretSantaModels)
            {
                oneModel.WhiteList.Add(model);
            }

            SecretSantaModels.Add(model);
            model.BlackList.Add(model);

            NewUserAddedEvent?.Invoke(this, EventArgs.Empty);
            return AddUserResult.Valid;
        }

        public void RemoveSanta(SecretSantaModel model)
        {
            foreach (var oneModel in SecretSantaModels)
            {
                oneModel.WhiteList.Remove(model);
                oneModel.BlackList.Remove(model);
            }

            SecretSantaModels.Remove(model);
        }

        private bool IsKobinationAlreadyUsed(SecretSantaModel owner, SecretSantaModel modelToBeMoved)
        {
            var ownersBlackListCopy = new List<SecretSantaModel>(owner.BlackList);
            ownersBlackListCopy.Add(modelToBeMoved);
            ownersBlackListCopy.Sort();

            foreach (var otherBlackListCopy in from oneModel in SecretSantaModels
                where oneModel != owner
                select new List<SecretSantaModel>(oneModel.BlackList))
            {
                otherBlackListCopy.Sort();

                if (HasTheListsTheSameValues(ownersBlackListCopy, otherBlackListCopy))
                {
                    return true;
                }
            }

            return false;
        }

        private bool HasTheListsTheSameValues(List<SecretSantaModel> list1, List<SecretSantaModel> list2)
        {
            if (!list1.Any() || list2.Any())
            {
                return false;
            }

            if (list1.Count != list2.Count)
            {
                return false;
            }

            return !(from oneOne in list1
                from oneTwo in list2
                where oneOne != oneTwo
                select oneOne).Any();
        }
    }
}