using System;
using System.Collections.Generic;
using System.Linq;
using WichtelGenerator.Core.Configuration;
using WichtelGenerator.Core.Enums;
using WichtelGenerator.Core.Models;
using WichtelGenerator.Core.Services;

namespace WichtelGenerator.Core.SantaManager
{
    public class SantaManager : ISantaManager
    {
        private const int MinimumUserHasOnWhiteList = 2;
        private readonly IListMappingService _listMappingService;

        public SantaManager(IConfigManager configManager, IListMappingService listMappingService)
        {
            ConfigManager = configManager;
            _listMappingService = listMappingService;

            var santas = ConfigManager.ReadSettings().SecretSantaModels;
            SecretSantaModels = santas ?? new List<SecretSantaModel>();
        }

        private IConfigManager ConfigManager { get; }

        public EventHandler<EventArgs> NewUserAddedEvent { get; set; }
        public List<SecretSantaModel> SecretSantaModels { get; }


        public SantaBlackListWishResult AddSantaToBlackList(
            SecretSantaModel owner,
            SecretSantaModel modelToBeMoved
        )
        {
            var hasReachedMaximumNumber =
                _listMappingService.GetCount(owner, SecredSantaMappingType.BlackListed) + 1
                >= SecretSantaModels.Count;
            if (hasReachedMaximumNumber)
            {
                return SantaBlackListWishResult.MaxValueAlreadyUsed;
            }

            var result = ValidateBlackListMove(owner, modelToBeMoved);
            if (result != SantaBlackListWishResult.Valid)
            {
                return result;
            }

            _listMappingService.ChangeRelationship(
                owner,
                modelToBeMoved,
                SecredSantaMappingType.BlackListed
            );

            return result;
        }


        public void AddSantaToWhiteList(SecretSantaModel owner, SecretSantaModel modelToBeMoved)
        {
            if (owner == modelToBeMoved)
            {
                throw new Exception("A model cannot keep itself on the whitelist");
            }

            _listMappingService.ChangeRelationship(
                owner,
                modelToBeMoved,
                SecredSantaMappingType.WhiteListed
            );
        }

        public AddUserResult AddNewSanta(SecretSantaModel model)
        {
            if (SantaAllReadyExists(model, out var santaAllReadyExists)) return santaAllReadyExists;

            AddNewModelToAllWhiteLists(model);

            SecretSantaModels.Add(model);
            _listMappingService.ChangeRelationship(
                model,
                model,
                SecredSantaMappingType.BlackListed
            );

            NewUserAddedEvent?.Invoke(this, EventArgs.Empty);
            return AddUserResult.Done;
        }

        public void RemoveSanta(SecretSantaModel model)
        {
            _listMappingService.RemoveSanta(model);
            SecretSantaModels.Remove(model);
        }

        private SantaBlackListWishResult ValidateBlackListMove(
            SecretSantaModel owner,
            SecretSantaModel modelToBeMoved
        )
        {
            var result = SantaBlackListWishResult.Valid;

            var hasJustOneOnWhiteListAfterMoveing =
                _listMappingService.GetCount(owner, SecredSantaMappingType.WhiteListed) - 1 == 1;
            var moveModelIsJustOnOneWhiteList = _listMappingService.CountOfAppearOnList(
                                                    modelToBeMoved,
                                                    SecredSantaMappingType.WhiteListed
                                                )
                                                < MinimumUserHasOnWhiteList;

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

        private void AddNewModelToAllWhiteLists(SecretSantaModel newModel)
        {
            foreach (var oneModel in SecretSantaModels)
            {
                _listMappingService.ChangeRelationship(
                    oneModel,
                    newModel,
                    SecredSantaMappingType.WhiteListed
                );

                _listMappingService.ChangeRelationship(
                    newModel,
                    oneModel,
                    SecredSantaMappingType.WhiteListed
                );
            }
        }

        private bool SantaAllReadyExists(
            SecretSantaModel model,
            out AddUserResult santaAllReadyExists
        )
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

        private bool IsCobinationAlreadyUsed(
            SecretSantaModel owner,
            SecretSantaModel modelToBeMoved
        )
        {
            var ownersBlackListCopy = _listMappingService.GetHoleList(
                owner,
                SecredSantaMappingType.BlackListed
            );
            ownersBlackListCopy.Add(modelToBeMoved); // Just as Fake to Check posible Values.

            ownersBlackListCopy = ownersBlackListCopy.OrderBy(p => p.Name).ToList();

            var result = (
                from otherBlackListCopy in
                    from oneModel in SecretSantaModels
                    where oneModel != owner
                    select _listMappingService.GetHoleList(
                        oneModel,
                        SecredSantaMappingType.BlackListed,
                        true
                    )
                select otherBlackListCopy.OrderBy(p => p.Name).ToList()).Any(
                otherBlackListCopy =>
                    HasTheListsTheSameValues(ownersBlackListCopy, otherBlackListCopy)
            );

            return result;
        }

        private bool HasTheListsTheSameValues(
            List<SecretSantaModel> list1,
            List<SecretSantaModel> list2
        )
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