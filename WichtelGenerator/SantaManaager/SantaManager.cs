using System;
using System.Collections.Generic;
using System.Linq;
using WichtelGenerator.Core.Enums;
using WichtelGenerator.Core.Models;

namespace WichtelGenerator.Core.SantaManaager
{
    public class SantaManager : ISantaManager
    {
        private const int MinimumUserHasOnWhiteList = 2;

        public SantaManager()
        {
#if DEBUG
            var newUser = new SecretSantaModel { Name = "User 1" };
            AddNewSanta(newUser);
            newUser = new SecretSantaModel { Name = "User 2" };
            AddNewSanta(newUser);
            newUser = new SecretSantaModel { Name = "User 3" };
            AddNewSanta(newUser);
            newUser = new SecretSantaModel { Name = "User 4" };
            AddNewSanta(newUser);
#endif
        }

        public EventHandler<EventArgs> NewUserAddedEvent { get; set; }
        public List<SecretSantaModel> SecretSantaModels { get; } = new List<SecretSantaModel>();

        public SantaBlackListWishResult AddSantaToBlackList(SecretSantaModel owner, SecretSantaModel modelToBeMoved)
        {
            var hasReachedMaximumNumber = owner.BlackListModel.BlackList.Count + 1 >= SecretSantaModels.Count;
            if (hasReachedMaximumNumber)
            {
                return SantaBlackListWishResult.MaxValueAlreadyUsed;
            }

            var result = ValidateBlackListMove(owner, modelToBeMoved);
            if (result != SantaBlackListWishResult.Valid)
            {
                return result;
            }

            MoveToBlackList(owner, modelToBeMoved);

            return result;
        }


        public void AddSantaToWhiteList(SecretSantaModel owner, SecretSantaModel modelToBeMoved)
        {
            if (owner == modelToBeMoved)
            {
                throw new Exception("A model cannot keep itself on the whitelist");
            }

            owner.BlackListModel.BlackList.Remove(modelToBeMoved);
            var selectedModel = owner.WhiteListModel.WhitList.FirstOrDefault(p => p == modelToBeMoved);
            if (selectedModel != null)
            {
                return;
            }

            owner.WhiteListModel.WhitList.Add(modelToBeMoved);
        }

        public AddUserResult AddNewSanta(SecretSantaModel model)
        {
            if (SantaAllReadyExists(model, out var santaAllReadyExists)) return santaAllReadyExists;

            AddNewModelToAllWhiteLists(model);

            SecretSantaModels.Add(model);
            model.BlackListModel.BlackList.Add(model);

            NewUserAddedEvent?.Invoke(this, EventArgs.Empty);
            return AddUserResult.Done;
        }

        public void RemoveSanta(SecretSantaModel model)
        {
            foreach (var oneModel in SecretSantaModels)
            {
                oneModel.WhiteListModel.WhitList.Remove(model);
                oneModel.BlackListModel.BlackList.Remove(model);
            }

            SecretSantaModels.Remove(model);
        }

        private SantaBlackListWishResult ValidateBlackListMove(SecretSantaModel owner, SecretSantaModel modelToBeMoved)
        {
            var result = SantaBlackListWishResult.Valid;

            var hasJustOneOnWhiteListAfterMoveing = owner.WhiteListModel.WhitList.Count - 1 == 1;
            var moveModelIsJustOnOneWhiteList = HowManySantasHasOnWhiteList(modelToBeMoved) < MinimumUserHasOnWhiteList;

            if (hasJustOneOnWhiteListAfterMoveing || moveModelIsJustOnOneWhiteList)
            {
                result = IsCobinationAlreadyUsed(owner, modelToBeMoved)
                    ? SantaBlackListWishResult.CombinationAlreadyExist
                    : SantaBlackListWishResult.Valid;
                if (result != SantaBlackListWishResult.Valid)
                {
                    return result;
                }
            }

            return result;
        }

        private void MoveToBlackList(SecretSantaModel owner, SecretSantaModel modelToBeMoved)
        {
            owner.WhiteListModel.WhitList.Remove(modelToBeMoved);
            var selected = owner.BlackListModel.BlackList.FirstOrDefault(oneModel => oneModel == modelToBeMoved);
            if (selected == null)
            {
                owner.BlackListModel.BlackList.Add(modelToBeMoved);
            }
        }

        private int HowManySantasHasOnWhiteList(SecretSantaModel model)
        {
            var result = SecretSantaModels.Where(oneSanta => oneSanta != model)
                .Count(oneSanta => oneSanta.WhiteListModel.WhitList.Any(p => p == model));
            return result;
        }

        private void AddNewModelToAllWhiteLists(SecretSantaModel model)
        {
            foreach (var oneModel in SecretSantaModels)
            {
                oneModel.WhiteListModel.WhitList.Add(model);
                model.WhiteListModel.WhitList.Add(oneModel);
            }
        }

        private bool SantaAllReadyExists(SecretSantaModel model, out AddUserResult santaAllReadyExists)
        {
            santaAllReadyExists = AddUserResult.Done;
            var selectedModel = SecretSantaModels.FirstOrDefault(p => p.Name == model.Name);
            if (selectedModel != null)
            {
                {
                    santaAllReadyExists = AddUserResult.SantaAllReadyExists;
                    return true;
                }
            }

            return false;
        }

        private bool IsCobinationAlreadyUsed(SecretSantaModel owner, SecretSantaModel modelToBeMoved)
        {
            var ownersBlackListCopy = new List<SecretSantaModel>(owner.BlackListModel.BlackList) { modelToBeMoved };
            ownersBlackListCopy = ownersBlackListCopy.OrderBy(p => p.Name).ToList();

            var result = (from otherBlackListCopy in from oneModel in SecretSantaModels
                    where oneModel != owner
                    select new List<SecretSantaModel>(oneModel.BlackListModel.BlackList)
                select otherBlackListCopy.OrderBy(p => p.Name).ToList()).Any(otherBlackListCopy =>
                HasTheListsTheSameValues(ownersBlackListCopy, otherBlackListCopy));

            return result;
        }

        private bool HasTheListsTheSameValues(List<SecretSantaModel> list1, List<SecretSantaModel> list2)
        {
            if (!list1.Any() || !list2.Any())
            {
                return false;
            }

            if (list1.Count != list2.Count)
            {
                return false;
            }

            // Funktioniert nur, wenn die Listen nach Namen Sortiert sind.
            var result = !list1.Where((t, i) => t != list2[i]).Any();
            return result;
        }
    }
}