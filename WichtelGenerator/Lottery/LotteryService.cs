using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using WichtelGenerator.Core.Exeptions;
using WichtelGenerator.Core.Models;
using WichtelGenerator.Core.Notification;

[assembly: InternalsVisibleTo("WichtelGenerator.Core.Test")]

namespace WichtelGenerator.Core.Lottery
{
    public class LotteryService : ILotteryService
    {
        public LotteryService(INotificationManager notificationManager)
        {
            NotificationManager = notificationManager;
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
            players = secretSantaModels.OrderByDescending(p => p.BlackListModel.BlackList.Count);
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
        private void TakeTheFirstAvailable(SecretSantaModel player, ICollection<SecretSantaModel> allreadyUsed)
        {
            foreach (var oneFromWhite in from oneFromWhite in player.WhiteListModel.WhitList
                let result = allreadyUsed.FirstOrDefault(p => p.Equals(oneFromWhite) && oneFromWhite.Choise != p)
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
        private bool IsInSantaList(IEnumerable<SecretSantaModel> list, SecretSantaModel searchedSanta)
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
        private bool RandomRuffle(SecretSantaModel player, ICollection<SecretSantaModel> allreadyUsed)
        {
            var random = new Random();

            int i;
            for (i = 0; i < player.WhiteListModel.WhitList.Count; i++)
            {
                var posibleChoise = player.WhiteListModel.WhitList[random.Next(player.WhiteListModel.WhitList.Count)];
                if (IsInSantaList(allreadyUsed, posibleChoise)) continue;
                if (posibleChoise.Choise == player) continue; // Do not draw each other


                //noch nicht verwendet
                player.Choise = posibleChoise;
                allreadyUsed.Add(posibleChoise);
                break;
            }

            return i < player.WhiteListModel.WhitList.Count;
        }
    }
}