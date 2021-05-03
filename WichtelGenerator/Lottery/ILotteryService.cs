using System.Collections.Generic;
using WichtelGenerator.Core.Models;

namespace WichtelGenerator.Core.Lottery
{
    public interface ILotteryService
    {
        /// <summary>
        ///     Starts the raffle
        /// </summary>
        /// <param name="players"></param>
        /// <returns></returns>
        IEnumerable<SecretSantaModel> Raffle(IEnumerable<SecretSantaModel> players);
    }
}