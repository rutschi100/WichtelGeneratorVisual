using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using WichtelGenerator.Core.Exeptions;
using WichtelGenerator.Core.Models;
using WichtelGenerator.Core.Notification;
using WichtelGenerator.Core.Services;

[assembly: InternalsVisibleTo("WichtelGenerator.Core.Test")]

namespace WichtelGenerator.Core.Lottery
{
    public class LotteryService : ILotteryService
    {
        private readonly IListMappingService _listMappingService;


        public LotteryService(
            INotificationManager notificationManager,
            IListMappingService listMappingService
        )
        {
            NotificationManager = notificationManager;
            _listMappingService = listMappingService;
        }

        private INotificationManager NotificationManager { get; }

        public IEnumerable<SecretSantaModel> Raffle(IEnumerable<SecretSantaModel> players)
        {
            var secretSantaModels = players as SecretSantaModel[] ?? players.ToArray();
            if (!AllSantasHasAname(secretSantaModels))
            {
                throw new LotteryFailedExeption("Incomplete list. Some santas do not have a name");
            }

            //--- Vorbereiten
            players = secretSantaModels.OrderByDescending(
                p => _listMappingService.GetCount(p, SecredSantaMappingType.BlackListed)
            );
            var allreadyUsed = new List<SecretSantaModel>();

            // ReSharper disable once PossibleMultipleEnumeration
            foreach (var onePlayer in players)
            {
                if (RandomRuffle(onePlayer, allreadyUsed))
                {
                    continue;
                }

                TakeTheFirstAvailable(onePlayer, allreadyUsed); //--- Emergency plan
            }

            if (HasDoubleValues(allreadyUsed))
            {
                throw new LotteryFailedExeption("Duplicate lottery drawings were identified.");
            }

            NotificationManager.SendRaffle(players);
            // ReSharper disable once PossibleMultipleEnumeration
            return players;
        }

        private bool AllSantasHasAname(IEnumerable<SecretSantaModel> list)
        {
            return list.All(oneSantaModel => !string.IsNullOrEmpty(oneSantaModel.Name));
        }

        /// <summary>
        ///     Checks if there are duplicate entries in the list
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        private bool HasDoubleValues(IEnumerable<SecretSantaModel> list)
        {
            var groupedUsed = list.GroupBy(p => p);
            return groupedUsed.Any(oneGrouop => oneGrouop.Count() > 1);
        }

        /// <summary>
        ///     Takes the first available santa and adds it as a choice
        /// </summary>
        /// <param name="player"></param>
        /// <param name="allreadyUsed"></param>
        private void TakeTheFirstAvailable(
            SecretSantaModel player,
            ICollection<SecretSantaModel> allreadyUsed
        )
        {
            foreach (var oneFromWhite in
                from oneFromWhite in _listMappingService.GetHoleList(
                    player,
                    SecredSantaMappingType.WhiteListed
                )
                let result = allreadyUsed.FirstOrDefault(
                    p => p.Equals(oneFromWhite) && oneFromWhite.Choise != p
                )
                where result == null
                select oneFromWhite)
            {
                //not yet used
                player.Choise = oneFromWhite;
                allreadyUsed.Add(oneFromWhite);
                break;
            }

            if (player.Choise == null)
            {
                throw new LotteryFailedExeption();
            }
        }

        /// <summary>
        ///     Checks if the santa occurs in the list
        /// </summary>
        /// <param name="list"></param>
        /// <param name="searchedSanta"></param>
        /// <returns></returns>
        private bool IsInSantaList(
            IEnumerable<SecretSantaModel> list,
            SecretSantaModel searchedSanta
        )
        {
            var result = list.FirstOrDefault(p => p == searchedSanta);
            return result != null;
        }

        /// <summary>
        ///     Tries to add a random santa as a choice and returns success
        /// </summary>
        /// <param name="player"></param>
        /// <param name="allreadyUsed"></param>
        /// <returns></returns>
        private bool RandomRuffle(
            SecretSantaModel player,
            ICollection<SecretSantaModel> allreadyUsed
        )
        {
            var random = new Random();
            var whiteList = _listMappingService.GetHoleList(
                player,
                SecredSantaMappingType.WhiteListed
            );

            if (!whiteList.Any())
            {
                throw new LotteryFailedExeption($"There is no Santas in Whitelist. Ruffle failed!");
            }
            
            int i;
            for (i = 0; i < whiteList.Count; i++)
            {
                var posibleChoise = whiteList[random.Next(whiteList.Count)];
                if (IsInSantaList(allreadyUsed, posibleChoise)) continue;
                if (posibleChoise.Choise == player) continue; // Do not draw each other

                //noch nicht verwendet
                player.Choise = posibleChoise;
                allreadyUsed.Add(posibleChoise);
                break;
            }

            return i < whiteList.Count;
        }
    }
}