using System;
using System.Collections.Generic;
using System.Linq;
using WichtelGenerator.Core.Exeptions;
using WichtelGenerator.Core.Models;

namespace WichtelGenerator.Core.Lottery
{
    public class LotteryService : ILotteryService
    {
        public IEnumerable<SecretSantaModel> Raffle(IEnumerable<SecretSantaModel> players)
        {
            //--- Vorbereiten
            players = players.OrderByDescending(p => p.BlackList.Count);
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

            // ReSharper disable once PossibleMultipleEnumeration
            return players;
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
            foreach (var oneFromWhite in from oneFromWhite in player.WhiteList
                let result = allreadyUsed.FirstOrDefault(p => p.Equals(oneFromWhite))
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

            var i = 0;
            for (i = 0; i < player.WhiteList.Count(); i++)
            {
                var posibleChoise = player.WhiteList[random.Next(player.WhiteList.Count)];
                if (IsInSantaList(allreadyUsed, posibleChoise)) continue;

                //noch nicht verwendet
                player.Choise = posibleChoise;
                allreadyUsed.Add(posibleChoise);
                break;
            }

            return i < player.WhiteList.Count();
        }
    }
}